import axiosInstance from './apiClient';
import type { PagedRequest, PagedResponse, ApiResponse } from '../types';
import type {
    GoodsReceiptDto,
    GoodsReceiptSummaryDto,
    CreateGoodsReceiptDto,
    UpdateGoodsReceiptDto,
    ResolveBarcodeDto
} from '../types/GoodsReceipt';

export const goodsReceiptApi = {
    getPaged: (params: PagedRequest) =>
        axiosInstance.get<ApiResponse<PagedResponse<GoodsReceiptSummaryDto>>>('/goods-receipts', { params }),

    getById: (id: string) =>
        axiosInstance.get<ApiResponse<GoodsReceiptDto>>(`/goods-receipts/${id}`),

    create: (data: CreateGoodsReceiptDto) =>
        axiosInstance.post<ApiResponse<GoodsReceiptDto>>('/goods-receipts', data),

    update: (id: string, data: UpdateGoodsReceiptDto) =>
        axiosInstance.put<ApiResponse<GoodsReceiptDto>>(`/goods-receipts/${id}`, data),

    complete: (id: string) =>
        axiosInstance.post<ApiResponse<GoodsReceiptDto>>(`/goods-receipts/${id}/complete`),

    resolveBarcode: (barcode: string) =>
        axiosInstance.get<ApiResponse<ResolveBarcodeDto>>('/goods-receipts/resolve-barcode', { params: { barcode } })
};
