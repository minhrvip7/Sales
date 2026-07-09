# Feature: Inventory Management — Implementation Hub

> **Spec:** [`../../spec/spec-inventory.md`](../../spec/spec-inventory.md)
> **BE Design:** [`../../spec/design-inventory.md`](../../spec/design-inventory.md)

## 🚦 Global Status

| Track | Spoke file | Status |
|---|---|---|
| Backend | `implement-be-inventory.md` | 🚧 |
| **Quality Gate** | *(xem checklist bên dưới)* | 🚧 |
| Frontend | `implement-fe-inventory.md` | 🚧 |

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
- [ ] API endpoint GET trả về HTTP 200, đúng cấu trúc DTO có phân trang.
- [ ] Swagger Docs (`/swagger`) hiện đúng và đầy đủ thông tin XML Comments.

---

## 🌍 Project Deltas (Shared)
- [ ] **Data Migration**: Trường `StockQuantity` trong bảng `Products` bị đổi tên thành `OnHandQuantity`, và thêm `AllocatedQuantity`. Sẽ cần script migration cẩn thận để giữ lại tồn kho hiện tại.

---

## ❓ Open Questions (Cross-cutting)
### ⚠️ MEDIUM (Cần chốt trước khi sang FE)
- **Q1**: Có cần làm FE ngay trong task này không, hay chỉ cần cung cấp API backend trước? -> {User}
