# Feature: Goods Receipt — Implementation Hub

> **Spec:** [`../spec-inventory-goods-receipt.md`](../spec-inventory-goods-receipt.md)
> **BE Design:** [`../design-inventory-goods-receipt.md`](../design-inventory-goods-receipt.md)
> **FE Design:** [`../fe-design-inventory-goods-receipt.md`](../fe-design-inventory-goods-receipt.md)

## 🚦 Global Status

| Track | Spoke file | Status |
|---|---|---|
| Backend | `implement-be-inventory-goods-receipt.md` | 🚧 |
| **Quality Gate** | *(xem checklist bên dưới)* | 🚧 |
| Frontend | `implement-fe-inventory-goods-receipt.md` | 🚧 |

> Khi Backend hoàn tất, AI/Dev sẽ đổi status Backend thành ✅, kiểm tra Quality Gate, sau đó mới tiến hành track Frontend.

---

## 🛑 Quality gate (Phase 3a → 3b)

*(BẮT BUỘC pass toàn bộ trước khi bắt đầu FE session)*

**Kiểm tra tĩnh (AI Agent tự chạy):**
- [ ] Tất cả phase BE spoke = ✅
- [ ] `dotnet build` trong thư mục `back_end/` — 0 errors
- [ ] `dotnet ef database update` — thành công (hoặc schema đã apply DB)

**Kiểm tra động (Chạy API thực tế):**
- [ ] Backend Server đang chạy (`dotnet run`).
- [ ] API endpoint `GET /api/goods-receipts` trả về HTTP 200, đúng cấu trúc DTO có phân trang.
- [ ] API endpoint `POST /api/goods-receipts` tạo mới thành công, nhận đúng body.
- [ ] Swagger Docs (`/swagger`) hiện đúng và đầy đủ thông tin XML Comments cho các endpoints trên.

---

## 🌍 Project Deltas (Shared)
- [ ] Tích hợp tính năng `Virtualized Select` cho combobox chọn Sản Phẩm / Kho nhập.
- [ ] Đảm bảo tuân thủ kiến trúc Clean Architecture, SoftDelete (theo Rule AGENTS.md).

---

## ❓ Open Questions (Cross-cutting)
(Không có)
