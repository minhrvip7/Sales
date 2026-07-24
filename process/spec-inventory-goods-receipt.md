# Spec: Tính năng Nhập Kho (Goods Receipt)

**Document ID:** SPEC-INVENTORY-GOODS-RECEIPT
**Status:** Ready for design
**Module:** Inventory
**Feature:** Inbound (Nhập kho)

## 1. Mục tiêu
Cung cấp tính năng cho thủ kho/nhân viên kho (End-user) thực hiện tạo và quản lý Phiếu nhập kho (Goods Receipt) từ các chứng từ gốc (Purchase Order, Return Order, Transfer Order), kiểm đếm thực tế bằng mã vạch hoặc nhập tay, và ghi nhận tồn kho cũng như giá vốn một cách chính xác.

## 2. Quy trình nghiệp vụ (Business Flow)
1. **Tạo phiếu:** User chọn loại phiếu (Purchase/Return/Transfer) và chọn chứng từ tham chiếu tương ứng.
2. **Tải dữ liệu:** Hệ thống tự động tải thông tin nhà cung cấp (nếu là Purchase) và danh sách sản phẩm (Expected Quantity) từ chứng từ gốc.
3. **Kiểm đếm (Lines):**
   - **Quét mã vạch:** Quét mã vạch để gọi API phân giải sản phẩm. Cộng dồn số lượng thực tế (`ActualQuantity`) vào dòng tương ứng. Nếu quét sản phẩm ngoài PO, tự động tạo dòng mới kèm cảnh báo.
   - **Nhập tay:** Chọn sản phẩm, chọn đơn vị tính, nhập số lượng đếm thực tế.
4. **Kiểm tra chênh lệch:** Hệ thống cho phép `ActualQuantity` > `ExpectedQuantity` nhưng sẽ hiển thị cảnh báo (Warning) nổi bật trên giao diện.
5. **Hoàn tất (Posting):**
   - User chốt phiếu. Hệ thống thực hiện đồng bộ trong 1 transaction: Đổi trạng thái phiếu thành "Completed", cộng tồn kho, tính lại giá vốn theo Bình quân gia quyền (Moving Average Cost).
   - Phiếu đã chốt không thể sửa, xóa hay thao tác lại.

## 3. Cấu trúc dữ liệu yêu cầu (Data Requirements)
### 3.1. Phiếu Nhập Kho (Goods Receipt Header)
- `Id`: Khóa chính
- `Code`: Mã phiếu (Ví dụ: `GR-YYMM-00001`)
- `Type`: Enum (`Purchase=1`, `Return=2`, `Transfer=3`)
- `ReferenceId`: Khóa ngoại trỏ đến ID của PO/SO/TO (tùy thuộc Type)
- `ReferenceCode`: Chuỗi lưu mã chứng từ gốc (để hiển thị nhanh)
- `SupplierId`: Trỏ đến Nhà cung cấp (Lấy từ PO, Read-only)
- `WarehouseId`: Kho nhập
- `ReceiptDate`: Ngày nhập thực tế
- `Notes`: Ghi chú
- `Status`: Enum (`Draft=0`, `Completed=1`, `Cancelled=2`)
- `TotalQuantity`: Tổng số lượng thực tế (của tất cả các dòng)
- *Các trường Audit & ISoftDelete theo chuẩn của AGENTS.md*

### 3.2. Dòng Phiếu Nhập (Goods Receipt Line)
- `Id`: Khóa chính
- `GoodsReceiptId`: FK trỏ về Header
- `ProductId`: FK trỏ về Sản phẩm
- `UoMId` (Unit of Measure): FK trỏ về Đơn vị tính
- `ConversionRate`: Tỷ lệ quy đổi so với đơn vị cơ bản lúc nhập
- `ExpectedQuantity`: Số lượng dự kiến từ chứng từ gốc (tính theo UoM)
- `ActualQuantity`: Số lượng thực tế nhập kho (tính theo UoM)
- `Notes`: Ghi chú tại từng dòng (VD: Hàng móp méo, ướt...)

## 4. Ràng buộc & Cảnh báo (Constraints & Warnings)
- **Validation khi hoàn tất:**
  - Chỉ cho phép POST (Hoàn tất) khi trạng thái hiện tại là `Draft`.
  - Phải có ít nhất 1 dòng sản phẩm (Line) có `ActualQuantity > 0`.
- **Cảnh báo vượt mức:** Tại Frontend, nếu dòng nào có `ActualQuantity > ExpectedQuantity`, highlight dòng đó (màu cam/đỏ) để user lưu ý, nhưng không block thao tác POST.
- **Toàn vẹn dữ liệu:** Không có Hard Delete, mọi Entity phải kế thừa interface `ISoftDelete` và áp dụng Global Query Filter (theo chuẩn quy định Backend).

## 5. Kế hoạch tiếp theo
- Chuyển sang **Phase 2 & 2b (Backend & Frontend Design)** để thiết kế cấu trúc chi tiết (Entities, DTO, AppService, API, Route, UI Modal).
