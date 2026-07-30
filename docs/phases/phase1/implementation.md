# 第一阶段实现说明

## 交付范围

第一阶段交付了以下能力：

- `Frontend/` 中的 Vite、React、TypeScript 和 Oxlint 前端工程。
- Host 中的 PicoServer 1.7.6、静态资源托管、CORS 和本地 API。
- Avalonia NativeWebView，以及加载、失败和前端就绪状态。
- Windows Desktop 的 `requireAdministrator` UAC 清单。
- React 页面中的 `ping` / `pong` 交互和可访问 URL 列表。
- Windows 与 POSIX shell 的统一基线验证脚本。

## 构建与运行链路

1. `npm run build:host` 完成 TypeScript 检查和 Vite 生产构建。
2. 构建脚本复制 Web 产物、生成资源清单，并将其作为 Avalonia 资源纳入 Host。
3. Host 启动时把资源解包到应用数据目录。
4. PicoServer 使用 `http://+:8090/` 托管静态资源和 API。
5. Host 通过真实的 `/api/ping` 健康检查确认服务就绪。
6. Avalonia WebView 从 `about:blank` 导航到 `http://127.0.0.1:8090/`。
7. React 发送 `avweb.frontendReady` 消息，Host 隐藏加载层。

## 本地接口

| 路径 | 用途 |
|---|---|
| `GET /api/ping` | 返回纯文本 `pong`，用于健康检查与端到端验证。 |
| `GET /api/host/urls` | 返回回环地址和当前可用的局域网 IPv4 地址。 |

## 主要文件

| 位置 | 职责 |
|---|---|
| `Frontend/package.json` | 前端开发、检查和 Host 资源构建命令。 |
| `Frontend/src/` | React 页面、Host API 调用和前端就绪握手。 |
| `Frontend/scripts/` | Host Web 资源准备与清单生成。 |
| `Host/src/Host/PicoServer/` | PicoServer 生命周期、路由和静态资源服务。 |
| `Host/src/Host.Desktop/` | Windows Desktop 启动、UAC 和 WebView 宿主。 |
| `Host/src/Host/Assets/Web/` | 构建时生成的 Avalonia 内嵌 Web 资源。 |
| `init.ps1`、`init.sh` | 仓库级基线验证入口。 |

技术使用方式继续维护在
[Frontend/README.md](../../../Frontend/README.md) 和
[Host/doc/PicoServer.md](../../../Host/doc/PicoServer.md)。
