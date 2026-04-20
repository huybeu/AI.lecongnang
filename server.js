require('dotenv').config();
const fs = require('fs');
const path = require('path');
const express = require('express');

const PORT = Number(process.env.PORT) || 3000;
const ANTHROPIC_KEY = process.env.ANTHROPIC_API_KEY;
const ALLOWED_MODEL = 'claude-sonnet-4-20250514';

const distPath = path.join(__dirname, 'client', 'dist');
const distIndex = path.join(distPath, 'index.html');
const hasDist = fs.existsSync(distIndex);

const app = express();
app.use(express.json({ limit: '512kb' }));

app.post('/api/chat', async (req, res) => {
  if (!ANTHROPIC_KEY) {
    res.status(500).json({ error: 'ANTHROPIC_API_KEY chưa cấu hình trên server' });
    return;
  }

  const { model, max_tokens: maxTokens, system, messages } = req.body || {};

  if (model !== ALLOWED_MODEL) {
    res.status(400).json({ error: 'Model không hợp lệ' });
    return;
  }
  if (!Array.isArray(messages) || messages.length === 0) {
    res.status(400).json({ error: 'messages phải là mảng không rỗng' });
    return;
  }

  const capped = Math.min(Number(maxTokens) || 400, 1024);

  try {
    const r = await fetch('https://api.anthropic.com/v1/messages', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'x-api-key': ANTHROPIC_KEY,
        'anthropic-version': '2023-06-01',
      },
      body: JSON.stringify({
        model: ALLOWED_MODEL,
        max_tokens: capped,
        system: typeof system === 'string' ? system : '',
        messages,
      }),
    });

    const data = await r.json();
    res.status(r.ok ? 200 : r.status).json(data);
  } catch (e) {
    res.status(502).json({ error: 'Lỗi kết nối tới Anthropic' });
  }
});

if (hasDist) {
  app.use(express.static(distPath));
} else {
  console.warn(
    '[server] Chưa có client/dist — chạy `npm run build` rồi `npm start`, hoặc dev: `npm run dev:api` + `npm run dev:client`'
  );
}

app.listen(PORT, () => {
  console.log(`http://localhost:${PORT}${hasDist ? ' (API + React build)' : ' (chỉ API)'}`);
});
