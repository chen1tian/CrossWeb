# CrossWeb

CrossWeb 是一个将 React Web UI、Avalonia WebView 与 PicoServer 本地 API 融合在
同一桌面应用中的跨平台项目起点。

## 项目结构

- `CrossWebFrontend/`：Vite + React + TypeScript 前端。
- `CrossWebHost/`：Avalonia 宿主与 PicoServer/ABP 服务。
- `AGENTS.md`、`feature_list.json`、`progress.md` 和
  `session-handoff.md`：供编码智能体持续工作的初始 harness。

## 初始化

Windows：

```powershell
.\init.ps1
```

POSIX shell：

```bash
./init.sh
```

初始化会安装锁定的前端依赖、执行 lint 和生产构建，并构建 Desktop Host。

## 运行

Windows 完成初始化后，以管理员身份启动：

```powershell
Start-Process `
  .\CrossWebHost\src\CrossWebHost.Desktop\bin\Debug\net10.0\CrossWebHost.Desktop.exe `
  -Verb RunAs
```

应用通过 PicoServer 监听 `http://+:8090/`。界面会显示可访问的回环与局域网
URL；点击“发送 ping”后应显示 `pong`。

## 开发入口

- 前端说明：[`CrossWebFrontend/README.md`](CrossWebFrontend/README.md)
- Host 规则：[`CrossWebHost/AGENTS.md`](CrossWebHost/AGENTS.md)
- PicoServer 说明：
  [`CrossWebHost/doc/PicoServer.md`](CrossWebHost/doc/PicoServer.md)

开始修改前先阅读根目录 `AGENTS.md`，运行初始化脚本，并在
`feature_list.json` 中建立任务；同一时间只选择一个依赖已完成的任务。
