import React, { useState, useEffect } from 'react';
import { Table, Card, Button, Typography, Tag } from 'antd';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { useParams, useNavigate } from 'react-router-dom';
import { inventoryService } from '../../services/inventoryService';
import type { InventoryTransaction } from '../../types';
import dayjs from 'dayjs';

const { Title } = Typography;

const TransactionTypeConfig: Record<number, { text: string; color: string }> = {
  1: { text: 'Nhập (Inbound)', color: 'blue' },
  2: { text: 'Xuất bán (Outbound)', color: 'green' },
  3: { text: 'Kiểm kê tăng', color: 'purple' },
  4: { text: 'Kiểm kê giảm', color: 'red' },
  5: { text: 'Xuất khác', color: 'orange' },
};

const InventoryTransactionHistory: React.FC = () => {
  const { productId } = useParams<{ productId: string }>();
  const navigate = useNavigate();
  const [data, setData] = useState<InventoryTransaction[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [productName, setProductName] = useState<string>('');

  useEffect(() => {
    if (productId) {
      fetchTransactions(productId);
    }
  }, [productId]);

  const fetchTransactions = async (id: string) => {
    setLoading(true);
    try {
      const transactions = await inventoryService.getTransactions(id);
      setData(transactions);
      if (transactions.length > 0) {
        setProductName(transactions[0].productName);
      }
    } catch (error) {
      console.error('Failed to fetch transactions', error);
    } finally {
      setLoading(false);
    }
  };

  const columns = [
    {
      title: 'Ngày giao dịch',
      dataIndex: 'createdDate',
      key: 'createdDate',
      render: (text: string) => dayjs(text).format('DD/MM/YYYY HH:mm'),
    },
    {
      title: 'Loại',
      dataIndex: 'type',
      key: 'type',
      render: (type: number) => {
        const config = TransactionTypeConfig[type] || { text: 'Không xác định', color: 'default' };
        return <Tag color={config.color}>{config.text}</Tag>;
      },
    },
    {
      title: 'Mã tham chiếu',
      dataIndex: 'referenceNumber',
      key: 'referenceNumber',
    },
    {
      title: 'SL Giao dịch',
      key: 'transacted',
      render: (_: any, record: InventoryTransaction) => (
        `${record.transactedQty} ${record.transactedUomName}`
      ),
    },
    {
      title: 'SL Quy đổi (Base Qty)',
      dataIndex: 'baseQty',
      key: 'baseQty',
      render: (text: number) => (
        <span style={{ color: text > 0 ? 'green' : (text < 0 ? 'red' : 'inherit'), fontWeight: 'bold' }}>
          {text > 0 ? `+${text}` : text}
        </span>
      ),
    },
    {
      title: 'Lý do',
      dataIndex: 'reason',
      key: 'reason',
    },
  ];

  return (
    <Card>
      <div style={{ display: 'flex', alignItems: 'center', marginBottom: 16 }}>
        <Button 
          type="text" 
          icon={<ArrowLeftOutlined />} 
          onClick={() => navigate('/inventory')}
          style={{ marginRight: 8 }}
        />
        <Title level={4} style={{ margin: 0 }}>
          Thẻ kho {productName ? `- ${productName}` : ''}
        </Title>
      </div>

      <Table
        columns={columns}
        dataSource={data}
        rowKey="id"
        loading={loading}
        pagination={{ defaultPageSize: 10 }}
      />
    </Card>
  );
};

export default InventoryTransactionHistory;
