# Concept: Quản lý Tồn kho (Inventory Management)

## 1. Bối cảnh & Mục tiêu (Background & Goals)
- **Mô-đun:** Quản lý Tồn kho (Inventory Management).
- **Mục tiêu:** Kiểm soát chặt chẽ luồng hàng hóa luân chuyển (Nhập, Xuất, Kiểm kê, Điều chỉnh), quản lý đa trạng thái tồn kho để chống bán vượt mức (overselling), hỗ trợ đa đơn vị tính (UOM) và tuân thủ các quy tắc ghi vết giao dịch.

## 2. Phạm vi (Scope)
### In-Scope (Trong phạm vi):
- Quản lý 3 trạng thái tồn kho cơ bản: On-hand, Allocated, Available.
- Nghiệp vụ Nhập kho (Inbound): Tham chiếu PO, tính tồn kho theo UOM quy đổi, cập nhật giá vốn (Moving Average).
- Nghiệp vụ Xuất kho (Outbound): Chặn tồn kho âm, tự động trừ On-hand và Allocated, hỗ trợ xuất kho qua Barcode.
- Nghiệp vụ Kiểm kê (Stocktake) & Điều chỉnh (Adjustment): Đối soát số liệu thực tế, tự động sinh phiếu điều chỉnh (Nhập thừa / Xuất thiếu).
- Quy tắc lưu trữ UOM: Ghi nhận `Transacted_Qty`, `Transacted_UOM` và `Base_Qty` trong mọi giao dịch kho.
- Ghi vết (ISMS): Soft delete cho các chứng từ, bắt buộc tạo giao dịch bù trừ (Reversal) khi có sai sót, lưu trữ Audit Trail cho cấu hình tồn kho.

### Out-of-Scope (Ngoài phạm vi - Tạm thời):
- Quản lý tồn kho theo Lô/Hạn sử dụng (Batch/Expiry) hoặc Số Serial (Serial Number) - *Chưa đề cập trong PRD*.
- Quản lý đa kho (Multi-warehouse / Location) hoặc vị trí Bin - *Chưa rõ có yêu cầu hay không*.

## 3. Các Quyết định Kỹ thuật (Technical Decisions)
- **Entity Tồn kho (Inventory):** Sẽ duy trì một bảng `InventoryBalances` để theo dõi nhanh số dư của từng sản phẩm: `OnHandQty`, `AllocatedQty`, `AvailableQty` (được tính toán hoặc duy trì tự động).
- **Entity Giao dịch kho (InventoryTransactions):** Bảng lịch sử/thẻ kho, luôn lưu trữ `Transacted_Qty`, `Transacted_UOM` và quy đổi ra `Base_Qty`.
- **Cơ chế Tính Giá vốn (Moving Average):** Giá vốn bình quân gia quyền sẽ được tính toán lại ngay sau mỗi giao dịch **Nhập kho** và lưu tại mức Sản phẩm (`Product` hoặc `InventoryValuation`).

## 4. Các câu hỏi phản biện & Làm rõ (Open Questions)
Để thiết kế chính xác hơn, cần BA/User xác nhận các vấn đề sau:

1. **Quản lý đa kho (Multi-warehouse):** Hệ thống có quản lý nhiều kho (Warehouse) hay chỉ 1 kho duy nhất? (Nếu đa kho, `InventoryBalances` cần thêm `WarehouseId`).
2. **Quy tắc chặn Xuất kho:** PRD nêu "chặn nếu số lượng yêu cầu xuất > số lượng Tồn kho On-hand hiện có". Tuy nhiên, nếu xuất kho không phải cho Đơn bán hàng (ví dụ: xuất tiêu hao, xuất hủy), việc xuất này có cần check giới hạn của `Available` để tránh làm `Available` bị âm hay không?
3. **Lưu trữ Giá vốn (Moving Average Cost):** Giá vốn bình quân gia quyền được lưu trực tiếp trên bảng `Product` hay có một bảng riêng biệt như `ProductCosts` để theo dõi theo thời gian/kho?
4. **Phân bổ Tồn kho (Allocation):** Quá trình "Allocate" (tăng AllocatedQty) xảy ra chính xác ở thời điểm nào? Ngay khi Sales Order được duyệt (Confirmed), hay có một bước "Xác nhận Giữ kho" riêng biệt?

## 5. Next steps
- Xin ý kiến BA/User để trả lời các câu hỏi ở phần 4.
- Sau khi chốt, sẽ chuyển sang **Phase 1.5** để viết file `spec-inventory-management.md`.
