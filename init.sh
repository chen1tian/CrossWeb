#!/usr/bin/env bash
set -euo pipefail

echo "=== CrossWeb harness 验证 ==="
dotnet --version

if [[ ! -f "CrossWebFrontend/package.json" ]]; then
  echo "未找到 CrossWebFrontend/package.json。" >&2
  exit 1
fi

echo "=== 安装锁定的前端依赖 ==="
npm --prefix CrossWebFrontend ci

echo "=== 检查前端代码 ==="
npm --prefix CrossWebFrontend run lint

echo "=== 构建并准备 CrossWebHost 前端资源 ==="
npm --prefix CrossWebFrontend run build:host

echo "=== 构建 Desktop CrossWebHost ==="
dotnet build CrossWebHost/src/CrossWebHost.Desktop/CrossWebHost.Desktop.csproj --nologo

echo "=== 验证完成 ==="
echo "请阅读 feature_list.json，并且一次只处理一个依赖已完成的任务。"
