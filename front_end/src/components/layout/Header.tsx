import React from 'react';
import { Layout, Button, Space, Avatar, Dropdown } from 'antd';
import type { MenuProps } from 'antd';
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  UserOutlined,
  LogoutOutlined,
} from '@ant-design/icons';

const { Header: AntHeader } = Layout;

interface HeaderProps {
  collapsed: boolean;
  setCollapsed: (collapsed: boolean) => void;
}

export const Header: React.FC<HeaderProps> = ({ collapsed, setCollapsed }) => {
  const userItems: MenuProps['items'] = [
    {
      key: 'profile',
      icon: <UserOutlined />,
      label: 'Thông tin cá nhân',
    },
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: 'Đăng xuất',
      danger: true,
      onClick: () => {
        localStorage.removeItem('accessToken');
        window.location.reload();
      }
    },
  ];

  return (
    <AntHeader style={{
      padding: 0,
      background: '#fff',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'space-between',
      boxShadow: '0 1px 4px rgba(0, 21, 41, 0.08)',
      zIndex: 1,
    }}>
      <Button
        type="text"
        icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
        onClick={() => setCollapsed(!collapsed)}
        style={{
          fontSize: '16px',
          width: 64,
          height: 64,
        }}
      />
      <div style={{ paddingRight: '24px' }}>
        <Space size="large">
          <span style={{ fontWeight: 500 }}>Xin chào, Quản trị viên</span>
          <Dropdown menu={{ items: userItems }} placement="bottomRight">
            <Avatar style={{ backgroundColor: '#1890ff', cursor: 'pointer' }} icon={<UserOutlined />} />
          </Dropdown>
        </Space>
      </div>
    </AntHeader>
  );
};
