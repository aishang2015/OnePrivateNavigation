# OnePrivateNavigation

一个极简风格的导航网站，自托管，开箱即用，适合个人和小组使用。

## 使用方式
下载压缩包，找到OnePrivateNavigation.exe双击运行。
浏览器访问 http://127.0.0.1:30000/ 进行访问，局域网修改对应ip即可。
http://127.0.0.1:30000/login 进入管理后台管理导航分组和网址，默认用户名: admin 密码:admin 
可自行用nssm或其他方式托管服务

可修改appsettings.json来配置启动端口。程序启动后，数据库默认会生成在程序目录下的DB文件夹内。

## 开发
使用blazor混合模式项目模板，webassembly组件 + api方式实现基本功能。
克隆项目，用vs打开解决方案，把OnePrivateNavigation设为启动项目。直接f5调试即可。

## 截图

![屏幕截图_24-3-2025_154438_127 0 0 1](https://github.com/user-attachments/assets/6a14b2cc-b49d-4c40-bd92-b80149cfffc5)

