import React, { useEffect, useState } from 'react';
import { Card, Form, Row, Col, Select, Input, DatePicker, Button, Table, message, Popconfirm, Typography, Space } from 'antd';
import { SaveOutlined, CheckCircleOutlined, DeleteOutlined, ArrowLeftOutlined, ScanOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import dayjs from 'dayjs';
import { goodsReceiptApi } from '../../../services/GoodsReceiptApi';
import type { GoodsReceiptDto } from '../../../types/GoodsReceipt';
import { GoodsReceiptType, GoodsReceiptStatus } from '../../../types/GoodsReceipt';

const { Option } = Select;
const { Title } = Typography;

export const GoodsReceiptForm: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [form] = Form.useForm();
    
    const [loading, setLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);
    const [completing, setCompleting] = useState(false);
    const [receipt, setReceipt] = useState<GoodsReceiptDto | null>(null);
    const [lines, setLines] = useState<any[]>([]);
    const [barcode, setBarcode] = useState('');
    const [resolvingBarcode, setResolvingBarcode] = useState(false);

    const isEditMode = !!id;
    const isReadOnly = receipt?.status === GoodsReceiptStatus.Completed || receipt?.status === GoodsReceiptStatus.Cancelled;

    useEffect(() => {
        if (isEditMode) {
            loadReceipt(id!);
        } else {
            form.setFieldsValue({
                type: GoodsReceiptType.Purchase,
                receiptDate: dayjs(),
                warehouseId: 'b3370ec5-85fc-4235-9cd4-36a8ceef1f11' // Mock Warehouse ID
            });
        }
    }, [id]);

    const loadReceipt = async (receiptId: string) => {
        setLoading(true);
        try {
            const res = await goodsReceiptApi.getById(receiptId);
            const data = res.data.data;
            setReceipt(data);
            form.setFieldsValue({
                ...data,
                receiptDate: dayjs(data.receiptDate)
            });
            setLines(data.lines || []);
        } catch (error) {
            message.error('Lỗi khi tải thông tin phiếu');
        } finally {
            setLoading(false);
        }
    };

    const handleSave = async (values: any) => {
        if (lines.length === 0) {
            message.error('Vui lòng thêm ít nhất một sản phẩm');
            return;
        }

        setSubmitting(true);
        try {
            const payload = {
                ...values,
                receiptDate: values.receiptDate.toISOString(),
                lines: lines
            };

            if (isEditMode) {
                await goodsReceiptApi.update(id!, payload);
                message.success('Cập nhật phiếu nháp thành công');
            } else {
                const res = await goodsReceiptApi.create(payload);
                message.success('Tạo phiếu nháp thành công');
                navigate(`/inventory/goods-receipt/${res.data.data.id}`, { replace: true });
            }
        } catch (error) {
            message.error('Lỗi khi lưu phiếu');
        } finally {
            setSubmitting(false);
        }
    };

    const handleComplete = async () => {
        if (!id) return;
        setCompleting(true);
        try {
            await goodsReceiptApi.complete(id);
            message.success('Chốt phiếu thành công!');
            loadReceipt(id);
        } catch (error) {
            message.error('Lỗi khi chốt phiếu');
        } finally {
            setCompleting(false);
        }
    };

    const handleBarcodeScan = async () => {
        if (!barcode) return;
        setResolvingBarcode(true);
        try {
            const res = await goodsReceiptApi.resolveBarcode(barcode);
            const product = res.data.data;
            
            // Check if product already exists in lines
            const existingIndex = lines.findIndex(l => l.productId === product.productId && l.uoMId === product.uoMId);
            if (existingIndex >= 0) {
                const newLines = [...lines];
                newLines[existingIndex].actualQuantity += 1;
                setLines(newLines);
            } else {
                setLines([...lines, {
                    id: null,
                    productId: product.productId,
                    productName: product.productName,
                    uoMId: product.uoMId,
                    uoMName: product.uoMName,
                    conversionRate: product.conversionRate,
                    expectedQuantity: 0,
                    actualQuantity: 1,
                    notes: ''
                }]);
            }
            setBarcode('');
        } catch (error) {
            message.error('Không tìm thấy sản phẩm với mã vạch này');
        } finally {
            setResolvingBarcode(false);
        }
    };

    const handleQuantityChange = (val: string, index: number) => {
        const num = Number(val);
        if (isNaN(num)) return;
        const newLines = [...lines];
        newLines[index].actualQuantity = num;
        setLines(newLines);
    };

    const removeLine = (index: number) => {
        const newLines = [...lines];
        newLines.splice(index, 1);
        setLines(newLines);
    };

    const columns = [
        {
            title: 'Sản phẩm',
            dataIndex: 'productName',
            key: 'productName',
        },
        {
            title: 'ĐVT',
            dataIndex: 'uoMName',
            key: 'uoMName',
        },
        {
            title: 'Dự kiến',
            dataIndex: 'expectedQuantity',
            key: 'expectedQuantity',
        },
        {
            title: 'Thực nhận',
            dataIndex: 'actualQuantity',
            key: 'actualQuantity',
            render: (text: number, record: any, index: number) => {
                if (isReadOnly) return text;
                return (
                    <Input 
                        type="number" 
                        value={text} 
                        onChange={(e) => handleQuantityChange(e.target.value, index)}
                        style={{ 
                            borderColor: record.expectedQuantity > 0 && text > record.expectedQuantity ? 'orange' : undefined 
                        }}
                    />
                );
            }
        },
        {
            title: 'Thao tác',
            key: 'action',
            render: (_: any, __: any, index: number) => {
                if (isReadOnly) return null;
                return (
                    <Button danger icon={<DeleteOutlined />} onClick={() => removeLine(index)} />
                );
            }
        }
    ];

    return (
        <Card loading={loading}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 24 }}>
                <Space>
                    <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/inventory/goods-receipt')} />
                    <Title level={4} style={{ margin: 0 }}>
                        {isEditMode ? `Chi tiết phiếu: ${receipt?.code || ''}` : 'Tạo Phiếu Nhập Kho'}
                    </Title>
                </Space>
                <Space>
                    {!isReadOnly && (
                        <Button type="default" icon={<SaveOutlined />} onClick={() => form.submit()} loading={submitting}>
                            Lưu nháp
                        </Button>
                    )}
                    {isEditMode && !isReadOnly && (
                        <Popconfirm
                            title="Chốt phiếu nhập kho"
                            description="Bạn có chắc chắn muốn chốt phiếu này? Thao tác này sẽ cập nhật tồn kho và KHÔNG THỂ hoàn tác!"
                            onConfirm={handleComplete}
                            okText="Chốt phiếu"
                            cancelText="Hủy"
                            okButtonProps={{ danger: true, loading: completing }}
                        >
                            <Button type="primary" danger icon={<CheckCircleOutlined />}>
                                Chốt phiếu
                            </Button>
                        </Popconfirm>
                    )}
                </Space>
            </div>

            <Form layout="vertical" form={form} onFinish={handleSave} disabled={isReadOnly}>
                <Row gutter={16}>
                    <Col xs={24} sm={12} md={6}>
                        <Form.Item name="type" label="Loại phiếu" rules={[{ required: true }]}>
                            <Select>
                                <Option value={GoodsReceiptType.Purchase}>Nhập mua hàng</Option>
                                <Option value={GoodsReceiptType.Return}>Nhập hàng trả lại</Option>
                                <Option value={GoodsReceiptType.Transfer}>Nhập chuyển kho</Option>
                            </Select>
                        </Form.Item>
                    </Col>
                    <Col xs={24} sm={12} md={6}>
                        <Form.Item name="referenceCode" label="Mã tham chiếu (PO/SO)" rules={[{ required: true }]}>
                            <Input placeholder="Nhập mã tham chiếu" />
                        </Form.Item>
                    </Col>
                    <Col xs={24} sm={12} md={6}>
                        <Form.Item name="warehouseId" label="Kho nhập" rules={[{ required: true }]}>
                            <Select>
                                <Option value="b3370ec5-85fc-4235-9cd4-36a8ceef1f11">Kho Trung Tâm</Option>
                            </Select>
                        </Form.Item>
                    </Col>
                    <Col xs={24} sm={12} md={6}>
                        <Form.Item name="receiptDate" label="Ngày nhập" rules={[{ required: true }]}>
                            <DatePicker style={{ width: '100%' }} format="DD/MM/YYYY HH:mm" showTime />
                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col span={24}>
                        <Form.Item name="notes" label="Ghi chú">
                            <Input.TextArea rows={2} />
                        </Form.Item>
                    </Col>
                </Row>
            </Form>

            <div style={{ marginTop: 24 }}>
                <Title level={5}>Chi tiết sản phẩm</Title>
                
                {!isReadOnly && (
                    <div style={{ marginBottom: 16 }}>
                        <Input
                            prefix={<ScanOutlined />}
                            placeholder="Quét mã vạch sản phẩm (nhấn Enter)"
                            value={barcode}
                            onChange={e => setBarcode(e.target.value)}
                            onPressEnter={handleBarcodeScan}
                            disabled={resolvingBarcode}
                            style={{ width: 300 }}
                        />
                    </div>
                )}

                <Table 
                    columns={columns} 
                    dataSource={lines} 
                    rowKey={(record, index) => record.id || index}
                    pagination={false}
                />
            </div>
        </Card>
    );
};

export default GoodsReceiptForm;
