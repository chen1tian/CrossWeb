$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Write-Host '=== AvWeb harness verification ==='
dotnet --version

Write-Host '=== Build Desktop Host ==='
dotnet build 'Host/src/Host.Desktop/Host.Desktop.csproj' --nologo
if ($LASTEXITCODE -ne 0) {
    throw "Desktop Host build failed with exit code $LASTEXITCODE."
}

if (Test-Path -LiteralPath 'Frontend/package.json') {
    Write-Warning 'Frontend/package.json exists, but frontend verification must be added after the package manager is selected.'
}

Write-Host '=== Verification complete ==='
Write-Host 'Read feature_list.json and work on exactly one eligible feature.'
