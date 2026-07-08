# Kỹ năng Đọc và Phân tích Tài liệu (Document Reading & Analysis Skills)
## Tài liệu Hướng dẫn Quy chuẩn (Developer Guideline)

Tài liệu này tổng hợp các kỹ năng cốt lõi và phương pháp đọc, phân tích tài liệu nghiệp vụ (SRS/BRD) cũng như đặc tả kỹ thuật (Spec) trong quá trình phát triển phần mềm.

---

## 1. Kỹ năng Đọc tài liệu Nghiệp vụ (Business & SRS Analysis)

### 1.1. Đọc lướt (Scanning) để nắm tổng quan
*   **Mục tiêu:** Hiểu được **Mục đích (Why)** và **Đối tượng sử dụng (Who)** của tính năng.
*   **Hành động:** Đọc phần Giới thiệu, Phạm vi (Scope) và mô tả sơ lược về tính năng để định hình bức tranh toàn cảnh trước khi đi vào chi tiết.

### 1.2. Phân tích luồng nghiệp vụ chính và ngoại lệ (Happy Path vs. Edge Cases)
*   **Happy Path (Luồng chuẩn):** Xác định luồng đi lý tưởng nhất khi không xảy ra bất kỳ lỗi hay ngoại lệ nào (ví dụ: Khách chọn hàng -> Thanh toán thành công -> Trừ kho).
*   **Edge Cases (Luồng ngoại lệ):** Tìm kiếm và đặt câu hỏi cho các trường hợp đặc biệt:
    *   Điều gì xảy ra nếu hàng trong kho bị hết giữa chừng?
    *   Nếu khách hủy đơn hàng, hệ thống có hoàn lại kho không? Tính theo tỷ lệ nào?
    *   Khi thanh toán thất bại, trạng thái đơn hàng sẽ như thế nào?

### 1.3. Bóc tách và phân loại Quy tắc Nghiệp vụ (Business Rules - BR)
*   Liệt kê toàn bộ các ràng buộc nghiệp vụ (Constraints) mà hệ thống bắt buộc phải tuân theo.
*   Phân loại các ràng buộc thành:
    *   **Ràng buộc cứng (Hard block):** Hệ thống chặn hoàn toàn thao tác (ví dụ: Không cho sửa đơn vị cơ bản nếu đã có hóa đơn).
    *   **Cảnh báo mềm (Warning):** Hệ thống cảnh báo nhưng cho phép tiếp tục nếu người dùng xác nhận.
    *   **Ràng buộc dữ liệu (Data constraints):** Hệ số quy đổi phải $> 0$, mã vạch không được trùng lặp,...

---

## 2. Kỹ năng Đọc tài liệu Kỹ thuật (Technical Spec Analysis)

### 2.1. Đọc hiểu Sơ đồ Kiến trúc & Dữ liệu
*   **Sơ đồ Thực thể (ERD):** Phân tích các thực thể mới, các mối quan hệ ($1-1, 1-N, N-N$) và các khóa ngoại ($FK$), chỉ số ($Index$) cần bổ sung.
*   **Sơ đồ tuần tự (Sequence Diagram):** Xác định trình tự gọi API và luồng dữ liệu chạy giữa Client, Web Server, Database và các bên thứ ba (Third-party APIs).

### 2.2. Phân tích vùng ảnh hưởng (Impact Analysis)
Khi đọc một Spec thay đổi, luôn tự đặt câu hỏi:
*   **Tương thích ngược (Backward Compatibility):** Thay đổi này có làm hỏng các API hoặc dữ liệu cũ không? (Ví dụ: Đổi tên cột `UnitId` thành `BaseUnitId` có làm sập Frontend cũ không? Cách giải quyết: Giữ các trường alias hoặc cập nhật đồng bộ).
*   **Hiệu năng (Performance):** Việc thêm truy vấn kiểm tra chéo (như check unique barcode) có làm chậm luồng lưu sản phẩm không? Cần thêm Index vào cột nào để tối ưu?

---

## 3. Quy trình làm rõ Nghiệp vụ (Clarification Process)

Khi phát hiện các điểm chưa rõ ràng hoặc mâu thuẫn trong tài liệu:
1.  **Ghi chép có cấu trúc:** Ghi lại câu hỏi dưới dạng bảng bao gồm: *Điểm nghi vấn, Lý do nghi vấn (rủi ro kỹ thuật/dữ liệu), Phương án đề xuất xử lý*.
2.  **Chốt phương án sớm:** Gửi bảng nghi vấn cho BA hoặc Khách hàng để chốt phương án bằng văn bản trước khi bắt đầu lập kế hoạch triển khai.
3.  **Cập nhật tài liệu nguồn:** Ngay sau khi chốt phương án, cập nhật lại tài liệu SRS/Spec để đảm bảo tài liệu luôn phản ánh đúng code thực tế.
