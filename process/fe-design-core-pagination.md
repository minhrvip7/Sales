# Frontend Design: Pagination & Virtualized Select

## 1. Cấu trúc dữ liệu & API Client (Axios)
Tạo file `src/types/common.ts` (hoặc bổ sung vào file tương đương):
```typescript
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
```

Cập nhật các hàm axios trong `src/services/` (ví dụ `categoryService.ts`, `productService.ts`) để truyền `params`:
```typescript
export const getPagedCategories = async (params: PagedRequest) => {
  const response = await axios.get<PagedResponse<Category>>('/api/category', { params });
  return response.data;
};
```

## 2. Cập nhật Ant Design Table
Tại các trang danh sách (`CategoryList`, `ProductList`, v.v.):
- Thêm state quản lý table: `pageNumber`, `pageSize`, `totalRecords`, `keyword`.
- Gọi hàm API khi một trong các state này thay đổi (sử dụng useEffect).
- Gắn cấu hình phân trang cho Antd `Table`:
```tsx
<Table 
  dataSource={data} 
  rowKey="id"
  pagination={{
    current: pageNumber,
    pageSize: pageSize,
    total: totalRecords,
    showSizeChanger: true,
    onChange: (page, size) => {
      setPageNumber(page);
      setPageSize(size);
    }
  }}
/>
```

## 3. Cập nhật Virtualized Select (cho Dropdown/Combobox)
- Áp dụng component `PaginatedSelect` (dựa trên tài liệu `virtualized_select` skill) thay thế cho `<Select>` truyền thống.
- Các vị trí áp dụng:
  - Form tạo/sửa Product (chọn Category, chọn Unit).
  - Form tạo/sửa Order (chọn Product).

```tsx
<Form.Item name="categoryId" label="Danh mục">
  <PaginatedSelect 
    fetchData={async (page, keyword) => {
      const res = await getPagedCategories({ pageNumber: page, pageSize: 20, keyword });
      return { data: res.data, totalPages: res.totalPages };
    }}
    valueProp="id"
    labelProp="name"
    placeholder="Chọn danh mục"
  />
</Form.Item>
```
