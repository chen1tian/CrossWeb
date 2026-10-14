# 会话交接

## 当前目标

`maintenance-003`：根据当前实现重写根 README。文档已经重写并完成静态检查；
任务处于 `blocked`，待统一基线通过后完成归档。

- 当前分支：`main`
- 当前阻塞项：本机 npm 在 `npm ci` 时出现
  `Class extends value undefined is not a constructor or null`；详情和验证结果见
  `progress.md`。

## 恢复步骤

1. 阅读 `README.md`、`AGENTS.md`、`feature_list.json` 和 `progress.md`。
2. Windows 运行 `.\init.ps1`，POSIX shell 运行 `./init.sh`。
3. 优先恢复 npm 环境并复验当前任务；验证通过后归档，再选择新工作。
4. 修改 `CrossWebHost/` 前阅读 `CrossWebHost/AGENTS.md`。
