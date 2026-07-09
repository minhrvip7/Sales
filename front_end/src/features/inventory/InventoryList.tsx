import React, { useState, useEffect } from 'react';
import { Table, Input, Card, Button, Typography, Space, Tag } from 'antd';
import { SearchOutlined, HistoryOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { inventoryService } from '../../services/inventoryService';
import type { InventoryBalance } from '../../types';

const { Title } = Typography;

const InventoryList: React.FC = () => {
  const [data, setData] = useState<InventoryBalance[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [searchText, setSearchText] = useState<string>('');
  const navigate = useNavigate();

  const fetchData = async () => {
    setLoading(true);
    try {
      const balances = await inventoryService.getBalances();
      setData(balances);
    } catch (error) {
      console.error('Failed to fetch inventory balances', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const filteredData = data.filter((item) =>
    item.productName.toLowerCase().includes(searchText.toLowerCase()) ||
    item.productCode.toLowerCase().includes(searchText.toLowerCase())
  );

  const columns = [
    {
      title: 'Mã SP',
      dataIndex: 'productCode',
      key: 'productCode',
    },
    {
      title: 'Tên SP',
      dataIndex: 'productName',
      key: 'productName',
    },
    {
      title: 'On-hand',
      dataIndex: 'onHandQty',
      key: 'onHandQty',
      render: (text: number) => <strong>{text}</strong>,
    },
    {
      title: 'Allocated',
      dataIndex: 'allocatedQty',
      key: 'allocatedQty',
      render: (text: number) => <Tag color="orange">{text}</Tag>,
    },
    {
      title: 'Available',
      dataIndex: 'availableQty',
      key: 'availableQty',
      render: (text: number) => <Tag color={text < 0 ? 'red' : 'green'}>{text}</Tag>,
    },
    {
      title: 'Hành động',
      key: 'action',
      render: (_: any, record: InventoryBalance) => (
        <Button
          type="link"
          icon={<HistoryOutlined />}
          onClick={() => navigate(`/inventory/transactions/${record.productId}`)}
        >
          Thẻ kho
        </Button>
      ),
    },
  ];

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Tổng quan tồn kho</Title>
      </div>

      <Space style={{ marginBottom: 16 }}>
        <Input
          placeholder="Tìm theo mã hoặc tên SP"
          prefix={<SearchOutlined />}
          value={searchText}
          onChange={(e) => setSearchText(e.target.value)}
          style={{ width: 300 }}
          allowClear
        />
        <Button onClick={fetchData}>Làm mới</Button>
      </Space>

      <Table
        columns={columns}
        dataSource={filteredData}
        rowKey="id"
        loading={loading}
        pagination={{ defaultPageSize: 10 }}
      />
    </Card>
  );
};

export default InventoryList;
