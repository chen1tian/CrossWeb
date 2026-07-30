# 跨平台应用

## 说明
这是一个新建的跨平台项目，技术架构如下：
1. 前端: React项目，现在尚未创建
2. 后端（Host）：跨平台Avalonia项目

后端Host使用WebView承载前端的网页，使用PicoServer作为本地Web服务器，前端React项目通过PicoServer访问后端Host提供的API接口。
PicoServer(https://picoserver.cn/guide.html)
PcoServer 实时通讯 文档: https://picoserver.cn/realtime.html

## 需求

### 第一阶段

1. 创建前端文件
2. 修改后端Host:
   1. 增加PicoServer支持
   2. 修改为WebView承载前端网页
3. 完成最小可用产品（MVP）：
   1. 前端React项目可以通过PicoServer访问后端Host提供的API接口
   2. 后端Host可以通过WebView显示前端网页
   3. 一次请求交互：ping，pong，前端发送ping请求，后端返回pong响应

