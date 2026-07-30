# 会话交接

## 当前目标

第二阶段已经完成，当前正在处理 `maintenance-001`：同步 Android 回环 HTTP 配置。

- 当前分支：`main`
- 当前基线提交：第二阶段里程碑提交（`完成第二阶段 .NET 模板`）
- 当前阻塞项：完成代码与模板验证后，需要 Android 真机复验页面加载。
- 第二阶段归档：[docs/phases/phase2/README.md](docs/phases/phase2/README.md)
- 第一阶段归档：[docs/phases/phase1/README.md](docs/phases/phase1/README.md)

## 恢复步骤

1. 阅读 `README.md`、`AGENTS.md`、`feature_list.json` 和 `progress.md`。
2. Windows 运行 `.\init.ps1`，POSIX shell 运行 `./init.sh`。
3. 新工作开始前更新 `feature_list.json`，并保持最多一个 `in-progress` 任务。
4. 修改 `CrossWebHost/` 前阅读 `CrossWebHost/AGENTS.md`。
5. 不修改第一阶段归档中的历史名称。
