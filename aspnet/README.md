# Backend ASP.NET Core (IIS / Windows Server)

Dùng khi hosting **không có Node.js**, chỉ **IIS 10 + .NET**.

## Yêu cầu trên server

- Cài **[.NET 8 Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/8.0)** (gồm runtime + ASP.NET Core Module cho IIS).
- Trong IIS: Application Pool **No Managed Code**, pipeline **Integrated**.

## Cấu hình API key

- **Không** commit key vào git.
- Trên IIS: biến môi trường `ANTHROPIC_API_KEY`, **hoặc** `Anthropic:ApiKey` trong `appsettings.Production.json` / User Secrets (chỉ dev).

## Publish & static frontend

1. Build React (từ thư mục gốc repo):

   ```bash
   npm run build
   ```

2. Copy toàn bộ nội dung `client/dist/*` vào `aspnet/LeCongNang.Api/wwwroot/` (ghi đè).

3. Publish API:

   ```bash
   cd aspnet/LeCongNang.Api
   dotnet publish -c Release -o ./publish
   ```

4. Trong IIS: trỏ site đến thư mục `publish` (physical path).

Endpoint: **`POST /api/chat`** — giống bản Node; landing trong `wwwroot` gọi `/api/chat` cùng host.

## SQL Server

Project này **không dùng MSSQL** cho chat. Chỉ cần database khi sau này anh/chị thêm đăng ký, lưu lead, v.v.
