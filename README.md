# Hướng dẫn Khởi chạy Dự án Quản lý Bán hàng (Sales Management)

Hệ thống bao gồm hai phần chính: **Back-End** (.NET 8 Web API + PostgreSQL) và **Front-End** (React + TypeScript + Vite + Ant Design v5).

---

## 🛠️ Yêu cầu hệ thống
* **.NET SDK**: Phiên bản 8.0 hoặc 9.0.
* **Node.js**: Phiên bản 18+ (kèm npm).
* **PostgreSQL**: Đang chạy trên cổng `5432` tại local máy tính.

---

## 🚀 Hướng dẫn khởi chạy Back-End

Mở Terminal (Command Prompt hoặc PowerShell) tại thư mục `back_end`:

### Bước 1: Khởi tạo/Cập nhật Database (nếu cần thiết)
Nếu chưa cập nhật cấu trúc database, chạy lệnh sau để áp dụng Migration vào PostgreSQL:
```bash
dotnet ef database update --project Sales.Infrastructure --startup-project Sales.Api
```

### Bước 2: Chạy ứng dụng API
Chạy lệnh sau để khởi động dự án Web API:
```bash
dotnet run --project Sales.Api
```
* **Swagger UI** (Môi trường Development): Truy cập tại đường dẫn [http://localhost:5000/swagger](http://localhost:5000/swagger) để kiểm tra các API endpoint.

---

## 💻 Hướng dẫn khởi chạy Front-End

Mở một Terminal mới tại thư mục `front_end`:

### Bước 1: Cài đặt thư viện (nếu chưa cài đặt)
```bash
npm install
```

### Bước 2: Khởi chạy môi trường Phát triển (Local Dev)
Khởi động Vite development server:
```bash
npm run dev
```
* **Ứng dụng Web**: Truy cập đường dẫn hiển thị trên Terminal (mặc định là [http://localhost:3000](http://localhost:3000)).
* **Cấu hình API**: Bạn có thể thay đổi đường dẫn API backend tại tệp tin `.env` bằng cách sửa thuộc tính `VITE_API_BASE_URL`.
