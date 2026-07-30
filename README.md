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
npm run build:host
```

详细说明参见 [`Frontend/README.md`](./Frontend/README.md)。根目录的
`init.ps1` 或 `init.sh` 会同时验证 Host 和前端。

## 构建与运行

在 Windows 仓库根目录执行：

```powershell
.\init.ps1
.\Host\src\Host.Desktop\bin\Debug\net10.0\Host.Desktop.exe
```

启动时需要允许 Windows UAC 提升。Host 启动 PicoServer、解包内嵌的 React
生产资源，通过健康检查后再让 Avalonia WebView 导航到本地页面。界面底部会
显示当前可访问的回环和局域网 URL。

## 本地 API

Host 通过 PicoServer 监听 `http://+:8090/`，本机客户端可通过
`http://127.0.0.1:8090` 访问 API。

```text
GET http://127.0.0.1:8090/api/ping
→ pong
```

在 WebView 页面中点击“发送 ping”，`Host API` 状态应变为 `pong`。也可以在
Host 运行时执行：

```powershell
Invoke-WebRequest http://127.0.0.1:8090/api/ping
Invoke-RestMethod http://127.0.0.1:8090/api/host/urls
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

### 第二阶段
1. 修改名称：修改根目录名称为：CrossWeb，修改后端为 CrossWebHost，前端为 CrossWebFrontend。
2. 将此项目转换为.net模板，方便以此为基础创建新的跨平台Web融合项目。   
参考：
   - https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates
   - https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template
