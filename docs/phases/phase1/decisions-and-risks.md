# 第一阶段决策与遗留风险

## 已确认决策

- React 负责 Web UI，Avalonia 负责原生宿主外壳，ABP/PicoServer 负责服务和
  API。
- PicoServer 固定使用 8090 端口和 `http://+:8090/` 通配符监听。
- Windows Desktop 通过 UAC 管理员启动满足通配符监听权限要求。
- 前端生产资源属于生成物，不提交到 Git，由 `build:host` 重建。
- WebView 在健康检查通过前保持 `about:blank`，避免提前加载失败。
- 第一阶段五个任务合并为一个里程碑提交。
- 用户沟通、项目文档、进度、交接和提交信息统一使用中文。

## 遗留风险

- Host 仍有既有的 NuGet、编译和传递依赖安全警告；它们未阻塞第一阶段，
  应由后续依赖治理任务单独处理。
- `http://+:8090/` 监听所有接口，其他设备能否访问还取决于 Windows 防火墙
  和网络路由。
- 第一阶段只完成 Windows Desktop 端到端验收，Android、iOS 和 Browser
  不在该阶段的验收范围内。
- `harness-creator` 的结构验证器依赖英文关键词，中文 harness 的历史评分为
  56/100；这是关键词匹配限制，并不表示状态、验证、范围或交接机制缺失。

## 相关提交

| 提交 | 说明 |
|---|---|
| `381d40b` | 第一阶段 MVP 里程碑 |
| `50e25ec` | PicoServer 与 Host/前端通信集成 |
| `fa8b317` | 后端项目增加 WebView |
