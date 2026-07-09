import React, { useState } from 'react';
import { Table, Button, Space, Card, Tag, Modal, Form, Input, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import {
  useGetCategoriesQuery,
  useCreateCategoryMutation,
  useUpdateCategoryMutation,
  useDeleteCategoryMutation,
} from '../../services/api/categoryApi';
import type { Category, CreateCategoryDto } from '../../types';

export const CategoryList: React.FC = () => {
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(20);
  const [keyword] = useState('');

  const { data: response, isLoading } = useGetCategoriesQuery({ pageNumber, pageSize, keyword });
  const [createCategory] = useCreateCategoryMutation();
  const [updateCategory] = useUpdateCategoryMutation();
  const [deleteCategory] = useDeleteCategoryMutation();

  const categories = response?.data?.data || [];

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);
  const [form] = Form.useForm();

  const showModal = (category?: Category) => {
    if (category) {
      setEditingCategory(category);
      form.setFieldsValue({
        name: category.name,
        code: category.code,
        description: category.description,
      });
    } else {
      setEditingCategory(null);
      form.resetFields();
    }
    setIsModalOpen(true);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const handleFinish = async (values: any) => {
    try {
      if (editingCategory) {
        await updateCategory({
          id: editingCategory.id,
          body: values as CreateCategoryDto,
        }).unwrap();
        message.success('Cập nhật nhóm sản phẩm thành công.');
      } else {
        await createCategory(values as CreateCategoryDto).unwrap();
        message.success('Thêm nhóm sản phẩm thành công.');
      }
      setIsModalOpen(false);
    } catch (error) {}
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Xác nhận xóa',
      content: 'Bạn có chắc chắn muốn xóa nhóm sản phẩm này không? Hành động này không thể hoàn tác.',
      okText: 'Xóa',
      okType: 'danger',
      cancelText: 'Hủy',
      onOk: async () => {
        try {
          await deleteCategory(id).unwrap();
          message.success('Xóa nhóm sản phẩm thành công.');
        } catch (error) {}
      },
    });
  };

  const columns = [
    {
      title: 'Mã nhóm',
      dataIndex: 'code',
      key: 'code',
      render: (text: string) => <strong>{text}</strong>,
    },
    {
      title: 'Tên nhóm sản phẩm',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Mô tả',
      dataIndex: 'description',
      key: 'description',
      render: (text?: string) => text || '-',
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      render: (status: boolean) => (
        <Tag color={status ? 'green' : 'red'}>{status ? 'Hoạt động' : 'Khóa'}</Tag>
      ),
    },
    {
      title: 'Thao tác',
      key: 'actions',
      render: (_: any, record: Category) => (
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
        <h2>Quản lý Nhóm sản phẩm</h2>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()}>
          Thêm nhóm sản phẩm
        </Button>
      </div>

      <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
        <Table 
          dataSource={categories} 
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
        title={editingCategory ? 'Sửa thông tin nhóm sản phẩm' : 'Thêm nhóm sản phẩm mới'}
        open={isModalOpen}
        onCancel={handleCancel}
        footer={null}
        destroyOnClose
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item
            name="name"
            label="Tên nhóm"
            rules={[{ required: true, message: 'Vui lòng nhập tên nhóm sản phẩm' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            name="code"
            label="Mã nhóm"
            rules={[{ required: true, message: 'Vui lòng nhập mã nhóm sản phẩm' }]}
          >
            <Input disabled={!!editingCategory} />
          </Form.Item>

          <Form.Item name="description" label="Mô tả">
            <Input.TextArea rows={3} />
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
export default CategoryList;
