$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Write-Host '=== AvWeb harness 验证 ==='
dotnet --version

Write-Host '=== 构建 Desktop Host ==='
dotnet build 'Host/src/Host.Desktop/Host.Desktop.csproj' --nologo
if ($LASTEXITCODE -ne 0) {
    throw "Desktop Host 构建失败，退出码：$LASTEXITCODE。"
}

if (-not (Test-Path -LiteralPath 'Frontend/package.json')) {
    throw '未找到 Frontend/package.json。'
}

Push-Location 'Frontend'
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

    Write-Host '=== 构建前端 ==='
    npm run build
    if ($LASTEXITCODE -ne 0) {
        throw "前端构建失败，退出码：$LASTEXITCODE。"
    }
}
finally {
    Pop-Location
}

Write-Host '=== 验证完成 ==='
Write-Host '请阅读 feature_list.json，并且一次只处理一个依赖已完成的任务。'
