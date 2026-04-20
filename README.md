# AI.lecongnang

Landing (`index.html`) + backend Node (Express): phục vụ site tĩnh và `POST /api/chat` (proxy Anthropic, key chỉ trên server).

## Chạy local

Tạo file `.env` từ `.env.example`, điền `ANTHROPIC_API_KEY`, rồi:

```bash
npm install
npm start
```

Mở `http://localhost:3000` (hoặc `PORT` trong `.env`).
