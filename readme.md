- chrome ������� ��ݷ�ʽ���� --kiosk http://192.168.60.110:8071 --disable-infobars --disable-background-networking

- ����ʹ��chrome 73�汾�����

- ������������
sc create PharmService binpath= "D:\GitHub\PharmWebSocket\Pharmacy Reporter.Console\bin\Debug\Pharmacy ReporterConsoler.exe -service"  start= auto displayname= "PharmService"

-ɾ������
sc delete PharmService  