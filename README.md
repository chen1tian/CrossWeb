# 跨平台应用

## 说明
这是一个新建的跨平台项目，技术架构如下：
1. 前端：`Frontend/` 下的 Vite + React + TypeScript 项目
2. 后端（Host）：跨平台Avalonia项目

后端Host使用WebView承载前端的网页，使用PicoServer作为本地Web服务器，前端React项目通过PicoServer访问后端Host提供的API接口。
PicoServer(https://picoserver.cn/guide.html)
PcoServer 实时通讯 文档: https://picoserver.cn/realtime.html

## 前端开发

前端使用 Node.js、npm、Vite、React 和 TypeScript。在 `Frontend/` 目录运行：

```bash
npm ci
npm run dev
npm run lint
npm run typecheck
npm run build
```

详细说明参见 [`Frontend/README.md`](./Frontend/README.md)。根目录的
`init.ps1` 或 `init.sh` 会同时验证 Host 和前端。

## 本地 API

Host 通过 PicoServer 监听 `http://+:8090/`，本机客户端可通过
`http://127.0.0.1:8090` 访问 API。

```text
GET http://127.0.0.1:8090/api/ping
→ pong
```

生命周期、端口策略和验证方式参见
[`Host/doc/PicoServer.md`](./Host/doc/PicoServer.md)。

## 需求

### 第一阶段

1. 创建前端文件
2. 修改后端Host:
   1. 增加PicoServer支持
   2. 修改为WebView承载前端网页
3. 完成最小可用产品（MVP）：
   1. 前端React项目可以通过PicoServer访问后端Host提供的API接口
   2. 后端Host可以通过WebView显示前端网页
   3. 一次请求交互：ping，pong，前端发送ping请求，后端返回pong响应

