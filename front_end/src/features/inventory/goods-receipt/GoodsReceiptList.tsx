import React, { useEffect, useState } from 'react';
import { Card, Table, Typography, Tag, Button, Space, Input, Select, message } from 'antd';
import { PlusOutlined, EditOutlined, EyeOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import { goodsReceiptApi } from '../../../services/GoodsReceiptApi';
import type { GoodsReceiptSummaryDto } from '../../../types/GoodsReceipt';
import { GoodsReceiptStatus, GoodsReceiptType } from '../../../types/GoodsReceipt';

const { Title } = Typography;
const { Option } = Select;

export const GoodsReceiptList: React.FC = () => {
    const navigate = useNavigate();
    const [data, setData] = useState<GoodsReceiptSummaryDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [total, setTotal] = useState(0);
    const [queryParams, setQueryParams] = useState({
        pageNumber: 1,
        pageSize: 10,
        keyword: '',
        status: undefined as number | undefined
    });

    const fetchData = async () => {
        setLoading(true);
        try {
            const res = await goodsReceiptApi.getPaged(queryParams);
            setData(res.data.data.data);
            setTotal(res.data.data.totalRecords);
        } catch (error) {
            message.error('Lỗi khi tải danh sách phiếu nhập kho');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, [queryParams.pageNumber, queryParams.pageSize, queryParams.status]);

    const handleSearch = (value: string) => {
        setQueryParams({ ...queryParams, keyword: value, pageNumber: 1 });
        fetchData();
    };

    const columns = [
        {
            title: 'Mã phiếu',
            dataIndex: 'code',
            key: 'code',
            render: (text: string, record: GoodsReceiptSummaryDto) => (
                <a onClick={() => navigate(`/inventory/goods-receipt/${record.id}`)}>{text}</a>
            )
        },
        {
            title: 'Loại',
            dataIndex: 'type',
            key: 'type',
            render: (type: GoodsReceiptType) => {
                switch (type) {
                    case GoodsReceiptType.Purchase: return 'Nhập mua hàng';
                    case GoodsReceiptType.Return: return 'Nhập hàng trả lại';
                    case GoodsReceiptType.Transfer: return 'Nhập chuyển kho';
                    default: return 'Khác';
                }
            }
        },
        {
            title: 'Mã tham chiếu',
            dataIndex: 'referenceCode',
            key: 'referenceCode',
        },
        {
            title: 'Ngày nhập',
            dataIndex: 'receiptDate',
            key: 'receiptDate',
            render: (date: string) => dayjs(date).format('DD/MM/YYYY HH:mm')
        },
        {
            title: 'Tổng SL',
            dataIndex: 'totalQuantity',
            key: 'totalQuantity',
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            key: 'status',
            render: (status: GoodsReceiptStatus) => {
                let color = 'default';
                let text = 'Nháp';
                if (status === GoodsReceiptStatus.Completed) {
                    color = 'blue';
                    text = 'Hoàn tất';
                } else if (status === GoodsReceiptStatus.Cancelled) {
                    color = 'red';
                    text = 'Đã hủy';
                }
                return <Tag color={color}>{text}</Tag>;
            }
        },
        {
            title: 'Thao tác',
            key: 'action',
            render: (_: any, record: GoodsReceiptSummaryDto) => (
                <Space size="middle">
                    {record.status === GoodsReceiptStatus.Draft ? (
                        <Button 
                            type="text" 
                            icon={<EditOutlined />} 
                            onClick={() => navigate(`/inventory/goods-receipt/${record.id}`)} 
                        />
                    ) : (
                        <Button 
                            type="text" 
                            icon={<EyeOutlined />} 
                            onClick={() => navigate(`/inventory/goods-receipt/${record.id}`)} 
                        />
                    )}
                </Space>
            ),
        },
    ];

    return (
        <Card>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
                <Title level={3} style={{ margin: 0 }}>Danh sách Phiếu nhập kho</Title>
                <Button 
                    type="primary" 
                    icon={<PlusOutlined />} 
                    onClick={() => navigate('/inventory/goods-receipt/new')}
                >
                    Tạo mới
                </Button>
            </div>

            <div style={{ marginBottom: 16, display: 'flex', gap: '16px' }}>
                <Input.Search
                    placeholder="Tìm kiếm theo mã phiếu, tham chiếu..."
                    onSearch={handleSearch}
                    style={{ width: 300 }}
                    allowClear
                />
                <Select
                    placeholder="Chọn trạng thái"
                    style={{ width: 200 }}
                    allowClear
                    onChange={(val) => setQueryParams({ ...queryParams, status: val, pageNumber: 1 })}
                >
                    <Option value={GoodsReceiptStatus.Draft}>Nháp</Option>
                    <Option value={GoodsReceiptStatus.Completed}>Hoàn tất</Option>
                    <Option value={GoodsReceiptStatus.Cancelled}>Đã hủy</Option>
                </Select>
            </div>

            <Table
                columns={columns}
                dataSource={data}
                rowKey="id"
                loading={loading}
                pagination={{
                    current: queryParams.pageNumber,
                    pageSize: queryParams.pageSize,
                    total: total,
                    showSizeChanger: true,
                    onChange: (page, pageSize) => {
                        setQueryParams({ ...queryParams, pageNumber: page, pageSize });
                    }
                }}
            />
        </Card>
    );
};

export default GoodsReceiptList;
