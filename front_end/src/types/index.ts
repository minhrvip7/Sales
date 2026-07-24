export interface CustomerGroup {
  id: string;
  name: string;
  code: string;
  description?: string;
  discountPercentage: number;
  status: boolean;
  createdDate: string;
}

export interface Customer {
  id: string;
  name: string;
  code: string;
  phone?: string;
  email?: string;
  address?: string;
  customerGroupId: string;
  customerGroup?: CustomerGroup;
  status: boolean;
  createdDate: string;
}

export interface Category {
  id: string;
  name: string;
  code: string;
  description?: string;
  status: boolean;
  createdDate: string;
}

export interface Unit {
  id: string;
  name: string;
  code: string;
  description?: string;
  status: boolean;
  createdDate: string;
}

export interface ProductUnitConversion {
  alternativeUnitId: string;
  alternativeUnitName?: string;
  conversionRate: number;
  barcode?: string;
  price?: number;
}

export interface CreateProductDto {
  name: string;
  code: string;
  barcode?: string;
  description?: string;
  price: number;
  cost: number;
  categoryId: string;
  baseUnitId: string;
  conversions: ProductUnitConversion[];
}

export interface Product {
  id: string;
  name: string;
  code: string;
  barcode?: string;
  description?: string;
  price: number;
  cost: number;
  stockQuantity: number;
  categoryId: string;
  categoryName?: string;
  category?: Category;
  baseUnitId: string;
  baseUnitName?: string;
  baseUnit?: Unit;
  status: boolean;
  createdDate: string;
  hasTransactions: boolean;
  conversions: ProductUnitConversion[];
}

export interface OrderDetail {
  id?: string;
  orderId?: string;
  productId: string;
  product?: Product;
  unitId: string;
  unit?: Unit;
  conversionRate: number;
  quantity: number;
  unitPrice: number;
  discountPercentage: number;
  discountAmount: number;
  totalAmount: number;
}

export interface Order {
  id: string;
  orderNumber: string;
  customerId: string;
  customer?: Customer;
  orderDate: string;
  subTotal: number;
  discountAmount: number;
  taxAmount: number;
  totalAmount: number;
  notes?: string;
  orderStatus: number; // 0: Draft, 1: Confirmed, 2: Completed, 3: Cancelled
  paymentStatus: number; // 0: Unpaid, 1: Partially Paid, 2: Fully Paid
  createdDate: string;
  orderDetails: OrderDetail[];
}

export interface CreateOrderDetailDto {
  productId: string;
  quantity: number;
  discountPercentage: number;
}

export interface CreateOrderDto {
  customerId: string;
  notes?: string;
  orderDetails: CreateOrderDetailDto[];
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  message: string;
  data: T;
}

export interface CreateCategoryDto {
  name: string;
  code: string;
  description?: string;
}

export interface CreateUnitDto {
  name: string;
  code: string;
  description?: string;
}

export interface InventoryBalance {
  id: string;
  productId: string;
  productCode: string;
  productName: string;
  onHandQty: number;
  allocatedQty: number;
  availableQty: number;
}

export interface InventoryTransaction {
  id: string;
  productId: string;
  productName: string;
  type: number;
  referenceNumber: string;
  transactedQty: number;
  transactedUomName: string;
  baseQty: number;
  reason?: string;
  createdDate: string;
}

export interface PagedRequest {
  pageNumber: number;
  pageSize: number;
  keyword?: string;
}

export interface PagedResponse<T> {
  data: T[];
  totalRecords: number;
  totalPages: number;
  pageNumber: number;
  pageSize: number;
}
