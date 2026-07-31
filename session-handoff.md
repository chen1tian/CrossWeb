# 会话交接

## 当前目标

第二阶段与 `maintenance-001` 模板维护任务均已完成，当前没有进行中任务。

- 当前分支：`main`
- 当前基线提交：`6f0b6ac`（`修正android下无法启动端口的问题`）
- 当前阻塞项：无
- 第二阶段归档：[docs/phases/phase2/README.md](docs/phases/phase2/README.md)
- 第一阶段归档：[docs/phases/phase1/README.md](docs/phases/phase1/README.md)

## 恢复步骤

1. 阅读 `README.md`、`AGENTS.md`、`feature_list.json` 和 `progress.md`。
2. Windows 运行 `.\init.ps1`，POSIX shell 运行 `./init.sh`。
3. 新工作开始前更新 `feature_list.json`，并保持最多一个 `in-progress` 任务。
4. 修改 `CrossWebHost/` 前阅读 `CrossWebHost/AGENTS.md`。
5. 不修改第一阶段归档中的历史名称。
