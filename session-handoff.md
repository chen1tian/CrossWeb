# 会话交接

## 当前目标

- 第一阶段 MVP 已完成并通过最终验收。
- 下一阶段来自 README：先把项目重命名为 CrossWeb / CrossWebHost /
  CrossWebFrontend，再转换为 .NET 模板。
- 分支：`main`。
- 第一阶段里程碑提交信息：`完成第一阶段 MVP`。

## 已交付能力

- `Frontend/`：Vite + React + TypeScript + Oxlint。
- `npm run build:host`：生产构建、复制 Host Web 资源、生成资源清单。
- PicoServer：`http://+:8090/`、CORS、静态资源、`/api/ping`、
  `/api/host/urls`、启动健康检查和退出释放。
- Windows Desktop：`requireAdministrator` UAC 清单。
- Avalonia NativeWebView：服务就绪后导航、加载/错误状态、React 就绪握手。
- React MVP：发送 ping、显示 pong、显示实际可访问 URL。
- 中文 harness：任务依赖、完成证据、统一验证入口和恢复说明。

## 最终验证

| 检查 | 结果 |
|---|---|
| `.\init.ps1` | 通过 |
| npm audit | 0 个漏洞 |
| Oxlint | 通过 |
| TypeScript + Vite + `build:host` | 通过 |
| Desktop Host 构建 | 0 个错误、100 个既有警告 |
| UAC 管理员启动 | 通过 |
| `GET /api/ping` | `200 pong` |
| `GET /api/host/urls` | 返回回环和局域网地址 |
| 静态首页、JS、CSS | 200，内容类型正确 |
| 浏览器控制台与页面错误 | 无 |
| 浏览器 ping 交互 | `等待测试` → `pong` |
| Avalonia WebView | React、URL、`pong` 均已截图确认 |
| Host 退出后端口释放 | 通过 |

## 未解决风险

- 第一阶段无阻塞项。
- 既有 NuGet/编译警告和传递依赖安全公告需在后续依赖治理任务处理。
- 强通配符监听的外部可访问性受防火墙和网络路由影响。
- Android、iOS、Browser 尚未做端到端运行验收。
- 结构验证器依赖英文关键词，对中文 harness 给出 56/100；这是关键词匹配
  限制，不代表五个 harness 子系统缺失。

## 下一会话启动步骤

1. 阅读 `README.md`、`AGENTS.md`、`feature_list.json` 和 `progress.md`。
2. 修改 Host 前阅读 `Host/AGENTS.md`。
3. Windows 运行 `.\init.ps1`，POSIX shell 运行 `./init.sh`。
4. 确认第一阶段提交存在且工作树干净。
5. 为第二阶段创建新的依赖有序任务，先处理项目重命名。
