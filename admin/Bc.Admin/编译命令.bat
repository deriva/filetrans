
@echo off
E:
set srccodedir=E:\project\code\ZT\ZTEnergyAllInOneV2\2.soft\EnergyAllInOne\WebHost\EnergyAllInOne.WebHost
set buildTodir=E:\publicdir\AllInOneV2_Site\
 


 
cd %srccodedir%
rem 删除文件目录
rd /s/q %buildTodir%

dotnet publish -c Release -r win-x64 -o %buildTodir%   -f net6.0  --self-contained false

cd %buildTodir%
del /f /s /q %buildTodir%*.json