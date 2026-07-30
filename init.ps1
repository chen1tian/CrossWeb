$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Write-Host '=== CrossWeb harness 验证 ==='
dotnet --version

if (-not (Test-Path -LiteralPath 'CrossWebFrontend/package.json')) {
    throw '未找到 CrossWebFrontend/package.json。'
}

Push-Location 'CrossWebFrontend'
try {
    Write-Host '=== 安装锁定的前端依赖 ==='
    npm ci
    if ($LASTEXITCODE -ne 0) {
        throw "前端依赖安装失败，退出码：$LASTEXITCODE。"
    }

    Write-Host '=== 检查前端代码 ==='
    npm run lint
    if ($LASTEXITCODE -ne 0) {
        throw "前端 lint 失败，退出码：$LASTEXITCODE。"
    }

    Write-Host '=== 构建并准备 CrossWebHost 前端资源 ==='
    npm run build:host
    if ($LASTEXITCODE -ne 0) {
        throw "前端构建或 CrossWebHost 资源准备失败，退出码：$LASTEXITCODE。"
    }
}
finally {
    Pop-Location
}

Write-Host '=== 构建 Desktop CrossWebHost ==='
dotnet build 'CrossWebHost/src/CrossWebHost.Desktop/CrossWebHost.Desktop.csproj' --nologo
if ($LASTEXITCODE -ne 0) {
    throw "Desktop CrossWebHost 构建失败，退出码：$LASTEXITCODE。"
}

Write-Host '=== 验证完成 ==='
Write-Host '请阅读 feature_list.json，并且一次只处理一个依赖已完成的任务。'
