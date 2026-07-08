# Kỹ năng 3: Điều khiển Trạng thái & Khóa trường thông minh (UI Interaction)
## Tài liệu Kỹ năng Quy chuẩn (Developer Guideline)

Tài liệu này hướng dẫn cách kiểm soát và điều khiển thuộc tính tương tác của các controls trên giao diện dựa trên trạng thái nghiệp vụ trả về từ API.

---

## Quy trình Triển khai chi tiết (Step-by-Step Guide)

### Bước 1: Nhận diện trạng thái khóa từ Backend DTO
*   **Mô tả:** Nhận giá trị kiểm tra lịch sử giao dịch (ví dụ: trường `hasTransactions` trong dữ liệu sản phẩm) từ API trả về.

### Bước 2: Thiết lập vô hiệu hóa (disable) thuộc tính trên giao diện
*   **Mô tả:** Đặt giá trị thuộc tính `disabled` hoặc hiển thị thêm các dòng chữ cảnh báo nghiệp vụ cho người dùng biết tại sao trường dữ liệu này bị khóa.

> [!TIP]
> **Ví dụ cụ thể:** Thực hiện khóa dropdown đơn vị tính cơ bản của sản phẩm tại tệp [ProductList.tsx](file:///c:/Users/Public/Apps/Test/TestAI/front_end/src/features/product/ProductList.tsx):
> ```tsx
> <Form.Item
>   name="baseUnitId"
>   label="Đơn vị tính"
>   rules={[{ required: true, message: 'Vui lòng chọn đơn vị tính' }]}
>   
>   // 1. Hiển thị thông báo phụ nếu trường bị khóa
>   extra={
>     editingProduct?.hasTransactions ? (
>       <span style={{ color: 'orange', fontSize: '12px' }}>
>         Đã có giao dịch phát sinh trong lịch sử đơn hàng, không thể sửa đơn vị tính cơ bản.
>       </span>
>     ) : null
>   }
> >
>   {/* 2. Vô hiệu hóa dropdown nếu sản phẩm đã phát sinh hóa đơn */}
>   <Select 
>     placeholder="Chọn đơn vị" 
>     disabled={editingProduct?.hasTransactions}
>   >
>     {units.map(u => (
>       <Select.Option key={u.id} value={u.id}>{u.name}</Select.Option>
>     ))}
>   </Select>
> </Form.Item>
> ```
