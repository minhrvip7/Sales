import React, { useState } from 'react';
import { Layout } from 'antd';
import { Sidebar } from './Sidebar';
import { Header } from './Header';
import { Outlet } from 'react-router-dom';

const { Content } = Layout;

export const MainLayout: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sidebar collapsed={collapsed} />
      <Layout>
        <Header collapsed={collapsed} setCollapsed={setCollapsed} />
        <Content style={{
          margin: '24px 16px',
          padding: 24,
          background: '#fff',
          minHeight: 280,
          borderRadius: '8px',
          overflow: 'initial',
        }}>
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
};
export default MainLayout;
