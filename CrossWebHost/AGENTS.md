# CrossWebHost 开发提示

## 项目结构

**前端 (Avalonia UI)**

|项目	|说明|
|---	|---|
|CrossWebHost|	核心共享项目，包含视图和视图模型|
|CrossWebHost.Desktop|	桌面端启动项目 (Windows/macOS/Linux)|
|CrossWebHost.Android|	Android 启动项目|
|CrossWebHost.iOS|	iOS 启动项目|
|CrossWebHost.Browser|	WebAssembly 浏览器端项目|

**后端 (ABP Framework)**

|项目	|说明|
|---	|---|
|CrossWebHost.Domain|	领域层 - 实体、领域服务|
|CrossWebHost.Application.Contracts|	应用层契约 - DTO、接口定义|
|CrossWebHost.Application|	应用层实现|
|CrossWebHost.EntityFramework|	EF Core 基础设施|
|CrossWebHost.EntityFramework.MySql|	MySQL 支持|
|CrossWebHost.EntityFramework.SqlServer|	SQL Server 支持|

**目录结构**
```
CrossWebHost/                                      # 根目录
├── AGENTS.md                                      # AI 代理说明
├── doc/                                           # 文档目录
└── src/                                           # 源代码目录
    ├── Directory.Packages.props                   # NuGet 包版本集中管理
    ├── CrossWebHost/                              # 核心共享项目 (Avalonia UI)
    ├── CrossWebHost.Desktop/                      # 桌面端启动项目
    ├── CrossWebHost.Android/                      # Android 启动项目
    ├── CrossWebHost.iOS/                          # iOS 启动项目
    ├── CrossWebHost.Browser/                      # Browser 启动项目
    └── abp/                                       # ABP 后端模块
        ├── CrossWebHost.Domain/                   # 领域层
        │   ├── ApiResults/                        # API 返回结果定义
        │   ├── Config/                            # 配置
        │   ├── Extensions/                        # 扩展方法
        │   └── Localization/                      # 本地化资源
        ├── CrossWebHost.Application.Contracts/   # 应用层契约
        ├── CrossWebHost.Application/              # 应用层实现
        ├── CrossWebHost.EntityFramework/          # EF Core 基础设施
        │   ├── CrossWebHostDbContext.cs
        │   └── Migrations/                        # 数据库迁移
        ├── CrossWebHost.EntityFramework.MySql/    # MySQL 支持
        └── CrossWebHost.EntityFramework.SqlServer/ # SQL Server 支持
```

## 文件架构规范
- MVVM View文件: MVVM View文件统一放在 `src\CrossWebHost\Views` 目录下，如果模块包含多个View文件，那么建立模块文件夹，将文件统一存放在文件夹中。例如：采集功能视图为CollectView，此文件放在View目录下，因为它是功能根文件，视图上还包括采集计划(CollectPlan)，采集计划列表项(CollectPlanItem)等等文件，那么新建目录`Collects`(复数形式)，将CollectPlan、CollectPlanItem放在此目录下
- MVVM ViewModel文件：ViewModel文件统一存放在`src\CrossWebHost\ViewModels`目录下，与View文件路径一一对应

## 前端开发提示

- Material Icons 图标库用法: [`doc\material-icons.mdc`](./doc/material-icons.mdc)
- 控件优先使用Material Avalonia库: [`doc\MaterialAvalonia.mdc`](./doc/MaterialAvalonia.mdc)

## 后端开发提示

- 依赖注入: [`doc\ABPRules\di.mdc`](./doc/ABPRules/di.mdc)
- 创建实体类: [`doc\ABPRules\task-create-entity.mdc`](./doc/ABPRules/task-create-entity.mdc)
- 创建Crud应用服务: [`doc\ABPRules\task-create-crud-service.mdc`](./doc/ABPRules/task-create-crud-service.mdc)
- 数据库迁移：[`doc\ABPRules\task-ef.mdc`](./doc/ABPRules/task-ef.mdc)
- 保存/读取配置: [`doc\ABPRules\task-save-read-setting.mdc`](./doc/ABPRules/task-save-read-setting.mdc)
