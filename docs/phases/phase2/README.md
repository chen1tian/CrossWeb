# 第二阶段 .NET 模板归档

- 状态：已完成
- 完成日期：2026-07-30
- 产品要求：[README.md](../../../README.md)
- 里程碑提交：本阶段关闭提交（`完成第二阶段 .NET 模板`）

第二阶段将第一阶段原型统一重命名为 CrossWeb，并交付了可从源码目录或 NuGet
模板包安装的 `dotnet new crossweb` 项目模板。模板支持自定义项目名、干净的
初始 harness 状态，以及 Desktop Host、WebView、可访问 URL 和 ping/pong。

## 文档导航

- [原实施计划](./plan.md)：任务拆分、命名约定和阶段门槛。
- [实现说明](./implementation.md)：重命名、模板结构、名称替换与打包方式。
- [验证记录](./verification.md)：原型、目录模板、NuGet 包与生成实例的证据。
- [决策与遗留风险](./decisions-and-risks.md)：阶段决策、范围边界和已知风险。

## 持续维护的技术入口

- [模板配置](../../../.template.config/template.json)
- [模板包项目](../../../template-pack/CrossWeb.Templates.csproj)
- [前端开发说明](../../../CrossWebFrontend/README.md)
- [PicoServer 集成说明](../../../CrossWebHost/doc/PicoServer.md)
- [Host 开发规则](../../../CrossWebHost/AGENTS.md)
