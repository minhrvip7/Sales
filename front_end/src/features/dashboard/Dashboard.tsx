import React from 'react';
import { Row, Col, Card, Statistic, Table, Tag } from 'antd';
import {
  DollarOutlined,
  ShoppingCartOutlined,
  WarningOutlined,
} from '@ant-design/icons';
import { useGetOrdersQuery } from '../../services/api/orderApi';
import { useGetProductsQuery } from '../../services/api/productApi';

export const Dashboard: React.FC = () => {
  const { data: ordersResponse } = useGetOrdersQuery();
  const { data: productsResponse } = useGetProductsQuery();

  const orders = ordersResponse?.data || [];
  const products = productsResponse?.data || [];

  // Calculate statistics
  const totalRevenue = orders.reduce((sum, order) => sum + (order.orderStatus !== 3 ? order.totalAmount : 0), 0);
  const totalOrders = orders.length;
  const lowStockCount = products.filter(p => p.stockQuantity < 10).length;

  const recentOrdersColumns = [
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
      render: (_: any, record: any) => record.customer?.name || 'Khách vãng lai',
    },
    {
      title: 'Ngày đặt',
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
  ];

  return (
    <div>
      <h2 style={{ marginBottom: '24px' }}>Bảng điều khiển tổng quan</h2>
      
      <Row gutter={[16, 16]} style={{ marginBottom: '24px' }}>
        <Col xs={24} sm={8}>
          <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
            <Statistic
              title="Tổng doanh thu"
              value={totalRevenue}
              precision={0}
              valueStyle={{ color: '#3f8600' }}
              prefix={<DollarOutlined />}
              suffix=" ₫"
            />
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
            <Statistic
              title="Tổng số đơn hàng"
              value={totalOrders}
              valueStyle={{ color: '#1890ff' }}
              prefix={<ShoppingCartOutlined />}
            />
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
            <Statistic
              title="Sản phẩm sắp hết hàng (<10)"
              value={lowStockCount}
              valueStyle={{ color: lowStockCount > 0 ? '#cf1322' : '#d9d9d9' }}
              prefix={<WarningOutlined />}
            />
          </Card>
        </Col>
      </Row>

      <Card title="Đơn hàng gần đây" bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.06)' }}>
        <Table
          dataSource={orders.slice(0, 5)}
          columns={recentOrdersColumns}
          rowKey="id"
          pagination={false}
          locale={{ emptyText: 'Chưa có đơn hàng nào được tạo.' }}
        />
      </Card>
    </div>
  );
};
export default Dashboard;
