# Quy trình: SRS → Feature hoàn chỉnh với AI Agent

**Document ID:** PROC-AI-01
**Áp dụng cho:** Dự án Sales (C# Clean Architecture, React + Ant Design)
**Mục đích:** Quy trình chuẩn để team kết hợp AI agent khi nhận 1 SRS mới — đảm bảo output đúng nghiệp vụ + đúng convention codebase.

> **Convention 1**: SRS gốc đóng băng khi spec được tạo. Single source of truth từ đó là `spec-{module}-{feature}.md`. KHÔNG sửa SRS nữa.
>
> **Convention 2**: Phase 2.5 sản xuất **3 file hub-and-spoke** (hub + BE spoke + FE spoke) thay vì 1 file lớn. Lý do: (1) Cấu trúc rõ ràng ép verify Backend trước khi vào Frontend; (2) session BE / FE load context nhỏ hơn (giảm chi phí token); (3) BE/FE PR review độc lập.

---

## 0. Bối cảnh

Project được tối ưu cho AI Agent thao tác trên cấu trúc C# Clean Architecture (Backend) và React + Ant Design + Axios (Frontend). 
Bạn có thể linh hoạt sử dụng bất kỳ AI Agent nào (ví dụ: Antigravity subagents, Claude, v.v.), miễn là cấp đúng Context (Quy tắc + Template) cho Agent ở mỗi bước.

---

## Tổng quan 10 phase

```
┌────┐ ┌────┐ ┌────┐ ┌────┐ ┌────┐ ┌─────┐  ┌─────┐ ┌────┐ ┌────┐ ┌────┐
│1.BA│→│1.5.│→│2.BE│→│2b. │→│2.5.│→│3a.  │→ │3b.  │→│4.  │→│5.  │→│6.  │
│Conc│ │SPEC│ │DESI│ │FE  │ │IMPL│ │BUILD│  │BUILD│ │TEST│ │REVI│ │SHIP│
│ept │ │    │ │GN  │ │DESI│ │DOC │ │BE   │  │FE   │ │    │ │EW  │ │    │
│    │ │    │ │    │ │    │ │×3  │ │     │  │     │ │    │ │    │ │    │
└────┘ └────┘ └────┘ └────┘ └────┘ └─────┘  └─────┘ └────┘ └────┘ └────┘
 Human  Human   AI     AI    Human   AI        AI    Man +   AI   Human
 + AI   + AI                 + AI                     AI
 chat   chat                 chat
                                        ┃
                                   Quality gate
                                   (BE → FE):
                                   build✓ API✓
```

> **Quality gate** giữa mỗi phase: bạn duyệt artifact của phase trước rồi mới sang phase sau. KHÔNG để AI chạy thẳng từ SRS đến code 1000 dòng — không ai review nổi.

---

## PHASE 1 — BA Review & Concept (Human + AI chat)

Đưa SRS thô cho AI phản biện trước, sau đó thảo luận với BA để chốt phương án.
- **Output:** File `concept-{module}-{feature}.md` chứa các quyết định, scope in/out, open questions.

## PHASE 1.5 — Spec Extraction (Human + AI chat)

Trích xuất từ Concept + SRS gốc ra bản Spec chính thức.
- **Output:** File `spec-{module}-{feature}.md` (status `Ready for design`). Bản Spec này sẽ là single source of truth.

## PHASE 2 — Backend Design (AI Agent)

AI khảo sát codebase hiện tại, và design chi tiết backend.
- **Output:** `design-{module}-{feature}.md` (C# Signatures, Entity properties, DTOs, AppService, EF Migrations plan).

## PHASE 2b — FE Design (AI Agent)

AI thiết kế Frontend dựa trên Spec + BE Design, ứng dụng Ant Design components.
- **Output:** `fe-design-{module}-{feature}.md` (Route, Modal/Page layout, Axios API call map, Lookup variant).

## PHASE 2.5 — Implementation Doc (Human + AI chat)

Phân tách công việc thành checklist chi tiết cho session BE và session FE độc lập.
- **Output:** 3 file trong thư mục implement:
  - `implement-{module}-{feature}.md` (hub)
  - `implement-be-{module}-{feature}.md` (BE spoke)
  - `implement-fe-{module}-{feature}.md` (FE spoke)

## PHASE 3a — Backend Implementation (AI Agent session)

Mở session AI mới chỉ load: `implement-be-*.md` + `spec-*.md` + `design-*.md`.
- **Thực thi:** Theo thứ tự Core/Domain → Infrastructure (EF/DbContext) → Application (DTO, Service) → Api (Controller, Swagger).
- **Quality Gate:** `dotnet build` pass, Entity Framework Update Database thành công, Endpoint API test qua Postman/Swagger trả về đúng DTO.

## PHASE 3b — Frontend Implementation (AI Agent session)

Mở session AI mới chỉ load: `implement-fe-*.md` + `spec-*.md` + `fe-design-*.md`. 
*Yêu cầu:* Phase 3a Quality Gate phải xanh (API backend chạy ổn định).
- **Thực thi:** Khởi tạo Route → Tạo Redux Slice / Axios Call → Giao diện Danh sách (Antd Table) → Form Thêm/Sửa (Antd Modal/Form) → Chi tiết.
- **Quality Gate:** `npm run build` pass, UI render không lỗi console.

## PHASE 4 — Test & Verification (Human + AI Debug)

Chạy hệ thống lên và kiểm tra UI thực tế trên browser. Test golden paths, edge cases. Gọi AI debug nếu có lỗi.

## PHASE 5 — Code Review

AI rà soát lại các convention, Clean Architecture constraints, check permissions và bảo mật.

## PHASE 6 — Ship & Knowledge Capture

Tạo PR. Nếu có pattern hoặc quy tắc mới xuất hiện trong quá trình code, cập nhật vào file `AGENTS.md` (backend) hoặc frontend rules để Agent học cho các task sau.
