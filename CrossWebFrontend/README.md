# CrossWeb 前端

CrossWeb 的 React Web UI。该项目由 Avalonia CrossWebHost 中的 WebView 承载，并
通过 PicoServer 调用本地 CrossWebHost API。

## 工具链

- Node.js 22.12 或更高版本
- npm
- Vite
- React
- TypeScript
- Oxlint

## 命令

在 `CrossWebFrontend/` 目录运行：

```bash
npm ci
npm run dev
npm run lint
npm run typecheck
npm run build
npm run build:host
npm run preview
```

`npm run build:host` 会先生成 `CrossWebFrontend/dist/`，再把生产资源和资源清单复制
到 `CrossWebHost/src/CrossWebHost/Assets/Web/`，供 Avalonia 嵌入。该目标目录是生成目录，不
提交到 Git。

开发服务器会把 `/api` 代理到 `http://127.0.0.1:8090`。生产环境中页面和 API
均由 PicoServer 同源提供。

## 当前功能

- 页面完成两帧渲染后向 Avalonia WebView 发送 `crossweb.frontendReady`。
- “发送 ping”调用同源 `GET /api/ping` 并显示 `pong`。
- 页面读取 `GET /api/host/urls`，显示当前可访问的回环和局域网地址。

根目录的 `init.ps1` 和 `init.sh` 会安装锁定依赖、运行 lint、执行包含
TypeScript 检查的生产构建，并构建 Desktop CrossWebHost。
