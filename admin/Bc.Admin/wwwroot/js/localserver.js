var isOpenLocalServer = false;
var localserver_url = "http://127.0.0.1:15080";
var ls = {//localserver
    GetIISData:   (r)=> {
        var bind = r.attr.Bindings[0];
        var serverip = bind.ServerIP.split(':');
        var obj = {
            msg: JSON.stringify(r.attr),
            cmd: "IIS_Create",
            flag: 0, parm1: "",
            parm2: serverip[0],//ip
            parm3: serverip[1]//端口
        }
        return obj;
    },
    CreateIIS_Site: (no) => {
        http.get("/site/GetIISWebSite?no=" + no, "").then(r => {
            if (r.code == 100) {
                var obj = ls.GetIISData(r);
                obj.cmd = "IIS_Create"; 
                ls.Call(obj);
            }
        });
    },
    EnabledIIS_Site: (no, type) => {
        http.get("/site/GetIISWebSite?no=" + no, "").then(r => {
            if (r.code == 100) {
                var obj = ls.GetIISData(r);
                obj.cmd = "IIS_Enabled";
                obj.parm1 = type; 
                ls.Call(obj);
            }
        });
    },
    PublicSite: (no, env) => {//发布站点
        layer.confirm("确定发布吗", function () { 
            var obj = {
                no: no,
                cmd: "publicsite",
                env: env
            }
            ls.Call(obj);
        });
    },
    Call: (data) => {
        if (!isOpenLocalServer) {
            LP.ToastError("本地服务未开启"); return;
        }
        var url = localserver_url + "/site/" + data.cmd;
        http.post(url, data).then(r => {
            LP.ToastCode(r);
        });
    },
    IsOpen: () => {
        var url = localserver_url + "/home/index";
        http.post(url, {}).then(r => {
            isOpenLocalServer = true;
        });
    }
}

$(() => {
    ls.IsOpen();
});