# SRS: Quản lý Tồn kho (Inventory Management)

**Trạng thái:** Ready for design
**Tài liệu tham khảo:** PRD-INV-001, concept-inventory-management.md

## 1. Mục tiêu và Phạm vi (Overview & Scope)
- **Mục tiêu:** Kiểm soát luồng hàng hóa (Nhập, Xuất, Kiểm kê, Điều chỉnh), đảm bảo số liệu thực tế khớp với hệ thống. Quản lý tồn kho dưới 3 trạng thái để chống bán vượt mức (overselling), hỗ trợ quy đổi Đơn vị tính (UOM) và lưu vết giao dịch.
- **Phạm vi:** 
  - Chỉ quản lý một kho duy nhất (Single Warehouse).
  - Quản lý các luồng: Nhập kho (có tham chiếu PO), Xuất kho (có quét Barcode), Kiểm kê & Điều chỉnh (Stocktake & Adjustment).

## 2. Các Trạng thái Tồn kho Cơ bản
Quản lý 3 trạng thái tồn kho cho mỗi sản phẩm:
- **On-hand (Tồn kho thực tế):** Số lượng vật lý đang có trong kho.
- **Allocated/Reserved (Hàng đã giữ chỗ):** Số lượng hàng nằm trong các Đơn bán hàng (Sales Order) đã được xác nhận (`Confirmed`) nhưng chưa xuất kho thực tế.
- **Available (Tồn kho khả dụng):** Công thức: `Available = On-hand - Allocated`. Được dùng để cho phép khách hàng đặt mua mới hoặc thực hiện các lệnh xuất kho khác.

## 3. Luồng Nghiệp vụ Cốt lõi (Core Flows)

### 3.1. Nghiệp vụ Nhập kho (Inbound / Goods Receipt)
- Giao dịch làm tăng tồn kho `On-hand` (và kéo theo `Available` tăng).
- Có thể tạo "Phiếu Nhập Kho" tham chiếu từ Đơn đặt hàng mua (Purchase Order).
- **Hỗ trợ UOM:** Cho phép nhập theo Đơn vị quy đổi (VD: Thùng). Hệ thống tự tra cứu Hệ số quy đổi (VD: 1 Thùng = 24 Lon) và nhân lên để cộng vào tồn kho của Đơn vị cơ bản.
- **Tính giá vốn:** Cập nhật giá vốn nhập kho theo thuật toán Bình quân gia quyền (Moving Average) trên Đơn vị cơ bản. *Lưu ý: Giá vốn được lưu tại một bảng riêng biệt (VD: `ProductCosts`), không lưu chung trong bảng `Product`.*

### 3.2. Nghiệp vụ Xuất kho (Outbound / Goods Issue)
- Giao dịch xuất bán cho khách hàng: Làm giảm cả `On-hand` và `Allocated`.
- Giao dịch xuất kho khác (tiêu hao, hủy,...): Làm giảm `On-hand` (và kéo theo `Available` giảm).
- **Cơ chế chặn (Block):** 
  - Chặn giao dịch xuất kho nếu số lượng xuất yêu cầu lớn hơn `On-hand`.
  - Chặn giao dịch đối với các loại xuất khác (không qua Sales Order) nếu làm `Available` bị âm.
- **Áp dụng Barcode:** Quét mã vạch Đơn vị quy đổi, hệ thống tự sinh dòng xuất kho cho đơn vị đó với số lượng 1.

### 3.3. Kiểm kê kho & Điều chỉnh (Stocktake & Adjustment)
- Tạo Phiếu Kiểm kê (Stocktake Ticket), snapshot số lượng `On-hand` hiện tại của hệ thống.
- Nhập Số lượng đếm được thực tế (Counted Qty).
- Hệ thống tính **Chênh lệch (Variance)** = Counted Qty - System On-hand.
- Sau khi xác nhận phiếu kiểm kê, hệ thống tự động sinh ra "Phiếu Điều Chỉnh" (Inventory Adjustment):
  - Variance > 0: Sinh phiếu Nhập điều chỉnh (Thừa), tăng `On-hand`.
  - Variance < 0: Sinh phiếu Xuất điều chỉnh (Thiếu), giảm `On-hand`.

## 4. Quy tắc Tích hợp Đơn vị tính (UOM Rules)
- Để duy trì tính nhất quán, thẻ kho / lịch sử giao dịch (bảng `InventoryTransactions`) phải luôn lưu 3 trường sau cho mỗi dòng giao dịch:
  - `TransactedQty`: Số lượng theo đơn vị lúc thao tác (VD: 5).
  - `TransactedUom`: Đơn vị lúc thao tác (VD: Thùng).
  - `BaseQty`: Số lượng quy đổi tuyệt đối cộng/trừ vào kho theo đơn vị cơ bản (VD: 120 Lon).

## 5. Ghi vết Hệ thống & Bảo mật (ISMS Compliance)
- **Không được phép xóa cứng (Hard Delete):** Các phiếu xuất, nhập, điều chỉnh đã "Hoàn tất" (Completed/Posted) không được xóa vật lý. Mọi thực thể liên quan phải áp dụng Soft Delete theo quy chuẩn dự án (`ISoftDelete`).
- **Giao dịch bù trừ (Reversal Transaction):** Khi có sai sót trong các giao dịch đã hoàn tất, người dùng phải tạo một Phiếu Điều chỉnh ngược lại kèm theo **Lý do điều chỉnh** bắt buộc.
- **Audit Trail:** Mọi thay đổi trong cấu hình tồn kho phải lưu log với thông tin: Tài khoản thực hiện, Timestamp (Ngày giờ), Địa chỉ IP và Loại thao tác.

## 6. Các Quyết định thiết kế dữ liệu bổ sung (Data Decisions)
- Do chỉ có 1 kho duy nhất, bảng lưu số dư tồn kho (`InventoryBalances`) sẽ không cần khóa tham chiếu WarehouseId, chỉ cần tham chiếu theo ProductId.
- Quá trình phân bổ (Allocation): Tự động cộng dồn vào `AllocatedQty` ngay khi Sales Order được cập nhật trạng thái thành `Confirmed`.
