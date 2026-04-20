# AI.lecongnang

- **Frontend:** React 19 + **Vite** — shell SPA nhúng landing đầy đủ qua `public/static-landing.html` (có thể tách dần thành component React).
- **Backend:** **Node.js + Express** — `POST /api/chat` proxy Anthropic (key chỉ trên server).

## Cài đặt

```bash
npm install
cd client && npm install && cd ..
```

## Chạy dev (2 terminal)

1. API: `npm run dev:api` → `http://localhost:3000` (chỉ API nếu chưa build client).
2. React: `npm run dev:client` → `http://localhost:5173` (proxy `/api` → 3000).

Mở **http://localhost:5173** — chat trong landing gọi `/api/chat` qua proxy.

## Production

```bash
# Tạo .env với ANTHROPIC_API_KEY
npm run build
npm start
```

Mở **http://localhost:3000** — Express phục vụ `client/dist` + API.

## Nền tảng backend (gợi ý deploy)

Giữ **Express** trên VPS / **Railway** / **Render** / **Fly.io**; hoặc sau này đổi sang **Fastify** / **NestJS** nếu cần cấu trúc lớn hơn — không bắt buộc đổi khi quy mô hiện tại.
