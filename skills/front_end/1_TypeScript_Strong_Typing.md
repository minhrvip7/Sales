# Kỹ năng 1: Khai báo Kiểu dữ liệu & Đồng bộ DTOs (TypeScript Interfaces)
## Tài liệu Kỹ năng Quy chuẩn (Developer Guideline)

Tài liệu này hướng dẫn cách định nghĩa các kiểu dữ liệu và interfaces kiểu mạnh trong TypeScript để đồng bộ dữ liệu chặt chẽ với Backend DTOs.

---

## Quy trình Triển khai chi tiết (Step-by-Step Guide)

### Bước 1: Khai báo Interfaces cho Các đối tượng Quy đổi
*   **Mô tả:** Định nghĩa các interface con chứa thông tin phụ trợ (như đơn vị quy đổi, chi tiết hóa đơn quy đổi) để nhúng vào các thực thể chính.

> [!TIP]
> **Ví dụ cụ thể:** Định nghĩa interface `ProductUnitConversion` lưu cấu hình quy đổi đơn vị tính của sản phẩm tại tệp [index.ts](file:///c:/Users/Public/Apps/Test/TestAI/front_end/src/types/index.ts):
> ```typescript
> export interface ProductUnitConversion {
>   alternativeUnitId: string;
>   alternativeUnitName?: string;
>   conversionRate: number;
>   barcode?: string;
>   price?: number;
> }
> ```

---

### Bước 2: Đồng bộ cấu hình DTO tạo mới và Thực thể chính
*   **Mô tả:** Cập nhật các interface DTO gửi lên API và thực thể hiển thị trên client để đảm bảo tính nhất quán thuộc tính (như chuyển `unitId` thành `baseUnitId` và thêm danh sách quy đổi).

> [!TIP]
> **Ví dụ cụ thể:** Cập nhật interface `Product` và `CreateProductDto` trong tệp [index.ts](file:///c:/Users/Public/Apps/Test/TestAI/front_end/src/types/index.ts):
> ```typescript
> export interface CreateProductDto {
>   name: string;
>   code: string;
>   barcode?: string;
>   description?: string;
>   price: number;
>   cost: number;
>   categoryId: string;
>   baseUnitId: string;
>   conversions: ProductUnitConversion[];
> }
> 
> export interface Product {
>   id: string;
>   name: string;
>   code: string;
>   barcode?: string;
>   description?: string;
>   price: number;
>   cost: number;
>   stockQuantity: number;
>   categoryId: string;
>   category?: Category;
>   baseUnitId: string;
>   baseUnit?: Unit;
>   status: boolean;
>   createdDate: string;
>   hasTransactions: boolean; // Trạng thái sản phẩm đã phát sinh hóa đơn bán hàng
>   conversions: ProductUnitConversion[];
> }
> ```
