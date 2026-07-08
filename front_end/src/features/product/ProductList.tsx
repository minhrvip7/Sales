import React, { useState } from 'react';
import { Table, Button, Space, Card, Tag, Modal, Form, Input, InputNumber, message, Row, Col } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import {
  useGetProductsQuery,
  useCreateProductMutation,
  useUpdateProductMutation,
  useDeleteProductMutation,
} from '../../services/api/productApi';
import type { CreateProductDto, Product } from '../../types';

export const ProductList: React.FC = () => {
  const { data: response, isLoading } = useGetProductsQuery();
  const [createProduct] = useCreateProductMutation();
  const [updateProduct] = useUpdateProductMutation();
  const [deleteProduct] = useDeleteProductMutation();

  const products = response?.data || [];

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
        // Mock default category/unit ids if none exist
        categoryId: product.categoryId,
        unitId: product.unitId,
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
        await createProduct(values as CreateProductDto).unwrap();
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
        <Table dataSource={products} columns={columns} rowKey="id" loading={isLoading} />
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

          {/* Hardcode a mock Category and Unit for scaffolding purposes */}
          <Form.Item
            name="categoryId"
            label="Nhóm sản phẩm"
            initialValue="00000000-0000-0000-0000-000000000000"
            hidden
          >
            <Input />
          </Form.Item>
          
          <Form.Item
            name="unitId"
            label="Đơn vị tính"
            initialValue="00000000-0000-0000-0000-000000000000"
            hidden
          >
            <Input />
          </Form.Item>

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
