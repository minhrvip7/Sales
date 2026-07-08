import React from 'react';
import { Table, Button, Space, Card, Tag, Modal, message } from 'antd';
import { StopOutlined, EyeOutlined } from '@ant-design/icons';
import { useGetOrdersQuery, useCancelOrderMutation } from '../../services/api/orderApi';
import type { Order } from '../../types';

export const OrderList: React.FC = () => {
  const { data: response, isLoading } = useGetOrdersQuery();
  const [cancelOrder] = useCancelOrderMutation();

  const orders = response?.data || [];

  const handleCancelOrder = (id: string) => {
    Modal.confirm({
      title: 'Xác nhận hủy đơn',
      content: 'Bạn có chắc chắn muốn hủy đơn hàng này không? Số lượng tồn kho sản phẩm sẽ được hoàn lại.',
      okText: 'Hủy đơn',
      okType: 'danger',
      cancelText: 'Hủy',
      onOk: async () => {
        try {
          await cancelOrder(id).unwrap();
          message.success('Hủy đơn hàng thành công.');
        } catch (error) {}
      },
    });
  };

  const showDetails = (order: Order) => {
    Modal.info({
      title: `Chi tiết đơn hàng: ${order.orderNumber}`,
      width: 700,
      content: (
        <div style={{ marginTop: '16px' }}>
          <p><strong>Khách hàng:</strong> {order.customer?.name || 'Khách vãng lai'}</p>
          <p><strong>Ngày đặt:</strong> {new Date(order.orderDate).toLocaleString('vi-VN')}</p>
          <p><strong>Ghi chú:</strong> {order.notes || 'Không có'}</p>
          <h4 style={{ margin: '16px 0 8px' }}>Sản phẩm đã mua:</h4>
          <Table
            dataSource={order.orderDetails}
            pagination={false}
            rowKey="productId"
            size="small"
            columns={[
              {
                title: 'Sản phẩm',
                dataIndex: ['product', 'name'],
                key: 'productName',
                render: (_: any, record: any) => record.product?.name || 'Sản phẩm ẩn',
              },
              {
                title: 'Đơn giá',
                dataIndex: 'unitPrice',
                key: 'unitPrice',
                render: (val: number) => `${val.toLocaleString('vi-VN')} ₫`,
              },
              {
                title: 'Số lượng',
                dataIndex: 'quantity',
                key: 'quantity',
              },
              {
                title: 'Chiết khấu',
                dataIndex: 'discountPercentage',
                key: 'discountPercentage',
                render: (val: number) => `${val} %`,
              },
              {
                title: 'Thành tiền',
                dataIndex: 'totalAmount',
                key: 'totalAmount',
                render: (val: number) => <strong>{val.toLocaleString('vi-VN')} ₫</strong>,
              },
            ]}
          />
          <div style={{ textAlign: 'right', marginTop: '16px', fontSize: '16px' }}>
            <div>Tạm tính: {order.subTotal.toLocaleString('vi-VN')} ₫</div>
            <div>VAT (10%): {order.taxAmount.toLocaleString('vi-VN')} ₫</div>
            <div style={{ fontWeight: 'bold', color: '#cf1322', fontSize: '18px', marginTop: '8px' }}>
              Tổng cộng: {order.totalAmount.toLocaleString('vi-VN')} ₫
            </div>
          </div>
        </div>
      ),
      okText: 'Đóng',
    });
  };

  const columns = [
    {
      title: 'Mã đơn hàng',
      dataIndex: 'orderNumber',
      key: 'orderNumber',
      render: (text: string) => <strong>{text}</strong>,
    },
    {
      title: 'Khách hàng',
      dataIndex: ['customer', 'name'],
      key: 'customerName',
      render: (_: any, record: Order) => record.customer?.name || 'Khách vãng lai',
    },
    {
      title: 'Ngày bán',
      dataIndex: 'orderDate',
      key: 'orderDate',
      render: (date: string) => new Date(date).toLocaleDateString('vi-VN'),
    },
    {
      title: 'Tổng tiền',
      dataIndex: 'totalAmount',
      key: 'totalAmount',
      render: (val: number) => `${val.toLocaleString('vi-VN')} ₫`,
    },
    {
      title: 'Trạng thái',
      dataIndex: 'orderStatus',
      key: 'orderStatus',
      render: (status: number) => {
        if (status === 1) return <Tag color="blue">Đã xác nhận</Tag>;
        if (status === 2) return <Tag color="green">Đã hoàn thành</Tag>;
        if (status === 3) return <Tag color="red">Đã hủy</Tag>;
        return <Tag color="gold">Nháp</Tag>;
      },
    },
    {
      title: 'Thao tác',
      key: 'actions',
      render: (_: any, record: Order) => (
        <Space size="middle">
          <Button type="text" icon={<EyeOutlined />} onClick={() => showDetails(record)} />
          {record.orderStatus !== 3 && (
            <Button
              type="text"
              danger
              icon={<StopOutlined />}
              onClick={() => handleCancelOrder(record.id)}
            />
          )}
        </Space>
      ),
    },
  ];

  return (
    <div>
      <h2 style={{ marginBottom: '24px' }}>Quản lý đơn hàng bán lẻ</h2>

      <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
        <Table dataSource={orders} columns={columns} rowKey="id" loading={isLoading} />
      </Card>
    </div>
  );
};
export default OrderList;
