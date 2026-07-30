# 第二阶段实现说明

## CrossWeb 重命名

- 根项目中的 `Frontend/` 与 `Host/` 分别迁移为 `CrossWebFrontend/` 与
  `CrossWebHost/`。
- 解决方案、项目、命名空间、XAML 类型、Avalonia 资源 URI、ABP 模块、
  EF Migration、本地化资源和项目引用统一使用 `CrossWebHost`。
- npm 包名、页面标题、WebView 消息协议和内嵌资源清单分别使用
  `crossweb-frontend`、`CrossWeb`、`crossweb.frontendReady` 和
  `crossweb-host-web-manifest.json`。
- 初始化脚本、智能体规则、技术文档与忽略规则同步到新路径。

## 项目模板

模板入口为 [`.template.config/template.json`](../../../.template.config/template.json)：

- `shortName` 为 `crossweb`，`sourceName` 为 `CrossWeb`。
- `preferNameDirectory` 保证 `dotnet new crossweb -n MyProduct` 创建独立根目录。
- 内置 `name` 符号负责 `CrossWeb` 替换；`casing` 生成器派生全小写与全大写
  名称，用于 npm 包名、消息协议、资源清单和界面标签。
- 第一个 `sources` 映射复制可运行源码，并排除 Git 元数据、依赖、构建输出、
  日志、阶段文档和源仓库状态。
- 第二个 `sources` 映射从 `.template-content/` 注入干净的 README、
  `feature_list.json`、`progress.md` 和 `session-handoff.md`。

生成项目仍包含根 `AGENTS.md`、平台初始化脚本与 Host 层规则，因此可以从首次
会话开始按同一 harness 流程工作，而不会继承 CrossWeb 仓库的阶段历史。

## NuGet 模板包

[`template-pack/CrossWeb.Templates.csproj`](../../../template-pack/CrossWeb.Templates.csproj)
定义：

- `PackageType=Template`
- `PackageId=CrossWeb.Templates`
- `PackageVersion=0.1.0`
- 模板内容位于包内 `content/`

打包项目显式包含模板所需的隐藏文件和 PowerShell 初始化脚本，同时排除阶段
资料、当前状态、构建产物、依赖和打包输出。`.nupkg` 输出到已忽略的
`CrossWebHost/artifacts/packages/`。

## 自定义名称的页面适配

自定义名称可能显著长于 `CrossWeb`。前端标题采用受限的响应式字号与
`text-wrap: balance`：`SampleFusion` 在桌面视口保持单行，更长的
`PackagedFusion` 平衡为两行，避免单个汉字成为孤行。
