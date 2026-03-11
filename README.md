# Hệ thống quản lý trung tâm gia sư

## Yêu cầu môi trường
- .NET SDK 8.0+
- SQL Server (hoặc LocalDB)

## Cấu hình
1. Cập nhật `web/appsettings.json` hoặc user-secrets:
   - `ConnectionStrings:DefaultConnection`
   - `EmailSettings:SenderEmail`
   - `EmailSettings:SenderPassword`
2. Không lưu secrets trực tiếp trong source code.

## Migrate DB
```bash
cd web
dotnet ef database update
```

## Chạy ứng dụng
```bash
cd web
dotnet run
```

## Tài khoản seed demo
- admin / Admin123!
- nhanvien / Nhanvien123!
- giasu / Giasu123!
- phuhuynh / Phuhuynh123!

## Test
```bash
dotnet test web/He_thong_Quan_Ly_trung_tam_gia_su.sln
```
