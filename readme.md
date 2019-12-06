- chrome 浏览器器 快捷方式命令 --kiosk http://192.168.60.110:8071 --disable-infobars --disable-background-networking

- 必须使用chrome 73版本浏览器

- 创建服务命令
sc create PharmService binpath= "D:\GitHub\PharmWebSocket\Pharmacy Reporter.Console\bin\Debug\Pharmacy ReporterConsoler.exe -service"  start= auto displayname= "PharmService"

-删除服务
sc delete PharmService  