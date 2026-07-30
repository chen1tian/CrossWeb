#!/usr/bin/env bash
set -euo pipefail

echo "=== AvWeb harness 验证 ==="
dotnet --version

echo "=== 构建 Desktop Host ==="
dotnet build Host/src/Host.Desktop/Host.Desktop.csproj --nologo

if [[ ! -f "Frontend/package.json" ]]; then
  echo "未找到 Frontend/package.json。" >&2
  exit 1
fi

echo "=== 安装锁定的前端依赖 ==="
npm --prefix Frontend ci

echo "=== 检查前端代码 ==="
npm --prefix Frontend run lint

echo "=== 构建前端 ==="
npm --prefix Frontend run build

echo "=== 验证完成 ==="
echo "请阅读 feature_list.json，并且一次只处理一个依赖已完成的任务。"
