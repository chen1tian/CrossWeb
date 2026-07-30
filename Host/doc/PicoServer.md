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

### Windows URL ACL

PicoServer 1.7.6 的 `StartServer(int)` 使用 `http://+:端口/`。Windows
普通用户首次运行前，管理员需要为实际运行账户预留该 URL。请在管理员
PowerShell 中运行：

```powershell
$account = "$env:USERDOMAIN\$env:USERNAME"
netsh http add urlacl url=http://+:8090/ user=$account
```

查看预留：

```powershell
netsh http show urlacl url=http://+:8090/
```

不再使用该端口时，可在管理员 PowerShell 中删除预留：

```powershell
netsh http delete urlacl url=http://+:8090/
```

## 生命周期

`PicoServerService` 由 ABP 依赖注入容器作为单例管理：

1. ABP 初始化和数据库迁移完成后启动服务器。
2. Desktop 应用退出时停止服务器。
3. 进程退出事件提供额外的停止保障。
4. `StartAsync()` 和 `Stop()` 都是幂等操作。
5. 启动后最多等待 3 秒确认监听成功；URL ACL 缺失或端口被占用时，应用记录
   明确错误并停止初始化。

## 验证

运行 Desktop Host 后执行：

```powershell
Invoke-WebRequest http://127.0.0.1:8090/api/ping
```

或：

```bash
curl http://127.0.0.1:8090/api/ping
```

响应正文应为 `pong`。
