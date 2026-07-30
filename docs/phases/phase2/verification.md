# 第二阶段验证记录

验证日期：2026-07-30

平台：Windows，.NET SDK 10.0.302，Node.js/npm，Chromium Playwright 1.49.1

## CrossWeb 原型

最终运行：

```powershell
.\init.ps1
```

结果：

- npm 锁定依赖安装与审计通过，0 个漏洞。
- Oxlint 通过。
- TypeScript 与 Vite `build:host` 通过，3 个前端资源写入正确的新路径。
- Desktop CrossWebHost 构建通过，0 个错误；既有依赖、XML 注释和环境路径
  警告保留在阶段范围外。

运行时验收：

- Desktop 进程名为 `CrossWebHost.Desktop`，窗口标题为 `CrossWeb`。
- `GET /` 返回 200，页面标题和主标题使用 CrossWeb。
- `GET /api/ping` 返回 `pong`。
- `GET /api/host/urls` 返回回环地址与当前局域网地址。
- Playwright 在 1100×720 与 390×844 视口验证 URL 列表和 ping 交互。
- 控制台错误、页面错误、框架错误层与移动端横向溢出均为 0。

## 从源码目录安装模板

模板在隔离模板仓中从仓库目录安装成功，`dotnet new crossweb -n
CleanTemplateSmoke` 创建成功。

生成前扫描结果：

- `CleanTemplateSmokeHost/` 与 `CleanTemplateSmokeFrontend/` 存在。
- 解决方案、npm 包名、消息协议和资源清单均完成名称替换。
- `CrossWeb` 旧模板名称残留为 0。
- `.git`、`.vs`、`bin`、`obj`、`node_modules`、`dist`、`artifacts`、日志和
  阶段文档为 0。
- README、初始化脚本和 4 个 harness 文件完整。

## SampleFusion 端到端验收

在仓库外运行：

```powershell
dotnet new crossweb -n SampleFusion
.\init.ps1
```

结果：

- 生成 `SampleFusionHost/` 与 `SampleFusionFrontend/`。
- 解决方案列出 11 个已重命名项目；37 个 C# 文件使用
  `SampleFusionHost` 命名空间。
- npm 包名为 `samplefusion-frontend`，消息协议为
  `samplefusion.frontendReady`。
- 初始化通过，Desktop 构建 0 个错误。
- Desktop、首页、3 个可访问 URL 和 ping/pong 通过 Playwright 验收。
- 页面与控制台错误为 0。

## NuGet 模板包

运行：

```powershell
dotnet pack .\template-pack\CrossWeb.Templates.csproj `
  -c Release `
  -o .\CrossWebHost\artifacts\packages
```

结果：

- 生成 `CrossWeb.Templates.0.1.0.nupkg`。
- 包含 127 个 `content` 条目，包括 `.template.config/template.json`、
  3 个 `.gitignore` 和 `init.ps1`。
- 包内阶段资料、源仓库状态、依赖和构建目录为 0。
- 从 `.nupkg` 安装 `CrossWeb.Templates@0.1.0` 成功。
- `dotnet new crossweb -n PackagedFusion` 成功，名称替换和干净输出扫描通过。
- `PackagedFusion` 自身的 `.\init.ps1` 通过，Desktop 构建 0 个错误。
- 最终运行验收中页面标题为 `PackagedFusion`，3 个 URL 可见，ping 为
  `pong`，浏览器控制台与页面错误为 0。

所有隔离模板均已卸载，测试进程均已关闭。受本地递归删除策略限制，验证目录
被可恢复地移入已忽略的 `CrossWebHost/artifacts/`，不会进入 Git 或模板包。
