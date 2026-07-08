# Kỹ năng 2: Thiết kế Form động với Form.List (Ant Design Forms)
## Tài liệu Kỹ năng Quy chuẩn (Developer Guideline)

Tài liệu này hướng dẫn cách cấu hình biểu mẫu dạng mảng động (cho phép thêm, xóa, sửa các bản ghi con) bằng thành phần `Form.List` của Ant Design.

---

## Quy trình Triển khai chi tiết (Step-by-Step Guide)

### Bước 1: Khai báo thành phần Form.List trong biểu mẫu
*   **Mô tả:** Nhúng `<Form.List name="conversions">` vào trong biểu mẫu chính. `name` của list phải trùng với thuộc tính mảng trong DTO gửi lên server.

### Bước 2: Render các dòng nhập liệu động
*   **Mô tả:** Sử dụng callback nhận vào `fields` và các helper function `add`, `remove` để render ra giao diện dòng nhập liệu tương ứng.
*   **Quy chuẩn đặt tên trường con:** Đặt tên trường con theo cú pháp `name={[name, 'fieldName']}` để Antd tự động trích xuất thành mảng các đối tượng.

> [!TIP]
> **Ví dụ cụ thể:** Triển khai bảng nhập đơn vị tính quy đổi động trong tệp [ProductList.tsx](file:///c:/Users/Public/Apps/Test/TestAI/front_end/src/features/product/ProductList.tsx):
> ```tsx
> <Form.List name="conversions">
>   {(fields, { add, remove }) => (
>     <>
>       {fields.map(({ key, name, ...restField }) => (
>         <Space key={key} style={{ display: 'flex', marginBottom: 8 }} align="baseline">
>           {/* Dropdown chọn Đơn vị quy đổi */}
>           <Form.Item
>             {...restField}
>             name={[name, 'alternativeUnitId']}
>             rules={[{ required: true, message: 'Chọn đơn vị' }]}
>           >
>             <Select placeholder="Đơn vị" style={{ width: 130 }}>
>               {units.map(u => (
>                 <Select.Option key={u.id} value={u.id}>{u.name}</Select.Option>
>               ))}
>             </Select>
>           </Form.Item>
> 
>           {/* Hệ số quy đổi */}
>           <Form.Item
>             {...restField}
>             name={[name, 'conversionRate']}
>             rules={[{ required: true, message: 'Nhập hệ số' }]}
>           >
>             <InputNumber placeholder="Hệ số (>0)" min={0.0001} style={{ width: 100 }} />
>           </Form.Item>
> 
>           {/* Mã vạch UOM */}
>           <Form.Item {...restField} name={[name, 'barcode']}>
>             <Input placeholder="Mã vạch riêng" style={{ width: 120 }} />
>           </Form.Item>
> 
>           {/* Giá bán ghi đè (không bắt buộc) */}
>           <Form.Item {...restField} name={[name, 'price']}>
>             <InputNumber placeholder="Giá bán (₫)" min={0} style={{ width: 120 }} />
>           </Form.Item>
> 
>           {/* Nút xóa dòng cấu hình */}
>           <Button type="text" danger onClick={() => remove(name)} icon={<DeleteOutlined />} />
>         </Space>
>       ))}
>       
>       {/* Nút thêm dòng mới */}
>       <Form.Item>
>         <Button type="dashed" onClick={() => add()} block icon={<PlusOutlined />}>
>           Thêm Đơn vị quy đổi
>         </Button>
>       </Form.Item>
>     </>
>   )}
> </Form.List>
> ```
