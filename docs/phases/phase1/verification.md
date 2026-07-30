# 第一阶段验证记录

## 最终基线

2026-07-30 在 Windows 上运行 `.\init.ps1`，结果如下：

| 检查 | 结果 |
|---|---|
| npm 安装与审计 | 通过，0 个漏洞 |
| Oxlint | 通过 |
| TypeScript、Vite 和 `build:host` | 通过，生成 3 个 Host 资源 |
| Desktop Host 构建 | 通过，0 个错误、100 个既有警告 |

## 运行与端到端验收

| 场景 | 结果 |
|---|---|
| Windows UAC 提升启动 | 通过，Host 进程和窗口正常响应 |
| `GET /api/ping` | `200 pong` |
| `GET /api/host/urls` | 返回回环地址和当前有效的局域网 IPv4 地址 |
| 静态首页、JavaScript、CSS | 均返回 200，内容类型正确 |
| CORS | 正确回显请求 Origin |
| 浏览器首屏 | 内容非空，无错误覆盖层 |
| 浏览器控制台与页面错误 | 无 |
| 浏览器交互 | 点击“发送 ping”后由“等待测试”切换为 `pong` |
| Avalonia WebView | React 首屏、可访问 URL 和 `pong` 状态均显示正常 |
| Host 退出 | 8090 端口正常释放 |

Avalonia WebView 的截图用于当次人工验收，属于临时验证产物，未提交到 Git。

## 任务证据

### phase1-001：创建 React 前端

- 建立 `Frontend/package.json`，提供 `dev`、`lint`、`typecheck`、`build`
  和 `preview` 命令。
- `Frontend/README.md` 记录环境、技术栈、开发命令和构建输出。
- lint、类型检查、生产构建和 HTTP 冒烟检查通过。
- `init.ps1` 和 `init.sh` 已纳入前端验证。

### phase1-002：为 Host 集成 PicoServer

- PicoServer 1.7.6 注册为 ABP 单例，并随 Desktop Host 生命周期启动和停止。
- 服务监听 `http://+:8090/`，提供 CORS、`/api/ping` 和
  `/api/host/urls`。
- Windows Desktop 使用 `requireAdministrator` 清单获得通配符监听权限。
- UAC 提升后的 `ping`、CORS、URL 列表和退出释放端口均验证通过。

### phase1-003：在 Avalonia WebView 中显示 React UI

- Host 使用 NativeWebView 承载 React 页面。
- Web 资源经过 `build:host` 生成并由 PicoServer 同源托管。
- 只有真实健康检查通过后才从 `about:blank` 导航到页面。
- 前端就绪消息、加载状态和中文错误状态已实现。
- Windows Desktop 的实际 WebView 显示通过人工验收。

### phase1-004：完成 ping/pong 集成

- React 使用同源 `fetch('/api/ping')` 请求 Host。
- PicoServer 返回纯文本 `pong`，React 正确更新界面状态。
- 浏览器自动化和 Avalonia WebView 均完成端到端验证。

### phase1-005：关闭第一阶段 MVP

- 两个平台初始化脚本均覆盖前端和 Desktop Host 基线检查。
- 根 README、前端 README 和 PicoServer 文档记录了复现步骤。
- 第一阶段五个任务全部完成，不存在 MVP 阻塞项。
- 创建里程碑提交 `381d40b`。
