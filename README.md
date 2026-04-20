# AI.lecongnang

- **Frontend:** React + **Vite** — shell trong `client/`, landing đầy đủ: `client/public/static-landing.html` (iframe).
- **Backend (tùy hosting):**
  - **Node.js + Express** — `server.js`, `POST /api/chat` proxy Anthropic.
  - **ASP.NET Core 8** — thư mục `aspnet/LeCongNang.Api/` cho **IIS 10 / Windows Server** (không cần Node trên host). Xem `aspnet/README.md`.

## Cài đặt (frontend + Node)

```bash
npm install
cd client && npm install && cd ..
```

## Chạy dev — Node (2 terminal)

1. API: `npm run dev:api` → `http://localhost:3000`
2. React: `npm run dev:client` → `http://localhost:5173` (proxy `/api` → 3000)

## Production — Node

```bash
npm run build
npm start
```

Cần `.env` với `ANTHROPIC_API_KEY`.

## Production — IIS / .NET

Không cài Node trên server: publish project trong `aspnet/`, copy `client/dist` vào `wwwroot`, cài **.NET 8 Hosting Bundle** trên IIS. Chi tiết: **`aspnet/README.md`**.

**MSSQL:** hiện không dùng cho chat; chỉ khi sau này có lưu form / lead.
