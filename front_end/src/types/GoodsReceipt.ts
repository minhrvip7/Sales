export const GoodsReceiptType = {
    Purchase: 1,
    Return: 2,
    Transfer: 3
} as const;
export type GoodsReceiptType = typeof GoodsReceiptType[keyof typeof GoodsReceiptType];

export const GoodsReceiptStatus = {
    Draft: 0,
    Completed: 1,
    Cancelled: 2
} as const;
export type GoodsReceiptStatus = typeof GoodsReceiptStatus[keyof typeof GoodsReceiptStatus];

export interface GoodsReceiptSummaryDto {
    id: string;
    code: string;
    type: GoodsReceiptType;
    referenceCode: string;
    receiptDate: string;
    status: GoodsReceiptStatus;
    totalQuantity: number;
    createdDate: string;
}

export interface GoodsReceiptLineDto {
    id: string;
    productId: string;
    productName: string;
    uoMId: string;
    uoMName: string;
    conversionRate: number;
    expectedQuantity: number;
    actualQuantity: number;
    notes: string;
}

export interface GoodsReceiptDto {
    id: string;
    code: string;
    type: GoodsReceiptType;
    referenceId?: string | null;
    referenceCode: string;
    supplierId?: string | null;
    warehouseId: string;
    receiptDate: string;
    notes: string;
    status: GoodsReceiptStatus;
    totalQuantity: number;
    createdDate: string;
    lines: GoodsReceiptLineDto[];
}

export interface CreateGoodsReceiptLineDto {
    productId: string;
    uoMId: string;
    conversionRate: number;
    expectedQuantity: number;
    actualQuantity: number;
    notes: string;
}

export interface CreateGoodsReceiptDto {
    type: GoodsReceiptType;
    referenceId?: string | null;
    referenceCode: string;
    supplierId?: string | null;
    warehouseId: string;
    receiptDate: string;
    notes: string;
    lines: CreateGoodsReceiptLineDto[];
}

export interface UpdateGoodsReceiptLineDto {
    id?: string | null;
    productId: string;
    uoMId: string;
    conversionRate: number;
    expectedQuantity: number;
    actualQuantity: number;
    notes: string;
}

export interface UpdateGoodsReceiptDto {
    referenceId?: string | null;
    referenceCode: string;
    warehouseId: string;
    receiptDate: string;
    notes: string;
    lines: UpdateGoodsReceiptLineDto[];
}

export interface ResolveBarcodeDto {
    productId: string;
    productName: string;
    uoMId: string;
    uoMName: string;
    conversionRate: number;
    isBaseUnit: boolean;
}
