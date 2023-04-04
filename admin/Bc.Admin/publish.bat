echo  Í£Ö¹Õ¾µã
C:\Windows\System32\inetsrv\appcmd.exe stop site  "log.bcunite.com"
E:
cd  E:\Project\donet\Bc.Project\Bc.Log\trunk\Bc.Admin
dotnet publish -r win10-x64 -o D:\website\log.bcunite.com

echo  Æô¶¯Õ¾µã
C:\Windows\System32\inetsrv\appcmd.exe start site "log.bcunite.com"

pause