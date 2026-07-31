# maintenance-002：建立已完成任务归档机制

- 状态：已完成
- 完成日期：2026-07-31

## 完成内容

- 建立 `docs/completed/` 统一入口和机器可读的已完成特性索引。
- 将阶段任务与 `maintenance-001` 从活动状态文件迁移为归档引用。
- 精简 `feature_list.json`、`progress.md` 和 `session-handoff.md`。
- 在 `AGENTS.md` 中明确完成后的归档、引用和清理流程。

## 验证

- 修改前运行 `.\init.ps1`：通过；前端依赖审计、lint、`build:host` 和 Desktop
  CrossWebHost 构建均成功，构建保留既有警告。
- 修改后运行 `.\init.ps1`：通过；npm 审计 0 个漏洞，前端 lint、
  `build:host` 和 Desktop CrossWebHost 构建均成功，Host 为 0 个错误、72 个
  既有警告。
- 两个 `feature_list.json` 均可解析，`git diff --check` 通过，本次新增和修改
  文档的本地链接检查通过。
