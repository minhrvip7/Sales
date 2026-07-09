import React, { useState } from 'react';
import { Row, Col, Card, Form, Select, Button, Table, InputNumber, Input, Divider, message } from 'antd';
import { DeleteOutlined, ShoppingCartOutlined } from '@ant-design/icons';
import { useGetProductsQuery } from '../../services/api/productApi';
import { useCreateOrderMutation } from '../../services/api/orderApi';
import type { CreateOrderDto, CreateOrderDetailDto, Product } from '../../types';
import { useNavigate } from 'react-router-dom';
import { PaginatedSelect } from '../../components/PaginatedSelect';
import apiClient from '../../services/apiClient';

interface SelectedItem {
  key: string; // Product ID
  product: Product;
  quantity: number;
  discountPercentage: number;
  selectedUnitId: string;
  conversionRate: number;
  unitPrice: number;
}

export const CreateOrder: React.FC = () => {
  const navigate = useNavigate();
  const { data: productsResponse } = useGetProductsQuery();
  const [createOrder] = useCreateOrderMutation();

  const products = productsResponse?.data?.data || [];
  const [selectedItems, setSelectedItems] = useState<SelectedItem[]>([]);
  const [form] = Form.useForm();
  const [localProducts, setLocalProducts] = useState<Product[]>([]);

  // Calculate totals
  const subTotal = selectedItems.reduce((sum, item) => {
    const itemSubtotal = item.quantity * item.unitPrice;
    const discount = itemSubtotal * (item.discountPercentage / 100);
    return sum + (itemSubtotal - discount);
  }, 0);

  const taxAmount = subTotal * 0.1; // 10% VAT
  const totalAmount = subTotal + taxAmount;

  const handleAddProduct = (productId: string) => {
    const product = localProducts.find(p => p.id === productId) || products.find(p => p.id === productId);
    if (!product) return;

    // Check if stock is available
    if (product.stockQuantity <= 0) {
      message.error('Sản phẩm này đã hết hàng trong kho.');
      return;
    }

    // Check if already added
    const existing = selectedItems.find(item => item.key === productId);
    if (existing) {
      const addedBaseQty = existing.conversionRate;
      const currentBaseQty = existing.quantity * existing.conversionRate;
      if (currentBaseQty + addedBaseQty > product.stockQuantity) {
        message.error(`Không thể thêm. Tồn kho tối đa của sản phẩm này là ${product.stockQuantity} ${product.baseUnit?.name || 'Lon'}.`);
        return;
      }
      setSelectedItems(selectedItems.map(item => 
        item.key === productId ? { ...item, quantity: item.quantity + 1 } : item
      ));
    } else {
      setSelectedItems([...selectedItems, {
        key: productId,
        product,
        quantity: 1,
        discountPercentage: 0,
        selectedUnitId: product.baseUnitId,
        conversionRate: 1,
        unitPrice: product.price
      }]);
    }
  };

  const handleBarcodeScan = (barcode: string) => {
    if (!barcode) return;
    let foundProduct: Product | undefined;
    let foundUnitId: string | undefined;
    let conversionRate = 1;
    let unitPrice = 0;

    for (const p of [...products, ...localProducts]) {
      if (p.barcode === barcode) {
        foundProduct = p;
        foundUnitId = p.baseUnitId;
        conversionRate = 1;
        unitPrice = p.price;
        break;
      }
      const conv = p.conversions?.find(c => c.barcode === barcode);
      if (conv) {
        foundProduct = p;
        foundUnitId = conv.alternativeUnitId;
        conversionRate = conv.conversionRate;
        unitPrice = conv.price !== null && conv.price !== undefined ? conv.price : (p.price * conv.conversionRate);
        break;
      }
    }

    if (!foundProduct) {
      message.error(`Không tìm thấy sản phẩm với mã vạch: ${barcode}`);
      return;
    }

    if (foundProduct.stockQuantity < conversionRate) {
      message.error(`Sản phẩm '${foundProduct.name}' không đủ tồn kho.`);
      return;
    }

    const existing = selectedItems.find(item => item.key === foundProduct!.id);
    if (existing) {
      const targetUnitId = foundUnitId!;
      const targetRate = conversionRate;
      const targetPrice = unitPrice;

      const isSameUnit = existing.selectedUnitId === targetUnitId;
      const newQty = isSameUnit ? existing.quantity + 1 : 1;
      const requiredBaseQty = newQty * targetRate;

      if (requiredBaseQty > foundProduct.stockQuantity) {
        message.error(`Không đủ tồn kho (Yêu cầu quy đổi: ${requiredBaseQty} ${foundProduct.baseUnit?.name}, hiện có: ${foundProduct.stockQuantity}).`);
        return;
      }

      setSelectedItems(selectedItems.map(item => 
        item.key === foundProduct!.id 
          ? { ...item, selectedUnitId: targetUnitId, conversionRate: targetRate, unitPrice: targetPrice, quantity: newQty } 
          : item
      ));
      message.success(`Đã thêm 1 ${isSameUnit ? '' : (foundProduct.conversions?.find(c => c.alternativeUnitId === targetUnitId)?.alternativeUnitName || 'đơn vị mới')} sản phẩm ${foundProduct.name}.`);
    } else {
      setSelectedItems([...selectedItems, {
        key: foundProduct.id,
        product: foundProduct,
        quantity: 1,
        discountPercentage: 0,
        selectedUnitId: foundUnitId!,
        conversionRate,
        unitPrice
      }]);
      message.success(`Đã thêm sản phẩm ${foundProduct.name} vào giỏ.`);
    }
  };

  const handleUnitChange = (productId: string, unitId: string) => {
    setSelectedItems(selectedItems.map(item => {
      if (item.key !== productId) return item;

      const product = item.product;
      let conversionRate = 1;
      let unitPrice = product.price;

      if (unitId === product.baseUnitId) {
        conversionRate = 1;
        unitPrice = product.price;
      } else {
        const conv = product.conversions?.find(c => c.alternativeUnitId === unitId);
        if (conv) {
          conversionRate = conv.conversionRate;
          unitPrice = conv.price !== null && conv.price !== undefined ? conv.price : (product.price * conv.conversionRate);
        }
      }

      const requiredBaseQty = item.quantity * conversionRate;
      if (requiredBaseQty > product.stockQuantity) {
        const maxQty = Math.floor(product.stockQuantity / conversionRate);
        if (maxQty <= 0) {
          message.error(`Không đủ tồn kho để chuyển sang đơn vị này (Tồn kho: ${product.stockQuantity} ${product.baseUnit?.name || 'Lon'}).`);
          return item;
        }
        message.warning(`Số lượng được điều chỉnh giảm xuống ${maxQty} để vừa với tồn kho tối đa.`);
        return {
          ...item,
          selectedUnitId: unitId,
          conversionRate,
          unitPrice,
          quantity: maxQty
        };
      }

      return {
        ...item,
        selectedUnitId: unitId,
        conversionRate,
        unitPrice
      };
    }));
  };

  const handleRemoveItem = (productId: string) => {
    setSelectedItems(selectedItems.filter(item => item.key !== productId));
  };

  const handleQuantityChange = (productId: string, value: number | null) => {
    if (value === null || value <= 0) return;
    const item = selectedItems.find(it => it.key === productId);
    if (!item) return;

    const requiredBaseQty = value * item.conversionRate;
    if (requiredBaseQty > item.product.stockQuantity) {
      message.error(`Sản phẩm này chỉ còn ${item.product.stockQuantity} ${item.product.baseUnit?.name || 'Lon'} trong kho (Yêu cầu: ${requiredBaseQty}).`);
      return;
    }

    setSelectedItems(selectedItems.map(it => 
      it.key === productId ? { ...it, quantity: value } : it
    ));
  };

  const handleDiscountChange = (productId: string, value: number | null) => {
    if (value === null || value < 0 || value > 100) return;
    setSelectedItems(selectedItems.map(item => 
      item.key === productId ? { ...item, discountPercentage: value } : item
    ));
  };

  const handleFinish = async (values: any) => {
    if (selectedItems.length === 0) {
      message.error('Vui lòng chọn ít nhất một sản phẩm vào đơn hàng.');
      return;
    }

    const orderDetails: CreateOrderDetailDto[] = selectedItems.map(item => ({
      productId: item.product.id,
      unitId: item.selectedUnitId,
      quantity: item.quantity,
      discountPercentage: item.discountPercentage
    }));

    const orderDto: CreateOrderDto = {
      customerId: values.customerId,
      notes: values.notes,
      orderDetails
    };

    try {
      await createOrder(orderDto).unwrap();
      message.success('Tạo đơn hàng thành công.');
      navigate('/orders');
    } catch (error) {
      // Axios interceptor handles error message
    }
  };

  const columns = [
    {
      title: 'Sản phẩm',
      dataIndex: ['product', 'name'],
      key: 'productName',
      render: (_: any, record: SelectedItem) => (
        <div>
          <strong>{record.product.name}</strong>
          <div style={{ fontSize: '12px', color: '#8c8c8c' }}>Mã: {record.product.code}</div>
        </div>
      ),
    },
    {
      title: 'Đơn giá',
      key: 'price',
      render: (_: any, record: SelectedItem) => `${record.unitPrice.toLocaleString('vi-VN')} ₫`,
    },
    {
      title: 'Đơn vị tính',
      key: 'unit',
      render: (_: any, record: SelectedItem) => {
        const options = [
          { value: record.product.baseUnitId, label: record.product.baseUnit?.name || 'Lon' },
          ...(record.product.conversions || []).map(c => ({
            value: c.alternativeUnitId,
            label: c.alternativeUnitName || 'Quy đổi'
          }))
        ];
        return (
          <Select
            value={record.selectedUnitId}
            style={{ width: 110 }}
            onChange={(val) => handleUnitChange(record.product.id, val)}
            options={options}
          />
        );
      }
    },
    {
      title: 'Số lượng',
      key: 'quantity',
      render: (_: any, record: SelectedItem) => (
        <InputNumber
          min={1}
          max={Math.floor(record.product.stockQuantity / record.conversionRate)}
          value={record.quantity}
          onChange={(val) => handleQuantityChange(record.product.id, val)}
        />
      ),
    },
    {
      title: 'Chiết khấu (%)',
      key: 'discount',
      render: (_: any, record: SelectedItem) => (
        <InputNumber
          min={0}
          max={100}
          value={record.discountPercentage}
          onChange={(val) => handleDiscountChange(record.product.id, val)}
        />
      ),
    },
    {
      title: 'Thành tiền',
      key: 'total',
      render: (_: any, record: SelectedItem) => {
        const itemSubtotal = record.quantity * record.unitPrice;
        const discount = itemSubtotal * (record.discountPercentage / 100);
        return <strong>{(itemSubtotal - discount).toLocaleString('vi-VN')} ₫</strong>;
      },
    },
    {
      title: '',
      key: 'action',
      render: (_: any, record: SelectedItem) => (
        <Button
          type="text"
          danger
          icon={<DeleteOutlined />}
          onClick={() => handleRemoveItem(record.product.id)}
        />
      ),
    },
  ];

  return (
    <div>
      <h2 style={{ marginBottom: '24px' }}>Tạo đơn hàng mới (POS)</h2>

      <Form form={form} layout="vertical" onFinish={handleFinish}>
        <Row gutter={24}>
          <Col xs={24} lg={16}>
            <Card title="Chọn sản phẩm" bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)', marginBottom: '24px' }}>
              <Input.Search
                placeholder="Quét mã vạch sản phẩm hoặc đơn vị quy đổi..."
                enterButton="Thêm"
                onSearch={handleBarcodeScan}
                style={{ width: '100%', marginBottom: '16px' }}
              />

              <PaginatedSelect
                placeholder="Tìm sản phẩm theo tên hoặc mã..."
                style={{ width: '100%', marginBottom: '24px' }}
                onChange={handleAddProduct}
                value={null}
                fetchData={async (page, keyword) => {
                  const res = await apiClient.get('/product', { params: { pageNumber: page, pageSize: 20, keyword } });
                  const newProducts = res.data.data.data;
                  setLocalProducts(prev => {
                    const existingIds = new Set(prev.map(p => p.id));
                    const filtered = newProducts.filter((p: any) => !existingIds.has(p.id));
                    return [...prev, ...filtered];
                  });
                  const mappedData = newProducts.map((p: any) => ({
                    ...p,
                    displayName: `${p.name} - Mã: ${p.code} (Còn lại: ${p.stockQuantity} ${p.baseUnit?.name || 'Cái'}) - Giá: ${p.price.toLocaleString('vi-VN')} ₫`,
                    disabled: p.stockQuantity <= 0
                  }));
                  return { data: mappedData, totalPages: res.data.data.totalPages };
                }}
                valueProp="id"
                labelProp="displayName"
              />

              <Table
                dataSource={selectedItems}
                columns={columns}
                pagination={false}
                locale={{ emptyText: 'Chưa có sản phẩm nào được chọn. Vui lòng chọn sản phẩm ở trên.' }}
              />
            </Card>
          </Col>

          <Col xs={24} lg={8}>
            <Card title="Thông tin thanh toán" bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
              {/* Hardcode a mock Customer for scaffolding purposes, or select customer */}
              <Form.Item
                name="customerId"
                label="Khách hàng"
                initialValue="00000000-0000-0000-0000-000000000000"
                rules={[{ required: true, message: 'Vui lòng chọn khách hàng' }]}
              >
                <Select>
                  <Select.Option value="00000000-0000-0000-0000-000000000000">Khách vãng lai (Mặc định)</Select.Option>
                </Select>
              </Form.Item>

              <Form.Item name="notes" label="Ghi chú đơn hàng">
                <Input.TextArea rows={3} placeholder="Ghi chú thêm về đơn hàng..." />
              </Form.Item>

              <Divider />

              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '8px' }}>
                <span>Tạm tính:</span>
                <span>{subTotal.toLocaleString('vi-VN')} ₫</span>
              </div>
              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '8px' }}>
                <span>Thuế VAT (10%):</span>
                <span>{taxAmount.toLocaleString('vi-VN')} ₫</span>
              </div>
              <Divider style={{ margin: '12px 0' }} />
              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '24px', alignItems: 'center' }}>
                <span style={{ fontWeight: 'bold', fontSize: '16px' }}>Tổng tiền:</span>
                <span style={{ fontWeight: 'bold', fontSize: '20px', color: '#cf1322' }}>
                  {totalAmount.toLocaleString('vi-VN')} ₫
                </span>
              </div>

              <Button
                type="primary"
                htmlType="submit"
                size="large"
                block
                icon={<ShoppingCartOutlined />}
                disabled={selectedItems.length === 0}
              >
                Thanh toán & Lập đơn
              </Button>
            </Card>
          </Col>
        </Row>
      </Form>
    </div>
  );
};
export default CreateOrder;
