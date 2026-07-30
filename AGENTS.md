# AvWeb 智能体指南

本仓库用于构建 `README.md` 描述的跨平台应用。产品分为三个层次：

- React Web UI：位于 `Frontend/` 的 Vite + React + TypeScript 项目。
- Avalonia 宿主外壳：通过 WebView 加载 Web UI。
- PicoServer/ABP 服务：向 Web UI 提供本地 API。

`README.md` 是产品范围的权威来源。修改 `Host/` 下的内容时，还必须遵守
`Host/AGENTS.md` 中的文件放置和框架规则。

## 工作语言

- 与用户沟通、说明实现、汇报进度和总结结果时，统一使用中文。
- 新增或更新项目文档、进度记录、交接记录和提交信息时，统一使用中文。
- 代码标识符、文件名、命令、JSON 字段、状态枚举、第三方 API 名称和原始
  错误信息保持其技术原文，必要时在旁边用中文解释。
- 只有用户明确要求使用其他语言时，才临时切换语言。

## 启动流程

开始编辑前：

1. 确认当前工作目录是仓库根目录。
2. 阅读 `README.md`、本文件、`feature_list.json` 和 `progress.md`。
3. 接续之前的工作时，阅读 `session-handoff.md`。
4. 只有需要追溯已结束阶段时，才沿状态文件中的路径阅读 `docs/phases/` 归档。
5. 修改 `Host/` 前，阅读 `Host/AGENTS.md` 及其中链接的相关文档。
6. Windows 运行 `.\init.ps1`，POSIX shell 运行 `./init.sh`。
7. 只选择一个依赖已经完成的未完成任务。

如果基线验证失败，先把失败信息记录到 `progress.md`，再修改产品代码。
不得为了处理无关警告或缺陷而静默扩大当前任务范围。

## 工作规则

- **一次只做一个任务：** 同一时间只能有一个 `in-progress` 任务。
- 明确区分 React UI、Avalonia 外壳和 ABP/PicoServer API 的职责。
- 不要编辑生成的 `bin/` 或 `obj/` 内容。
- 修改范围不得超出当前任务的完成条件。
- 破坏性操作、依赖替换和架构变更必须获得用户明确授权。
- 没有记录验证证据时，不得把任务标记为 `done`。
- 会话结束前更新 `progress.md` 和 `feature_list.json`。

## 范围边界

- 只实现当前任务及其完成条件。
- 当前产品阶段和范围以 `README.md` 及 `feature_list.json` 为准。
- 无关警告清理、大范围重构、额外产品功能、部署和后续平台工作均不在当前
  范围内，除非用户明确加入。

## 必需状态文件

- `feature_list.json`：当前任务依赖、状态、完成条件和归档引用。
- `progress.md`：当前阶段、当前任务、阻塞项和下一步。
- `session-handoff.md`：恢复步骤、当前目标和归档入口。
- `init.ps1` / `init.sh`：统一的基线验证入口。

允许的任务状态为 `not-started`、`in-progress`、`blocked` 和 `done`。
最多只能有一个任务处于 `in-progress`。

## 完成定义

任务只有同时满足以下条件才能标记为 `done`：

1. `feature_list.json` 中该任务的全部完成条件均已满足。
2. 相关构建、测试、lint 和类型检查命令均已成功运行。
3. 无法自动验证的用户可见行为已经完成人工验证。
4. 命令和结果已在进行中文件中临时记录，或已写入对应阶段归档。
5. 文档和会话状态已经反映实际实现。

阶段关闭前，还必须完成 `README.md` 为该阶段定义的端到端验收，并将过程资料
归档。

## 验证

根据平台运行统一入口：

```powershell
.\init.ps1
```

```bash
./init.sh
```

当前基线会构建 Desktop Host 并验证前端：

```text
dotnet build Host/src/Host.Desktop/Host.Desktop.csproj --nologo
npm --prefix Frontend ci
npm --prefix Frontend run lint
npm --prefix Frontend run build:host
```

## 阶段归档规则

- 阶段完成并通过验收后，将实现过程、验证证据、关键决策和遗留风险归档到
  `docs/phases/<phase-id>/`。
- 每个阶段必须提供 `docs/phases/<phase-id>/README.md` 作为单一归档入口；
  `docs/README.md` 维护阶段索引。
- `feature_list.json` 中已完成任务只保留状态、完成条件和归档路径，不重复保存
  长篇证据。
- `progress.md` 只保留当前阶段、当前任务、当前阻塞项、下一步和已完成阶段的
  归档链接。
- `session-handoff.md` 只保留恢复步骤、当前目标、当前阻塞项和归档链接。
- 同一段阶段历史不得复制到多个进行中文件；历史内容只在对应归档中维护。
- 阶段进行期间允许在状态文件中临时记录必要信息，关闭阶段时必须完成归档和
  精简。

## 会话结束流程

1. 重新运行当前任务相关的验证。
2. 更新 `feature_list.json` 中的任务状态和必要证据。
3. 在 `progress.md` 中记录当前状态、阻塞项和准确的下一步。
4. 对于中断或跨会话工作，更新 `session-handoff.md`。
5. 阶段全部任务为 `done` 且验收通过后，按阶段归档规则整理过程信息。
6. 创建阶段里程碑提交前，确认归档和进行中文件均与实际状态一致。
7. 未达到提交条件时，保持工作树状态清晰并汇报未提交修改。

当下一会话能够从干净检出开始，按启动流程运行平台初始化脚本、确定唯一当前
任务并复现基线结果时，仓库才算具备可恢复性。
