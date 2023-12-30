# filetrans
C#.net 在windows环境下部署站点,编译发布，一健发布部署

框架：
trans:文件发布接收器   
用的是 .net framework4.8框架+winform的形式

admin:管理工具，用的是.net core6.0框架+前端layui框架



trans:有文件传输,接收的功能 替换传统的ftp，同时与本地web（admin）交互的功能 起到一个中间桥梁<br>
admin:<br>
|-----站点管理：编辑，克隆，取消，建站点 建虚拟目录，停止站点 ，移除站点功能  <br>
|-----发布目录  <br>
|-----server设置  <br>
|-----发布生产  <br>
|-----发布测试  <br>

发布站点前：要先启动trans文件  启动后刷新下页面   web会去连接本地trans是否启动

