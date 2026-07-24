import React from 'react';
import { Layout, Menu } from 'antd';
import {
  DashboardOutlined,
  ShoppingOutlined,
  ShoppingCartOutlined,
  PlusCircleOutlined,
  AppstoreOutlined,
  BlockOutlined,
  DatabaseOutlined,
  ImportOutlined,
} from '@ant-design/icons';
import { useNavigate, useLocation } from 'react-router-dom';

const { Sider } = Layout;

interface SidebarProps {
  collapsed: boolean;
}

export const Sidebar: React.FC<SidebarProps> = ({ collapsed }) => {
  const navigate = useNavigate();
  const location = useLocation();

  const menuItems = [
    {
      key: '/',
      icon: <DashboardOutlined />,
      label: 'Bảng điều khiển',
    },
    {
      key: '/products',
      icon: <ShoppingOutlined />,
      label: 'Sản phẩm',
    },
    {
      key: '/categories',
      icon: <AppstoreOutlined />,
      label: 'Nhóm sản phẩm',
    },
    {
      key: '/units',
      icon: <BlockOutlined />,
      label: 'Đơn vị tính',
    },
    {
      key: '/inventory',
      icon: <DatabaseOutlined />,
      label: 'Tồn kho',
    },
    {
      key: '/inventory/goods-receipt',
      icon: <ImportOutlined />,
      label: 'Nhập kho',
    },
    {
      key: '/orders',
      icon: <ShoppingCartOutlined />,
      label: 'Đơn hàng',
    },
    {
      key: '/orders/new',
      icon: <PlusCircleOutlined />,
      label: 'Lập đơn hàng',
    },
  ];

  return (
    <Sider trigger={null} collapsible collapsed={collapsed} width={240} theme="dark">
      <div style={{
        height: '64px',
        margin: '16px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'rgba(255, 255, 255, 0.1)',
        borderRadius: '8px',
        color: '#fff',
        fontWeight: 'bold',
        fontSize: collapsed ? '12px' : '18px',
        transition: 'all 0.2s',
      }}>
        {collapsed ? 'SALES' : 'SALES MANAGER'}
      </div>
      <Menu
        theme="dark"
        mode="inline"
        selectedKeys={[location.pathname]}
        onClick={({ key }) => navigate(key)}
        items={menuItems}
      />
    </Sider>
  );
};
