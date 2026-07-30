# PicoServer 集成

Host 使用 PicoServer 提供本地 HTTP API。

## 地址与端口

- 监听前缀：`http://+:8090/`
- 客户端基址：`http://127.0.0.1:8090`
- 端口：`8090`
- ping 接口：`GET /api/ping`
- 预期响应：`200 OK`，正文为 `pong`
- 响应类型：`text/plain; charset=utf-8`

固定端口便于后续 Avalonia WebView 和 React 使用同一个地址。`+` 是
`HttpListener` 的强通配符，会监听本机所有网络接口；防火墙和部署配置必须
避免把该端口暴露到不受信任的网络。

### Windows 管理员权限

PicoServer 1.7.6 的 `StartServer(int)` 使用 `http://+:端口/`。Windows
Desktop 的应用清单使用 `requireAdministrator`，因此 Windows 启动时会显示
UAC 提示，并以管理员权限运行。管理员令牌允许 PicoServer 注册通配符监听，
不需要用户另外配置 URL ACL。

该设置仅作用于 `Host.Desktop` 的 Windows 可执行文件，不改变 Android、iOS
或 Browser 项目的权限模型。由于整个 Desktop 进程都处于提升权限状态，后续
打开文件、启动外部程序或处理来自 WebView 的命令时必须继续限制输入范围。

## 生命周期

`PicoServerService` 由 ABP 依赖注入容器作为单例管理：

1. ABP 初始化和数据库迁移完成后启动服务器。
2. Desktop 应用退出时停止服务器。
3. 进程退出事件提供额外的停止保障。
4. `StartAsync()` 和 `Stop()` 都是幂等操作。
5. UAC 被拒绝时 Windows 不启动应用；应用启动后最多等待 3 秒确认监听成功，
   端口被占用时记录明确错误并停止初始化。

## 验证

Windows 上请直接启动生成的 `Host.Desktop.exe` 并允许 UAC 提升；不要用
`dotnet Host.Desktop.dll` 验证管理员清单，因为该方式运行的是 `dotnet.exe`
宿主，不使用应用自身的 Windows 清单。

运行 Desktop Host 后执行：

```powershell
Invoke-WebRequest http://127.0.0.1:8090/api/ping
```

或：

```bash
curl http://127.0.0.1:8090/api/ping
```

响应正文应为 `pong`。
