# Template: `implement-{module}-{feature}.md` (Hub)

**Mục đích**: File index quản lý tiến độ tổng thể của feature, chứa các deltas chung của dự án, và định nghĩa Quality Gate giữa BE và FE.

**Áp dụng**: Sinh ở Phase 2.5. Là file trung tâm nối kết `implement-be` và `implement-fe`.

---

# Feature: {Feature Name} — Implementation Hub

> **Spec:** [`docs/spec/{module}/spec-{module}-{feature}.md`](...)
> **BE Design:** [`docs/design/{module}/design-{module}-{feature}.md`](...)
> **FE Design:** [`docs/design/{module}/fe-design-{module}-{feature}.md`](...)

## 🚦 Global Status

| Track | Spoke file | Status |
|---|---|---|
| Backend | `implement-be-{module}-{feature}.md` | 🚧 |
| **Quality Gate** | *(xem checklist bên dưới)* | 🚧 |
| Frontend | `implement-fe-{module}-{feature}.md` | 🚧 |

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
- [ ] API endpoint `GET /api/{entity}` trả về HTTP 200, đúng cấu trúc DTO có phân trang.
- [ ] API endpoint `POST /api/{entity}` tạo mới thành công, nhận đúng body.
- [ ] Swagger Docs (`/swagger`) hiện đúng và đầy đủ thông tin XML Comments cho các endpoints trên.

---

## 🌍 Project Deltas (Shared)

*(Ghi lại những thay đổi hoặc overrides cụ thể ở cấp độ dự án ảnh hưởng đến cả BE và FE cho riêng feature này. Vd: Auth role mới, rule multi-tenancy đặc biệt...)*
- [ ] Thay đổi 1: ...
- [ ] Thay đổi 2: ...

---

## ❓ Open Questions (Cross-cutting)

*(Các câu hỏi mở liên quan đến sự giao tiếp giữa BE và FE. Các câu hỏi thuần tuý BE hoặc thuần tuý FE hãy để trong file Spoke tương ứng).*

### ⚠️ MEDIUM (Cần chốt trước khi sang FE)
- **Q1**: {Câu hỏi} -> {Ai chịu trách nhiệm trả lời}

### LOW (Defer)
- **Q2**: ...
