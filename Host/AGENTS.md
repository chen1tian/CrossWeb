# Host开发提示

## 项目结构

**前端 (Avalonia UI)**

|项目	|说明|
|---	|---|
|Host|	核心共享项目，包含视图和视图模型|
|Host.Desktop|	桌面端启动项目 (Windows/macOS/Linux)|
|Host.Android|	Android 启动项目|
|Host.iOS|	iOS 启动项目|
|Host.Browser|	WebAssembly 浏览器端项目|

**后端 (ABP Framework)**

|项目	|说明|
|---	|---|
|Host.Domain|	领域层 - 实体、领域服务|
|Host.Application.Contracts|	应用层契约 - DTO、接口定义|
|Host.Application|	应用层实现|
|Host.EntityFramework|	EF Core 基础设施|
|Host.EntityFramework.MySql|	MySQL 支持|
|Host.EntityFramework.SqlServer|	SQL Server 支持|

**目录结构**
```
Host/                                    # 根目录
├── AGENTS.md                               # AI代理README
├── doc/                                    # 文档目录
└── src/                                    # 源代码目录
    ├── Directory.Packages.props            # NuGet 包版本集中管理    │
    ├── Host/                            # 核心共享项目 (Avalonia UI)    
    ├── Host.Desktop/                    # 桌面端启动项目 (Windows/macOS/Linux)    
    ├── Host.Android/                    # Android 启动项目    
    ├── Host.iOS/                        # iOS 启动项目    
    ├── Host.Browser/                    # Web/Browser 启动项目 (WebAssembly)    
    └── abp/                                # ABP 框架后端模块
        ├── Host.Domain/                 # 领域层
        │   ├── ApiResults/                 # API 返回结果定义
        │   ├── Config/                     # 配置
        │   ├── Extensions/                 # 扩展方法
        │   ├── Domains/                    # 领域实体
        │   └── Localization/               # 本地化资源        │
        ├── Host.Application.Contracts/  # 应用层契约 (DTO、接口)        
        ├── Host.Application/            # 应用层实现        
        ├── Host.EntityFramework/        # EF Core 基础设施        
        │   ├── HostDbContext.cs        
        │   ├── Migrations/                 # 数据库迁移
        ├── Host.EntityFramework.MySql/      # MySQL 数据库支持        │
        └── Host.EntityFramework.SqlServer/  # SQL Server 数据库支持
```

## 文件架构规范
- MVVM View文件: MVVM View文件统一放在 `src\Host\Views` 目录下，如果模块包含多个View文件，那么建立模块文件夹，将文件统一存放在文件夹中。例如：采集功能视图为CollectView，此文件放在View目录下，因为它是功能根文件，视图上还包括采集计划(CollectPlan)，采集计划列表项(CollectPlanItem)等等文件，那么新建目录`Collects`(复数形式)，将CollectPlan、CollectPlanItem放在此目录下
- MVVM ViewModel文件：ViewModel文件统一存放在`src\Host\ViewModels`目录下，与View文件路径一一对应

## 前端开发提示

- Material Icons 图标库用法: [`doc\material-icons.mdc`](./doc/material-icons.mdc)
- 控件优先使用Material Avalonia库: [`doc\MaterialAvalonia.mdc`](./doc/MaterialAvalonia.mdc)

## 后端开发提示

- 依赖注入: [`doc\ABPRules\di.mdc`](./doc/ABPRules/di.mdc)
- 创建实体类: [`doc\ABPRules\task-create-entity.mdc`](./doc/ABPRules/task-create-entity.mdc)
- 创建Crud应用服务: [`doc\ABPRules\task-create-crud-service.mdc`](./doc/ABPRules/task-create-crud-service.mdc)
- 数据库迁移：[`doc\ABPRules\task-ef.mdc`](./doc/ABPRules/task-ef.mdc)
- 保存/读取配置: [`doc\ABPRules\task-save-read-setting.mdc`](./doc/ABPRules/task-save-read-setting.mdc)