# CrossWeb

CrossWeb 是一个融合 **React Web UI、Avalonia 原生宿主和本地 .NET 服务**的跨平台应用起点，同时提供可通过 `dotnet new crossweb` 创建新项目的 .NET 模板。

React 负责页面和交互，Avalonia 通过 WebView 承载页面，PicoServer 在应用进程内提供静态资源和 HTTP API；ABP 与 EF Core 提供依赖注入、应用分层和数据持久化基础。生产前端随宿主程序集分发，运行应用时无需启动 Vite 开发服务器。

## 当前能力与平台范围

- 内嵌 React 生产资源，由 PicoServer 同源提供页面和 API。
- 启动时初始化 ABP、执行 SQLite 数据库迁移，再启动本地 HTTP 服务。
- 服务健康检查通过后加载页面；前端发送就绪消息后显示 WebView，启动失败时显示中文错误信息。
- 示例页面可发送 `ping` 并显示 `pong`，同时列出回环地址和局域网 IPv4 地址。
- 支持从源码目录或本地 NuGet 模板包生成独立命名的项目。

当前页面用于验证宿主与前端的连通性。ABP 业务分层和数据库基础已具备，具体业务功能由使用模板的项目继续实现。

| 平台 | 当前范围与已有验证 |
|---|---|
| Windows Desktop | 默认构建目标；阶段归档已记录 WebView、API、模板实例的端到端验收 |
| Android | 已有启动项目、私有数据目录和回环 HTTP 配置；维护归档记录了 Release 构建及真机打开前端页面 |
| macOS / Linux Desktop | 由 Desktop 项目提供入口；尚无对应平台的运行验收记录 |
| iOS | 保留启动项目；尚无运行验收记录 |
| Browser / WebAssembly | 保留启动项目；当前本地 PicoServer 与原生 WebView 组合尚未完成此平台验收 |

以上验收情况来自仓库归档，当前环境的构建结果以实际运行初始化脚本为准。Vite 开发页面可以在浏览器中访问；这与 `CrossWebHost.Browser` 的 WebAssembly 启动项目是两个不同入口。

## 架构与目录

```text
React 页面
  ├─ HTTP /api/* ────────────────→ PicoServer → 本地应用服务
  └─ crossweb.frontendReady ─────→ Avalonia WebView 加载状态

Avalonia 宿主
  └─ ABP 初始化 → SQLite 迁移 → 解包前端资源 → 启动 PicoServer
       → ping 健康检查 → WebView 导航 → 前端就绪后显示页面
```

当前 `/api/ping` 和 `/api/host/urls` 由 `PicoServerService` 显式注册。新增业务 HTTP 接口时，需要在服务端明确接入应用服务。

```text
CrossWeb/
├── CrossWebFrontend/                   # Vite + React + TypeScript
│   ├── src/                            # 页面与 WebView 就绪通知
│   └── scripts/prepare-host-web.mjs     # 复制生产资源并生成清单
├── CrossWebHost/
│   ├── CrossWebHost.sln
│   ├── doc/                            # 宿主、PicoServer 与 ABP 文档
│   └── src/
│       ├── Directory.Packages.props    # NuGet 版本集中管理
│       ├── CrossWebHost/               # 共享宿主、WebView 与本地 HTTP 服务
│       ├── CrossWebHost.Desktop/       # Windows / macOS / Linux 入口
│       ├── CrossWebHost.Android/       # Android 入口
│       ├── CrossWebHost.iOS/           # iOS 入口
│       ├── CrossWebHost.Browser/       # WebAssembly 入口
│       └── abp/                       # Domain、Application.Contracts、
│                                      # Application、EntityFramework 及数据库扩展
├── .template.config/                   # dotnet new 配置与名称替换规则
├── .template-content/                  # 新项目专用 README 与初始任务状态
├── template-pack/                      # NuGet 模板打包项目
├── docs/                               # 已完成工作与阶段归档
├── init.ps1 / init.sh                   # 统一基线验证入口
└── AGENTS.md                           # 仓库工作规则
```

主要技术栈为 .NET 10、Avalonia 12、ABP 10、EF Core 10、PicoServer 1.7.6，以及 React 19、Vite 8、TypeScript 6 和 Oxlint。具体依赖以 `Directory.Packages.props`、前端 `package.json` 和 `package-lock.json` 为准。

## 快速开始

### 环境要求

- .NET 10 SDK。
- Node.js 与 npm，建议使用 Node.js 22.13+ 的 22.x 版本或 Node.js 24 LTS，并满足锁定依赖的 `engines` 要求。
- Windows 运行宿主时需要允许 UAC 提升，并具备系统 WebView 运行环境（WebView2）。
- `8090` 端口可用，依赖还原时能够访问所需的 npm / NuGet 源。

Android、iOS 和 WebAssembly 项目需要对应平台的 SDK 与 .NET workload；默认初始化脚本仅构建 Desktop 项目。

### 初始化与构建

在仓库根目录运行：

```powershell
.\init.ps1
```

POSIX shell 使用：

```bash
./init.sh
```

脚本依次显示 .NET SDK 版本，执行 `npm ci`、`npm run lint`、`npm run build:host`，最后构建 `CrossWebHost.Desktop`；任一步失败即停止。`build:host` 包含 TypeScript 检查、Vite 生产构建和宿主资源准备。

### 运行 Windows 桌面应用

完成初始化后，在仓库根目录运行：

```powershell
.\CrossWebHost\src\CrossWebHost.Desktop\bin\Debug\net10.0\CrossWebHost.Desktop.exe
```

允许 UAC 提升后，等待 WebView 显示 React 页面，点击“发送 ping”，状态应变为 `pong`。应用清单使用 `requireAdministrator`，以获得 PicoServer 通配符监听权限；运行时请启动 `.exe`，以应用自身的清单生效。

宿主运行期间也可用浏览器打开 [http://127.0.0.1:8090/](http://127.0.0.1:8090/)。局域网地址由当前网卡枚举生成，其他设备能否访问还取决于网络和防火墙配置。

## 开发方式

### React 页面开发

先构建并启动宿主，再在另一个终端运行：

```powershell
Set-Location .\CrossWebFrontend
npm run dev
```

打开 Vite 输出的开发地址。开发服务器将 `/api` 请求代理到 `http://127.0.0.1:8090`，因此测试 API 交互时仍需运行宿主。

| 命令（在 `CrossWebFrontend/` 中执行） | 用途 |
|---|---|
| `npm ci` | 按锁文件安装依赖 |
| `npm run dev` | 启动 Vite 开发服务器 |
| `npm run lint` | 运行 Oxlint |
| `npm run typecheck` | 运行 TypeScript 检查 |
| `npm run build` | 类型检查并生成 `dist/` |
| `npm run build:host` | 生产构建并准备宿主内嵌资源 |
| `npm run prepare:host` | 将已有 `dist/` 复制到宿主并生成资源清单 |
| `npm run preview` | 预览生产构建；此入口未配置开发服务器的 API 代理 |

要让桌面 WebView 显示前端修改，在仓库根目录依次执行以下命令，然后重启宿主：

```powershell
npm --prefix CrossWebFrontend run build:host
dotnet build .\CrossWebHost\src\CrossWebHost.Desktop\CrossWebHost.Desktop.csproj --nologo
```

资源流转为 `CrossWebFrontend/dist/` → `CrossWebHost/src/CrossWebHost/Assets/Web/` → Avalonia 内嵌资源 → 运行时解包目录。`dist/`、`Assets/Web/`、`bin/` 和 `obj/` 均为生成内容，不提交 Git。

### 宿主与本地服务开发

React 页面放在 `CrossWebFrontend/src/`；Avalonia 视图、视图模型和宿主服务分别放在共享项目的 `Views/`、`ViewModels/` 和 `Services/`。领域对象、DTO / 接口、应用服务和数据访问放在 `src/abp/` 对应项目；平台相关入口与配置放在对应平台项目。

修改宿主前阅读 [CrossWebHost 开发规则](./CrossWebHost/AGENTS.md)。

## 本地 API 与运行数据

PicoServer 当前使用固定端口 `8090`，监听前缀为 `http://+:8090/`，WebView 导航到 `http://127.0.0.1:8090/`。生产页面与 API 同源，服务同时启用了 CORS。

| 请求 | 响应 |
|---|---|
| `GET /` | React 生产首页 |
| `GET /api/ping` | 纯文本 `pong` |
| `GET /api/host/urls` | JSON，`urls` 数组包含回环地址和有效局域网 IPv4 地址 |

宿主运行时可执行：

```powershell
(Invoke-WebRequest http://127.0.0.1:8090/api/ping).Content
Invoke-RestMethod http://127.0.0.1:8090/api/host/urls
```

当前数据库使用 SQLite，启动时自动应用待执行迁移。MySQL 和 SQL Server 扩展项目已保留，默认启动流程使用 SQLite。

| 数据 | 默认位置 |
|---|---|
| Desktop SQLite 数据库 | 应用基目录下的 `db/CrossWebHost.db` |
| Android SQLite 数据库 | 应用私有目录下的 `db/CrossWebHost.db` |
| Desktop 前端解包目录 | `LocalApplicationData/CrossWeb/host-web/` |
| Android 前端解包目录 | 应用私有目录下的 `CrossWeb/host-web/` |
| 日志 | 当前工作目录下的 `Logs/logs_yyyyMMdd.txt` |

`ConnectionStrings:Default` 在初始化时按平台数据目录覆盖。前端解包目录每次启动重新生成。更多端口、生命周期与排错说明见 [PicoServer 集成文档](./CrossWebHost/doc/PicoServer.md)。

## 作为 .NET 项目模板使用

### 从源码安装

在本仓库根目录执行：

```powershell
dotnet new install .
```

然后切换到希望存放新项目的目录：

```powershell
dotnet new crossweb -n MyProduct
Set-Location .\MyProduct
.\init.ps1
```

生成项目包含 `MyProductHost/` 和 `MyProductFrontend/`。模板会替换项目名、命名空间、npm 包名、前端就绪消息与资源路径，并注入新项目专用 README 和初始任务状态；源仓库阶段历史、依赖目录与构建产物被排除。

Windows 也可通过根目录 [安装模板.bat](./安装模板.bat) 安装。该脚本执行 `dotnet new install ./ --force`，请从仓库根目录运行。

卸载源码模板时，在原安装目录执行：

```powershell
dotnet new uninstall .
```

### 打包与安装 NuGet 模板包

在仓库根目录执行：

```powershell
dotnet pack .\template-pack\CrossWeb.Templates.csproj `
  -c Release `
  -o .\CrossWebHost\artifacts\packages

dotnet new install `
  .\CrossWebHost\artifacts\packages\CrossWeb.Templates.0.1.0.nupkg
```

安装后同样使用 `dotnet new crossweb -n MyProduct` 创建项目。当前包版本为 `0.1.0`，输出目录已被 Git 忽略；已有验证覆盖本地打包和安装，尚未发布到公共 NuGet 源。

卸载包模板：

```powershell
dotnet new uninstall CrossWeb.Templates
```

生成项目的根 README 来自 `.template-content/README.md`，与本仓库的说明文档独立维护。

## 项目状态与文档

第一阶段完成 React / WebView / PicoServer 的最小闭环；第二阶段完成重命名、.NET 模板和本地模板包。后续维护包含 Android 回环 HTTP 配置与任务归档机制。

| 入口 | 内容 |
|---|---|
| [前端说明](./CrossWebFrontend/README.md) | 前端工具链与开发命令 |
| [宿主开发规则](./CrossWebHost/AGENTS.md) | 宿主文件放置与 ABP 分层规则 |
| [PicoServer 集成](./CrossWebHost/doc/PicoServer.md) | API、端口、权限与生命周期 |
| [文档索引](./docs/README.md) | 阶段文档导航 |
| [已完成工作](./docs/completed/README.md) | 已完成任务与验收证据 |
| [活动任务](./feature_list.json) | 未完成任务、依赖与完成条件 |
| [当前进度](./progress.md) | 当前状态、阻塞项与下一步 |
| [会话交接](./session-handoff.md) | 恢复步骤与当前目标 |

新工作遵循根目录 [AGENTS.md](./AGENTS.md)：先运行基线验证，同一时间只处理一个任务；完成后将证据归档，并从活动状态文件移除完成历史。

参考：[PicoServer 文档](https://picoserver.cn/guide.html)、[.NET 自定义模板](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)。
