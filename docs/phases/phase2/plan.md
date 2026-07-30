# 第二阶段实施计划

本文件为阶段开始前确定的原始实施计划，阶段完成后随归档保留。

## 阶段目标

第二阶段把第一阶段完成的 AvWeb 原型重命名为 CrossWeb，并将其制作成可以通过
`dotnet new` 安装、实例化和验证的跨平台 Web 融合项目模板。

计划包含五个线性任务，同一时间只允许一个任务处于 `in-progress`：

```text
phase2-001 → phase2-002 → phase2-003 → phase2-004 → phase2-005
```

## 命名约定

| 当前名称 | 目标名称 |
|---|---|
| 仓库外层目录 `AvWeb` | `CrossWeb` |
| `Host/` | `CrossWebHost/` |
| `Host.sln` | `CrossWebHost.sln` |
| `Host.*` 项目和命名空间 | `CrossWebHost.*` |
| `Frontend/` | `CrossWebFrontend/` |
| `avweb-frontend` | `crossweb-frontend` |
| `avweb.frontendReady` | `crossweb.frontendReady` |
| `avweb-host-web-manifest.json` | `crossweb-host-web-manifest.json` |

重命名只作用于项目标识和产品标识。不得误改 .NET 自身的 `IHost`、
`HostApplicationBuilder` 等 API，也不强制修改具有业务含义的
`WebHostView`。第一阶段归档保留原名称，作为历史证据。

Git 不跟踪仓库外层目录名，因此本地目录 `AvWeb` 改为 `CrossWeb` 是阶段关闭时
执行的工作区操作，不属于可提交差异。模板必须通过 `preferNameDirectory`
保证新实例使用用户指定的根目录名。

## phase2-001：完成 CrossWeb 原型重命名

原子化更新前后端目录、项目标识和互相引用，任务结束前不允许把中间破损状态
标记为完成。

完成条件：

- `Host/` 和 `Frontend/` 分别改为 `CrossWebHost/` 和
  `CrossWebFrontend/`。
- 后端解决方案、项目、项目引用、命名空间、XAML 类型、Avalonia 资源 URI、
  ABP 模块、EF Migration 和本地化资源保持一致。
- 前端包名、页面标题、WebView 消息协议和 Host 资源清单名称完成更新。
- 初始化脚本、忽略规则、AGENTS 和当前技术文档使用新路径。
- 除 `docs/phases/phase1/` 历史归档外，不残留应被替换的 AvWeb 产品标识或
  旧目录路径。
- `.\init.ps1` 通过，Desktop Host、WebView、URL 列表和 ping/pong 运行
  验收通过。

## phase2-002：建立 .NET 项目模板

在可运行的 CrossWeb 原型上增加 `.template.config/template.json`。

模板约定：

- `shortName`：`crossweb`
- `sourceName`：`CrossWeb`
- `preferNameDirectory`：`true`
- 模板类型：`project`
- 默认结构：`CrossWebHost/` 和 `CrossWebFrontend/`

完成条件：

- 模板元数据符合当前 .NET SDK 的模板规范。
- 正确替换 `CrossWeb`、`crossweb` 等不同大小写形式。
- 排除 `.git`、`.vs`、`bin`、`obj`、`node_modules`、`dist`、日志和临时
  验证产物。
- 不向新项目复制阶段归档、当前阶段状态和源仓库开发历史。
- 新项目包含干净的使用文档、初始化脚本和初始 harness 状态。
- 从本地目录安装模板并执行 `dotnet new crossweb` 成功。

## phase2-003：验证模板生成实例

在仓库外的临时目录生成不同名称的项目，例如：

```powershell
dotnet new crossweb -n SampleFusion
```

完成条件：

- 生成 `SampleFusionHost/` 和 `SampleFusionFrontend/`。
- 解决方案、项目、命名空间、npm 包名、消息协议和资源路径均完成名称替换。
- 新项目不包含源仓库阶段历史或构建产物。
- 在生成项目中运行 `.\init.ps1` 成功。
- 生成项目的 Desktop Host、WebView、URL 列表和 ping/pong 验收通过。
- 测试模板被卸载，临时目录得到清理。

## phase2-004：提供模板包和使用文档

提供可搬运的 NuGet 模板包，而不要求使用者访问源仓库目录。

完成条件：

- 模板包项目设置 `PackageType=Template`、唯一 `PackageId`、版本和说明。
- `dotnet pack` 成功生成 `.nupkg`，包内模板位于 `content`。
- 可以从 `.nupkg` 安装模板并创建自定义名称的项目。
- README 记录模板安装、创建、验证和卸载命令。
- `.nupkg` 和打包输出不提交 Git。

## phase2-005：关闭第二阶段

完成条件：

- CrossWeb 原型基线验证通过。
- 从模板包生成的自定义名称项目完成构建和端到端运行验收。
- README 标记第二阶段完成。
- 实现过程、验证证据、决策和风险归档到 `docs/phases/phase2/`。
- `feature_list.json`、`progress.md` 和 `session-handoff.md` 按归档规则精简。
- 创建第二阶段里程碑提交。
- 里程碑提交完成后，在不影响工作树的时机将本地仓库外层目录改为
  `CrossWeb`。

## 阶段验证门槛

第二阶段不能只验证模板配置文件可以加载。阶段验收至少覆盖：

1. 原型仓库运行 `.\init.ps1`。
2. 从本地目录安装模板并生成自定义名称实例。
3. 在实例中运行 `.\init.ps1`。
4. 从 `.nupkg` 安装模板并重复实例化。
5. 在生成实例中验证 Desktop Host、WebView、可访问 URL 和 ping/pong。
6. 扫描生成实例，确认没有旧产品标识、阶段历史和构建产物。

## 范围外事项

- 清理第一阶段遗留的 NuGet、XML 注释、环境路径和依赖安全警告。
- 增加新的产品功能或改变 PicoServer 端口策略。
- 发布模板包到公共 NuGet 源。
- 扩展 Android、iOS 或 Browser 的端到端运行范围。

## 参考资料

- [.NET 自定义模板](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)
- [创建 dotnet new 项目模板](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template)
