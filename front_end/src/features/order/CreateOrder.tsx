import React, { useState } from 'react';
import { Row, Col, Card, Form, Select, Button, Table, InputNumber, Input, Divider, message } from 'antd';
import { DeleteOutlined, ShoppingCartOutlined } from '@ant-design/icons';
import { useGetProductsQuery } from '../../services/api/productApi';
import { useCreateOrderMutation } from '../../services/api/orderApi';
import type { CreateOrderDto, CreateOrderDetailDto, Product } from '../../types';
import { useNavigate } from 'react-router-dom';

interface SelectedItem {
  key: string; // Product ID
  product: Product;
  quantity: number;
  discountPercentage: number;
}

export const CreateOrder: React.FC = () => {
  const navigate = useNavigate();
  const { data: productsResponse } = useGetProductsQuery();
  const [createOrder] = useCreateOrderMutation();

  const products = productsResponse?.data || [];
  const [selectedItems, setSelectedItems] = useState<SelectedItem[]>([]);
  const [form] = Form.useForm();

  // Calculate totals
  const subTotal = selectedItems.reduce((sum, item) => {
    const itemSubtotal = item.quantity * item.product.price;
    const discount = itemSubtotal * (item.discountPercentage / 100);
    return sum + (itemSubtotal - discount);
  }, 0);

  const taxAmount = subTotal * 0.1; // 10% VAT
  const totalAmount = subTotal + taxAmount;

  const handleAddProduct = (productId: string) => {
    const product = products.find(p => p.id === productId);
    if (!product) return;

    // Check if stock is available
    if (product.stockQuantity <= 0) {
      message.error('Sản phẩm này đã hết hàng trong kho.');
      return;
    }

    // Check if already added
    const existing = selectedItems.find(item => item.key === productId);
    if (existing) {
      if (existing.quantity >= product.stockQuantity) {
        message.error(`Không thể thêm. Tồn kho tối đa của sản phẩm này là ${product.stockQuantity}.`);
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
        discountPercentage: 0
      }]);
    }
  };

  const handleRemoveItem = (productId: string) => {
    setSelectedItems(selectedItems.filter(item => item.key !== productId));
  };

  const handleQuantityChange = (productId: string, value: number | null) => {
    if (value === null || value <= 0) return;
    const product = products.find(p => p.id === productId);
    if (!product) return;

    if (value > product.stockQuantity) {
      message.error(`Sản phẩm này chỉ còn ${product.stockQuantity} trong kho.`);
      return;
    }

    setSelectedItems(selectedItems.map(item => 
      item.key === productId ? { ...item, quantity: value } : item
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
      dataIndex: ['product', 'price'],
      key: 'price',
      render: (_: any, record: SelectedItem) => `${record.product.price.toLocaleString('vi-VN')} ₫`,
    },
    {
      title: 'Số lượng',
      key: 'quantity',
      render: (_: any, record: SelectedItem) => (
        <InputNumber
          min={1}
          max={record.product.stockQuantity}
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
        const itemSubtotal = record.quantity * record.product.price;
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
              <Select
                showSearch
                placeholder="Tìm sản phẩm theo tên hoặc mã..."
                optionFilterProp="children"
                style={{ width: '100%', marginBottom: '24px' }}
                onChange={handleAddProduct}
                value={null} // Reset select value after choosing
              >
                {products.map(p => (
                  <Select.Option key={p.id} value={p.id} disabled={p.stockQuantity <= 0}>
                    {p.name} - Mã: {p.code} (Còn lại: {p.stockQuantity} {p.unit?.name || 'Cái'}) - Giá: {p.price.toLocaleString('vi-VN')} ₫
                  </Select.Option>
                ))}
              </Select>

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
