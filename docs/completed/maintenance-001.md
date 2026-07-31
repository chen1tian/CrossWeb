# maintenance-001：同步 Android 回环 HTTP 配置

- 状态：已完成
- 完成日期：2026-07-31
- 完成提交：`6f0b6ac`（`修正android下无法启动端口的问题`）

## 完成内容

- Android 清单引用只允许 `localhost` 和 `127.0.0.1` 明文 HTTP 的 Network
  Security Config。
- PicoServer 回环自检失败时，按 Android 和其他平台显示准确诊断提示。
- CrossWebHost Android Release 构建通过。
- 自定义名称模板实例包含正确替换的 Android 配置与 PicoServer 提示。
- Android 真机能够打开 PicoServer 提供的前端页面。
