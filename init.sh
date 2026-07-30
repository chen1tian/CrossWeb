#!/usr/bin/env bash
set -euo pipefail

echo "=== AvWeb harness verification ==="
dotnet --version

echo "=== Build Desktop Host ==="
dotnet build Host/src/Host.Desktop/Host.Desktop.csproj --nologo

if [[ -f "Frontend/package.json" ]]; then
  echo "Frontend/package.json exists, but frontend verification must be added after the package manager is selected."
fi

echo "=== Verification complete ==="
echo "Read feature_list.json and work on exactly one eligible feature."
