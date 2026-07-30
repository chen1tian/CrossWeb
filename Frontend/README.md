# AvWeb Frontend

AvWeb 的 React Web UI。该项目由 Avalonia Host 中的 WebView 承载，并将在
后续功能中通过 PicoServer 调用本地 Host API。

## Toolchain

- Node.js 22.12 或更高版本
- npm
- Vite
- React
- TypeScript
- Oxlint

## Commands

在 `Frontend/` 目录运行：

```bash
npm ci
npm run dev
npm run lint
npm run typecheck
npm run build
npm run preview
```

生产构建输出到 `Frontend/dist/`。根目录的 `init.ps1` 和 `init.sh` 会安装
锁定依赖并运行 lint 与生产构建。

## Current Scope

当前仅完成前端工程脚手架。PicoServer API、Avalonia WebView 集成和
ping/pong 交互分别由后续 harness 功能跟踪。
