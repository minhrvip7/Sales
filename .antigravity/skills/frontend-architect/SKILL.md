---
name: frontend-architect
description: Chuyên gia thiết kế và tối ưu hóa kiến trúc Front-End (React, TypeScript, Vite, Ant Design v5, RTK Query). Kích hoạt khi cần phân tích, nâng cấp hoặc xây dựng cấu trúc giao diện và gọi API trong thư mục \front_end.
---

# Hướng dẫn Kiến trúc sư Front-End (Frontend Architect Skill)

Bạn là một chuyên gia về kiến trúc Front-End hiện đại sử dụng React, TypeScript và Vite. Bạn chịu trách nhiệm giám sát và tối ưu hóa cấu trúc thư mục `front_end`.

---

## 1. Cấu trúc thư mục Front-End (React + TypeScript + Vite)

Cấu trúc dự án được phân chia rõ ràng để đảm bảo tính tái sử dụng và dễ mở rộng:

* **src/types**: Chứa các khai báo kiểu dữ liệu TypeScript dùng chung (`index.ts`). Mọi trao đổi dữ liệu qua API đều phải định nghĩa kiểu dữ liệu tại đây.
* **src/services**:
  - `apiClient.ts`: Cấu hình Axios Client chung cho các cuộc gọi cần interceptors nâng cao.
  - `store.ts`: Redux Store cấu hình RTK (Redux Toolkit).
  - **src/services/api/**: Chứa các **RTK Query Slices** (như `productApi.ts`, `orderApi.ts`, `categoryApi.ts`, `unitApi.ts`) quản lý dữ liệu API động và cơ chế cache.
* **src/components/layout**: Chứa các component bố cục dùng chung (Sidebar, Header, MainLayout).
* **src/features**: Phân chia giao diện theo tính năng nghiệp vụ độc lập (Ví dụ: `dashboard`, `product`, `category`, `unit`, `order`).

---

## 2. Quy tắc Phát triển & Tối ưu hóa

### 2.1 An toàn Kiểu dữ liệu & Verbatim Module Syntax
* Do dự án bật chế độ `"verbatimModuleSyntax": true` trong `tsconfig.json`, mọi khai báo import chỉ phục vụ định nghĩa kiểu dữ liệu bắt buộc phải dùng cú pháp type-only:
  ```typescript
  import type { Product, CreateProductDto } from '../../types';
  ```

### 2.2 Quản lý API & Cache với RTK Query
* Thay vì sử dụng Redux-Saga phức tạp, dự án ưu tiên dùng **RTK Query** để tự động hóa trạng thái tải dữ liệu (`isLoading`), lỗi (`isError`) và cơ chế cache.
* Áp dụng tính năng tự động làm mới bộ nhớ đệm (Cache Invalidation) qua hệ thống tag (`tagTypes` và `providesTags` / `invalidatesTags`). Ví dụ: Khi lập đơn hàng thành công, tag `Product` phải bị loại bỏ để buộc bảng sản phẩm tải lại số tồn kho mới nhất.

### 2.3 Quản lý Biến môi trường
* Không được ghi đè cứng domain API (`localhost:5000`) trong mã nguồn. Hãy sử dụng cấu hình từ `.env` thông qua tiền tố `VITE_`:
  ```typescript
  baseUrl: import.meta.env.VITE_API_BASE_URL
  ```

### 2.4 Phong cách Thiết kế (Aesthetics)
* Sử dụng hệ thống thiết kế hiện đại của **Ant Design v5** kết hợp với các biến CSS trong `index.css` để tinh chỉnh giao diện.
* Tránh sử dụng quá nhiều class ad-hoc cồng kềnh, ưu tiên tính nhất quán của theme, bo tròn viền (`border-radius`) và thêm hiệu ứng chuyển động mượt mà (transitions) cho các nút bấm và thẻ thông tin (Cards).
