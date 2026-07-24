import React, { useState } from 'react';
import { Table, Button, Space, Card, Tag, Modal, Form, Input, message, Drawer, Descriptions } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, EyeOutlined } from '@ant-design/icons';
import {
  useGetUnitsQuery,
  useCreateUnitMutation,
  useUpdateUnitMutation,
  useDeleteUnitMutation,
} from '../../services/api/unitApi';
import type { Unit, CreateUnitDto } from '../../types';

export const UnitList: React.FC = () => {
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(20);
  const [keyword] = useState('');

  const { data: response, isLoading } = useGetUnitsQuery({ pageNumber, pageSize, keyword });
  const [createUnit] = useCreateUnitMutation();
  const [updateUnit] = useUpdateUnitMutation();
  const [deleteUnit] = useDeleteUnitMutation();

  const units = response?.data?.data || [];

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingUnit, setEditingUnit] = useState<Unit | null>(null);
  const [viewingUnit, setViewingUnit] = useState<Unit | null>(null);
  const [form] = Form.useForm();

  const showModal = (unit?: Unit) => {
    if (unit) {
      setEditingUnit(unit);
      form.setFieldsValue({
        name: unit.name,
        code: unit.code,
        description: unit.description,
      });
    } else {
      setEditingUnit(null);
      form.resetFields();
    }
    setIsModalOpen(true);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const handleFinish = async (values: any) => {
    try {
      if (editingUnit) {
        await updateUnit({
          id: editingUnit.id,
          body: values as CreateUnitDto,
        }).unwrap();
        message.success('Cập nhật đơn vị tính thành công.');
      } else {
        await createUnit(values as CreateUnitDto).unwrap();
        message.success('Thêm đơn vị tính thành công.');
      }
      setIsModalOpen(false);
    } catch (error: any) {
      message.error(error?.data?.message || 'Có lỗi xảy ra vui lòng thử lại!');
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Xác nhận xóa',
      content: 'Bạn có chắc chắn muốn xóa đơn vị tính này không? Hành động này không thể hoàn tác.',
      okText: 'Xóa',
      okType: 'danger',
      cancelText: 'Hủy',
      onOk: async () => {
        try {
          await deleteUnit(id).unwrap();
          message.success('Xóa đơn vị tính thành công.');
        } catch (error: any) {
          message.error(error?.data?.message || 'Không thể xóa đơn vị tính này!');
        }
      },
    });
  };

  const columns = [
    {
      title: 'Mã ĐVT',
      dataIndex: 'code',
      key: 'code',
      render: (text: string) => <strong>{text}</strong>,
    },
    {
      title: 'Tên đơn vị tính',
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
      render: (_: any, record: Unit) => (
        <Space size="middle">
          <Button type="text" icon={<EyeOutlined />} onClick={() => setViewingUnit(record)} />
          <Button type="text" icon={<EditOutlined />} onClick={() => showModal(record)} />
          <Button type="text" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record.id)} />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '24px', alignItems: 'center' }}>
        <h2>Quản lý Đơn vị tính (ĐVT)</h2>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()}>
          Thêm đơn vị tính
        </Button>
      </div>

      <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
        <Table 
          dataSource={units} 
          columns={columns} 
          rowKey="id" 
          loading={isLoading} 
          pagination={{
            current: response?.data?.pageNumber || pageNumber,
            pageSize: response?.data?.pageSize || pageSize,
            total: response?.data?.totalRecords || 0,
            showSizeChanger: true,
            showTotal: (total) => `Tổng cộng: ${total} bản ghi`,
            onChange: (page, size) => {
              setPageNumber(page);
              setPageSize(size);
            }
          }}
        />
      </Card>

      <Modal
        title={editingUnit ? 'Sửa thông tin đơn vị tính' : 'Thêm đơn vị tính mới'}
        open={isModalOpen}
        onCancel={handleCancel}
        footer={null}
        destroyOnClose
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item
            name="code"
            label="Mã ĐVT"
            rules={[{ required: true, message: 'Vui lòng nhập mã đơn vị tính' }]}
          >
            <Input disabled={!!editingUnit} />
          </Form.Item>

          <Form.Item
            name="name"
            label="Tên ĐVT"
            rules={[{ required: true, message: 'Vui lòng nhập tên đơn vị tính' }]}
          >
            <Input />
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

      <Drawer
        title="Chi tiết Đơn vị tính"
        width={500}
        onClose={() => setViewingUnit(null)}
        open={!!viewingUnit}
      >
        {viewingUnit && (
          <Descriptions bordered column={1}>
            <Descriptions.Item label="Mã ĐVT"><strong>{viewingUnit.code}</strong></Descriptions.Item>
            <Descriptions.Item label="Tên ĐVT"><strong>{viewingUnit.name}</strong></Descriptions.Item>
            <Descriptions.Item label="Trạng thái">
              <Tag color={viewingUnit.status ? 'green' : 'red'}>
                {viewingUnit.status ? 'Hoạt động' : 'Khóa'}
              </Tag>
            </Descriptions.Item>
            <Descriptions.Item label="Mô tả">
              {viewingUnit.description || '-'}
            </Descriptions.Item>
          </Descriptions>
        )}
      </Drawer>
    </div>
  );
};
export default UnitList;
