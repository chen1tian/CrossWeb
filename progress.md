# 会话进度日志

## 当前状态

- 最后更新：2026-07-30
- 当前任务：`phase1-002` — 为 Host 集成 PicoServer
- 最近完成：`phase1-001` — 创建 React 前端
- 下一个可执行任务：完成 `phase1-002` 的管理员环境运行验收
- 产品状态：第一阶段进行中。

## 已完成

- 建立五子系统项目 harness，并完成结构验证。
- 在 `main` 分支初始化 Git，并加入根级忽略规则。
- 使用 npm、Vite、React、TypeScript 和 Oxlint 创建 `Frontend/`。
- 加入严格 TypeScript 项目配置和 React `StrictMode`。
- 记录前端安装、开发、lint、类型检查、构建和预览命令。
- 在 `init.ps1` 和 `init.sh` 中加入可复现的前端依赖安装、lint 和构建检查。
- 验证开发服务器返回 HTTP 200 并提供 React 入口。
- 引用 PicoServer 1.7.6，并注册 ABP 单例 `PicoServerService`。
- 在 Host 生命周期中启动/停止 PicoServer，失败时记录错误并退出。
- 注册启用 CORS 的 `GET /api/ping`，处理程序返回 `pong`。
- 记录 `http://+:8090/` 监听策略、客户端地址和 Windows URL ACL。

## 进行中

- PicoServer 代码、生命周期、ping 路由和文档已完成。
- 当前机器缺少 `http://+:8090/` URL ACL；等待在管理员配置后完成真实
  HTTP、CORS 和退出释放端口验收。

## 下一步

1. 在管理员 PowerShell 中按 `Host/doc/PicoServer.md` 配置 URL ACL。
2. 启动 Desktop Host，验证 `GET /api/ping` 返回 `200` 和 `pong`。
3. 验证 CORS 响应以及 Host 退出后端口被释放。
4. 证据完整后把 `phase1-002` 标记为 `done`，再开始 `phase1-003`。

## 阻塞项与风险

- 当前账户不是管理员，且 `http://+:8090/` 没有 URL ACL，因此无法在本次
  环境完成真实 ping 冒烟测试。
- Avalonia WebView 包尚未选择；该事项属于 `phase1-003`。
- 最近一次完整 Host 验证为 0 个错误、68 个警告，包括包兼容性、多个
  NuGet 源、XML 文档、无效 `LIB` 搜索路径，以及 AutoMapper、Scriban、
  SQLitePCLRaw 传递依赖的已知安全公告。
- 当前没有自动化应用测试项目。
- 浏览器自动化无法启动本地 daemon，因此脚手架使用 HTTP 冒烟检查，
  没有生成浏览器截图证据。

## 已做决策

- 使用 `Frontend/` 作为 Web UI 目录。
- 使用 npm，并提交 `package-lock.json`。
- 使用 Vite + React + TypeScript 构建由 Host 托管的客户端 SPA。
- 使用 Oxlint 和严格 TypeScript 项目引用作为前端静态检查。
- 脚手架 UI 保持最小化，产品 UI 设计不属于 `phase1-001`。
- 第一阶段单个任务完成时不提交；`phase1-001` 至 `phase1-005` 全部完成
  且完整验证通过后，创建一个第一阶段里程碑提交。
- React 是 Web UI，Avalonia 是原生宿主外壳，ABP/PicoServer 是服务/API 层。
- 以 Desktop Host 项目作为基线，因为完整解决方案可能需要 Android、iOS
  和 Browser 工作负载。
- 除非现有 Host 警告阻塞当前任务，否则不把清理警告纳入第一阶段范围。
- 与用户沟通以及新增或更新的项目文档、进度、交接和提交信息统一使用中文。
- 按用户决定使用 PicoServer 原生的 `http://+:8090/` 强通配符监听，不限制
  为 `127.0.0.1`；部署时通过 URL ACL 和防火墙控制访问。

## 本次会话修改的文件

- `Frontend/`：React + TypeScript 应用、配置、锁文件和说明。
- `README.md`：前端位置和命令。
- `AGENTS.md`：项目拓扑、验证命令和中文工作语言规则。
- `feature_list.json`：第一阶段任务状态、完成条件和证据。
- `init.ps1` / `init.sh`：前端依赖、lint、构建检查及中文提示。
- `progress.md` / `session-handoff.md`：中文进度和恢复状态。
- `Host/src/Host/Services/PicoServerService.cs`：PicoServer 生命周期和 ping 路由。
- `Host/src/Host/App.axaml.cs` / `HostModule.cs`：应用生命周期和依赖注入。
- `Host/doc/PicoServer.md`：地址、URL ACL、生命周期和验证说明。

## 验证证据

- `npm run lint`：通过。
- `npm run typecheck`：通过。
- `npm run build`：通过；Vite 已生成 `Frontend/dist/`。
- 开发服务器 HTTP 冒烟检查：返回 `200 text/html`，React 根节点和模块入口存在。
- `.\init.ps1`：通过；Host 0 个错误/22 个警告，npm audit 0 个漏洞，
  前端 lint 和生产构建通过。
- Desktop Host 单独构建：通过；0 个错误、38 个警告。
- 最新 `.\init.ps1`：通过；Host 0 个错误/68 个警告，npm audit 0 个漏洞，
  前端 lint 和生产构建通过。
- URL ACL 缺失路径：通过；Host 记录明确错误并以 `-1` 退出，端口未残留。
