# CrossWeb

## 说明
这是一个新建的跨平台项目，技术架构如下：
1. 前端：`CrossWebFrontend/` 下的 Vite + React + TypeScript 项目
2. 后端（CrossWebHost）：跨平台Avalonia项目

后端Host使用WebView承载前端的网页，使用PicoServer作为本地Web服务器，前端React项目通过PicoServer访问后端Host提供的API接口。
PicoServer(https://picoserver.cn/guide.html)
PcoServer 实时通讯 文档: https://picoserver.cn/realtime.html

## 前端开发

前端使用 Node.js、npm、Vite、React 和 TypeScript。在 `CrossWebFrontend/` 目录运行：

```bash
npm ci
npm run dev
npm run lint
npm run typecheck
npm run build
npm run build:host
```

详细说明参见 [`CrossWebFrontend/README.md`](./CrossWebFrontend/README.md)。根目录的
`init.ps1` 或 `init.sh` 会同时验证 CrossWebHost 和前端。

## 构建与运行

在 Windows 仓库根目录执行：

```powershell
.\init.ps1
.\CrossWebHost\src\CrossWebHost.Desktop\bin\Debug\net10.0\CrossWebHost.Desktop.exe
```

启动时需要允许 Windows UAC 提升。CrossWebHost 启动 PicoServer、解包内嵌的 React
生产资源，通过健康检查后再让 Avalonia WebView 导航到本地页面。界面底部会
显示当前可访问的回环和局域网 URL。

## 本地 API

CrossWebHost 通过 PicoServer 监听 `http://+:8090/`，本机客户端可通过
`http://127.0.0.1:8090` 访问 API。

```text
GET http://127.0.0.1:8090/api/ping
→ pong
```

在 WebView 页面中点击“发送 ping”，`CrossWebHost API` 状态应变为 `pong`。也可以在
CrossWebHost 运行时执行：

```powershell
Invoke-WebRequest http://127.0.0.1:8090/api/ping
Invoke-RestMethod http://127.0.0.1:8090/api/host/urls
```

生命周期、端口策略和验证方式参见
[`CrossWebHost/doc/PicoServer.md`](./CrossWebHost/doc/PicoServer.md)。

## 项目模板

从源码目录安装并创建项目：

```powershell
dotnet new install .
dotnet new crossweb -n MyProduct
dotnet new uninstall .
```

生成可搬运的 NuGet 模板包：

```powershell
dotnet pack .\template-pack\CrossWeb.Templates.csproj `
  -c Release `
  -o .\CrossWebHost\artifacts\packages
```

从模板包安装、创建、验证和卸载：

```powershell
dotnet new install `
  .\CrossWebHost\artifacts\packages\CrossWeb.Templates.0.1.0.nupkg
dotnet new crossweb -n MyProduct
Set-Location .\MyProduct
.\init.ps1
Set-Location ..
dotnet new uninstall CrossWeb.Templates
```

模板默认创建 `MyProductHost/` 与 `MyProductFrontend/`，并包含中文 README、
初始化脚本以及干净的初始 harness 状态。打包输出位于已忽略的 `artifacts/`
目录，不提交 Git。

## 需求

### 第一阶段

状态：已完成。过程与验收资料见
[`docs/phases/phase1/README.md`](./docs/phases/phase1/README.md)。

1. 创建前端文件
2. 修改后端Host:
   1. 增加PicoServer支持
   2. 修改为WebView承载前端网页
3. 完成最小可用产品（MVP）：
   1. 前端React项目可以通过PicoServer访问后端Host提供的API接口
   2. 后端Host可以通过WebView显示前端网页
   3. 一次请求交互：ping，pong，前端发送ping请求，后端返回pong响应

### 第二阶段

状态：已完成。实施过程、验证证据、关键决策和遗留风险见
[`docs/phases/phase2/README.md`](./docs/phases/phase2/README.md)。

1. 将原型重命名为 CrossWeb：
   1. 仓库外层目录：`CrossWeb`
   2. 后端：`CrossWebHost`
   3. 前端：`CrossWebFrontend`
2. 将 CrossWeb 转换为可通过 `dotnet new` 安装和实例化的 .NET 项目模板。
3. 使用不同项目名生成独立实例，并验证构建、WebView、可访问 URL 和
   ping/pong。
4. 提供可安装的 NuGet 模板包和中文使用文档。

参考：

- https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates
- https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template
