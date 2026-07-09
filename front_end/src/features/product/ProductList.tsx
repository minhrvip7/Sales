import React, { useState } from 'react';
import { Table, Button, Space, Card, Tag, Modal, Form, Input, InputNumber, message, Row, Col, Divider } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import {
  useGetProductsQuery,
  useCreateProductMutation,
  useUpdateProductMutation,
  useDeleteProductMutation,
} from '../../services/api/productApi';
import type { CreateProductDto, Product } from '../../types';
import { PaginatedSelect } from '../../components/PaginatedSelect';
import apiClient from '../../services/apiClient';

export const ProductList: React.FC = () => {
  const [createProduct] = useCreateProductMutation();
  const [updateProduct] = useUpdateProductMutation();
  const [deleteProduct] = useDeleteProductMutation();

  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(20);
  const [keyword] = useState('');

  const { data: response, isLoading } = useGetProductsQuery({ pageNumber, pageSize, keyword });

  const products = response?.data?.data || [];

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [form] = Form.useForm();

  const showModal = (product?: Product) => {
    if (product) {
      setEditingProduct(product);
      form.setFieldsValue({
        name: product.name,
        code: product.code,
        barcode: product.barcode,
        price: product.price,
        cost: product.cost,
        description: product.description,
        categoryId: product.categoryId,
        baseUnitId: product.baseUnitId,
        conversions: product.conversions || [],
      });
    } else {
      setEditingProduct(null);
      form.resetFields();
    }
    setIsModalOpen(true);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const handleFinish = async (values: any) => {
    try {
      if (editingProduct) {
        await updateProduct({
          id: editingProduct.id,
          body: values as CreateProductDto,
        }).unwrap();
        message.success('Cập nhật sản phẩm thành công.');
      } else {
        // Loại bỏ id khỏi payload khi tạo mới, id chỉ dùng khi update
        const { id: _id, ...createPayload } = values;
        await createProduct(createPayload as CreateProductDto).unwrap();
        message.success('Thêm sản phẩm thành công.');
      }
      setIsModalOpen(false);
    } catch (error) {
      // Axios interceptor handles global message, but we can do extra logic here
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Xác nhận xóa',
      content: 'Bạn có chắc chắn muốn xóa sản phẩm này không?',
      okText: 'Xóa',
      okType: 'danger',
      cancelText: 'Hủy',
      onOk: async () => {
        try {
          await deleteProduct(id).unwrap();
          message.success('Xóa sản phẩm thành công.');
        } catch (error) {}
      },
    });
  };

  const columns = [
    {
      title: 'Mã SP',
      dataIndex: 'code',
      key: 'code',
      render: (text: string) => <strong>{text}</strong>,
    },
    {
      title: 'Tên sản phẩm',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Nhóm',
      key: 'categoryName',
      render: (_: any, record: Product) => record.category?.name || '-',
    },
    {
      title: 'ĐVT',
      key: 'unitName',
      render: (_: any, record: Product) => record.baseUnit?.name || '-',
    },
    {
      title: 'Giá bán',
      dataIndex: 'price',
      key: 'price',
      render: (val: number) => `${val.toLocaleString('vi-VN')} ₫`,
    },
    {
      title: 'Giá vốn',
      dataIndex: 'cost',
      key: 'cost',
      render: (val: number) => `${val.toLocaleString('vi-VN')} ₫`,
    },
    {
      title: 'Tồn kho',
      dataIndex: 'stockQuantity',
      key: 'stockQuantity',
      render: (val: number) => {
        if (val < 10) return <span style={{ color: 'red', fontWeight: 'bold' }}>{val} (Sắp hết)</span>;
        return val;
      },
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      render: (status: boolean) => (
        <Tag color={status ? 'green' : 'red'}>{status ? 'Đang bán' : 'Ngừng bán'}</Tag>
      ),
    },
    {
      title: 'Thao tác',
      key: 'actions',
      render: (_: any, record: Product) => (
        <Space size="middle">
          <Button type="text" icon={<EditOutlined />} onClick={() => showModal(record)} />
          <Button type="text" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record.id)} />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '24px', alignItems: 'center' }}>
        <h2>Danh mục sản phẩm</h2>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()}>
          Thêm sản phẩm
        </Button>
      </div>

      <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
        <Table 
          dataSource={products} 
          columns={columns} 
          rowKey="id" 
          loading={isLoading} 
          pagination={{
            current: response?.data?.pageNumber || pageNumber,
            pageSize: response?.data?.pageSize || pageSize,
            total: response?.data?.totalRecords || 0,
            showSizeChanger: true,
            onChange: (page, size) => {
              setPageNumber(page);
              setPageSize(size);
            }
          }}
        />
      </Card>

      <Modal
        title={editingProduct ? 'Sửa thông tin sản phẩm' : 'Thêm sản phẩm mới'}
        open={isModalOpen}
        onCancel={handleCancel}
        footer={null}
        destroyOnClose
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item
            name="name"
            label="Tên sản phẩm"
            rules={[{ required: true, message: 'Vui lòng nhập tên sản phẩm' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            name="code"
            label="Mã sản phẩm"
            rules={[{ required: true, message: 'Vui lòng nhập mã sản phẩm' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item name="barcode" label="Mã vạch (Barcode)">
            <Input />
          </Form.Item>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                name="price"
                label="Giá bán (₫)"
                rules={[{ required: true, message: 'Vui lòng nhập giá bán' }]}
              >
                <InputNumber style={{ width: '100%' }} min={0} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                name="cost"
                label="Giá vốn (₫)"
                rules={[{ required: true, message: 'Vui lòng nhập giá vốn' }]}
              >
                <InputNumber style={{ width: '100%' }} min={0} />
              </Form.Item>
            </Col>
          </Row>

          <Form.Item name="description" label="Mô tả">
            <Input.TextArea rows={3} />
          </Form.Item>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                name="categoryId"
                label="Nhóm sản phẩm"
                rules={[{ required: true, message: 'Vui lòng chọn nhóm sản phẩm' }]}
              >
                <PaginatedSelect 
                  placeholder="Chọn nhóm"
                  fetchData={async (page, keyword) => {
                    const res = await apiClient.get('/category', { params: { pageNumber: page, pageSize: 20, keyword } });
                    return { data: res.data.data.data, totalPages: res.data.data.totalPages };
                  }}
                  valueProp="id"
                  labelProp="name"
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                name="baseUnitId"
                label="Đơn vị tính"
                rules={[{ required: true, message: 'Vui lòng chọn đơn vị tính' }]}
                extra={editingProduct?.hasTransactions ? <span style={{ color: 'orange', fontSize: '12px' }}>Đã có giao dịch, không thể sửa đơn vị tính cơ bản.</span> : null}
              >
                <PaginatedSelect 
                  placeholder="Chọn đơn vị" 
                  disabled={editingProduct?.hasTransactions}
                  fetchData={async (page, keyword) => {
                    const res = await apiClient.get('/unit', { params: { pageNumber: page, pageSize: 20, keyword } });
                    return { data: res.data.data.data, totalPages: res.data.data.totalPages };
                  }}
                  valueProp="id"
                  labelProp="name"
                />
              </Form.Item>
            </Col>
          </Row>

          <Divider style={{ margin: '12px 0' }}>Đơn vị quy đổi</Divider>
          <Form.List name="conversions">
            {(fields, { add, remove }) => (
              <>
                {fields.map(({ key, name, ...restField }) => (
                  <Space key={key} style={{ display: 'flex', marginBottom: 8 }} align="baseline">
                    <Form.Item
                      {...restField}
                      name={[name, 'alternativeUnitId']}
                      rules={[{ required: true, message: 'Chọn đơn vị' }]}
                    >
                      <PaginatedSelect 
                        placeholder="Đơn vị" 
                        style={{ width: 130 }}
                        fetchData={async (page, keyword) => {
                          const res = await apiClient.get('/unit', { params: { pageNumber: page, pageSize: 20, keyword } });
                          return { data: res.data.data.data, totalPages: res.data.data.totalPages };
                        }}
                        valueProp="id"
                        labelProp="name"
                      />
                    </Form.Item>
                    <Form.Item
                      {...restField}
                      name={[name, 'conversionRate']}
                      rules={[{ required: true, message: 'Nhập hệ số' }]}
                    >
                      <InputNumber placeholder="Hệ số (>0)" min={0.0001} style={{ width: 100 }} />
                    </Form.Item>
                    <Form.Item
                      {...restField}
                      name={[name, 'barcode']}
                    >
                      <Input placeholder="Mã vạch riêng" style={{ width: 120 }} />
                    </Form.Item>
                    <Form.Item
                      {...restField}
                      name={[name, 'price']}
                    >
                      <InputNumber placeholder="Giá bán (₫)" min={0} style={{ width: 120 }} />
                    </Form.Item>
                    <Button type="text" danger onClick={() => remove(name)} icon={<DeleteOutlined />} />
                  </Space>
                ))}
                <Form.Item>
                  <Button type="dashed" onClick={() => add()} block icon={<PlusOutlined />}>
                    Thêm Đơn vị quy đổi
                  </Button>
                </Form.Item>
              </>
            )}
          </Form.List>

          <Form.Item style={{ textAlign: 'right', marginBottom: 0 }}>
            <Space>
              <Button onClick={handleCancel}>Hủy</Button>
              <Button type="primary" htmlType="submit">
                Lưu
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};
export default ProductList;
