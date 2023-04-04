var LP = {

    //注册事件
    RegisterEvent: function () {

    },
    Init: function () {
        LP.AllChecked(); //全选/反选
        //LP.TabStatusCount(); ///获取列表页的选项卡的数据
        LP.RegisterTips();
        //LP.LocationGo();
        //	LP.DataExport();
        LP.InitLayuiDate();
        //   LP.DetailToUrl();
        LP.InitBaseArea();
        LP.InitAreaName();
        LP.InitDefval();
        LP.RegisterEvent();
        //LP.InitUserInfo();
        LP.LayerTips();
        //LP.ClearAllTextTag();
        //LP.InitFmDateRang();
    },
    //layuiDate 时间插件初始化
    InitLayuiDate: function () {
        //layui-laydate---   1：【年月日】选择器   2：【年月日时分秒】选择器  3：【时分秒】选择器  4：【年月】选择器
        var getLen = $("[layui-laydate]").length;
        if (getLen > 0) {
            $("[layui-laydate]").each(function (i, dom) {
                if ($(this).attr('layui-laydate') == "1") {
                    laydate.render({
                        elem: this,
                    });
                } else if ($(this).attr('layui-laydate') == "2") {
                    laydate.render({
                        elem: this,
                        type: 'datetime',
                        //range: true,
                    });
                } else if ($(this).attr('layui-laydate') == "3") {
                    laydate.render({
                        elem: this,
                        type: 'time',
                    });
                } else if ($(this).attr('layui-laydate') == "4") {
                    laydate.render({
                        elem: this,
                        type: 'month',
                        //range: true,
                    });
                }
            });
        }
    },
    //渲染增改查模板
    DialogCRDReaderDataTmpl: function (data, html, w, h, title) {
        var tt = LP.CRDReaderDataTmpl(data, html);
        LP.DialogRb(tt, w, 0, title, 1);

    },
    //渲染增改查模板
    CRDReaderDataTmpl: function (data, html) {
        if (!html) return;
        if (data != null) {
            var lst = data;
            for (var it in data) {
                var str = "";
                if (lst[it] != null) str = lst[it];
                html = html.replace(new RegExp("\\{" + it + "\\}", "g"), str);
            }
        }
        var str = html;
        var res = str.match(/\{.*?\}/g);
        if (res != null) {
            for (var i = 0; i < res.length; i++) {
                html = html.replace(res[i], "");
            }
        }
        return html;
    },
    //全选/反选
    AllChecked: function () {
        if ($("[name=chk_all]").length > 0) {
            $("[name=chk_all]").on("click", function () {
                $("[name=chk_list]").prop("checked", $(this).prop("checked"));
            });
        }
    },
    //腾讯模板引擎
    TmplReader: (obj, tmpid, htmlid) => {
        var html = template(tmpid, obj);
        document.querySelector('#' + htmlid).innerHTML = html;
    },
    //获取用户是否选中数据
    isChecked: function (name) {
        var isTrue = false;
        var cond = $('input[name="' + name + '"]');
        for (var i = 0; i < cond.length; i++) {
            if (cond[i].checked == true) {
                isTrue = true;
                return isTrue;
            }
        }
        return isTrue;
    },
    DialogHtml: (config) => {

        if (!config.w) config.w = 0;
        if (!config.h) config.h = 0;
        var titleflag = false;
        if (config.title) titleflag = [config.title, 'font-size:18px;'];//标题栏标识
        if (!config.btnTxt) config.btnTxt = "提交"; var width = document.body.clientWidth;
        if (config.w == 0) config.w = width * 0.8;
        if (config.h == 0) config.h = window.innerHeight - 200;
        var index = layer.open({
            content: config.html,
            type: 1, offset: 'auto', title: titleflag,
            area: [config.w + 'px', config.h + 'px'],
            shade: 0.4,
            btn: [config.btnTxt, '取消'],
            yes: function (index, layero) {
                if (config.cb) config.cb();
            }, btn2: function (index, layero) {
                layer.close(index);
            }
        });
    },
    CommonAjax: function (url, data, title, cb) {
        if (title == undefined || title == null || title == "") {
            title = "确定操作吗？";
        }
        layer.confirm(title, function () {
            http.post(url, data).then(r => {
                if (typeof cb == "function") {
                    cb(r);
                } else {
                    if (r) {
                        LP.LayerMsg(r, function () {
                            if (r.attr != undefined && r.attr != null) {
                                window.location.href = r.attr;
                            } else {
                                window.location.reload();
                            }

                        });

                    }
                }
            }, err => {

            })
        }, function () {
            layer.closeAll();
        });


    },

    ///layer弹窗提示:成功1秒自动消失，失败需要手动点
    LayerMsg: function (data, cb) {
        var t = 2000;
        var ico = 5;
        if (data.success) {
            t = 3000;
            ico = 1;
            layer.msg(data.message, {
                icon: ico,
                time: t //2秒关闭（如果不配置，默认是3秒）
            }, cb);
        } else {
            layer.alert(data.message, {
                icon: ico
            }, function () {
                layer.closeAll();
                cb();
            });
        }
    },
    LayerTips: function (ops) {
        $("[tips]").unbind("mouseover").on("mouseover", function (e) {
            var id = $(this).attr("id");
            if (id == undefined) {
                id = new Date().valueOf();
                $(this).attr("id", id);
            }
            var txt = $(this).attr("tips");
            if (txt.length == 0) txt = $(this).text();
            layer.tips(txt, '#' + id, {
                tips: [3, '#FFB800'] //还可配置颜色
                ,
                time: 0
            });
        });
        $("[tips]").on("mouseout", function (e) {
            layer.closeAll();
        });
    },

    //右侧弹框:type:1id或类名 2：url，3具体的html ,2 offset:弹窗位置居右：rb，居中：auto，
    DialogRb: function (url, w, h, messageinfo, type, offset) {
        messageInfo = (messageinfo == "" || messageinfo == null) ? "message" : messageinfo;
        if (!w) w = 0;
        if (!h) h = 0;
        //自定页
        var width = document.body.clientWidth;
        var height = document.body.clientHeight;

        if (w == 0) w = width * 0.7;
        if (h == 0) h = window.innerHeight;
        //var sh = window.screen.height - 300;
        //if (sh > h) h = sh;
        if (type == undefined || type == null || type == "") type = 2; //url弹窗
        if (offset == undefined || offset == null || offset == "") offset = "rb"; //url弹窗

        if (type == 1) url = $(url).html();
        if (type == 3) type = 1; //弹div具体内容
        var index = layer.open({
            shift: 2,
            content: url,
            type: type,
            title: [messageInfo, 'font-size:18px;'],
            closeBtn: 1,
            area: [w + 'px', h + 'px'],
            maxmin: true,
            shadeClose: true,
            shade: 0.4,
            offset: offset,
            end: function () {
                layer.closeAll();
            },
            success: function (layero, index) { }
        });
    },

    //注册提示提示   
    RegisterTips: function () {
        if ($("[tipshtml=true]").length > 0) {
            $("[tipshtml=true]").each(function (i, dom) {
                $(dom).on("mouseover", function () {
                    var id = $(dom).attr("id");
                    if (id == undefined || id == null || id == "") {
                        id = "tips" + i;
                        $(dom).attr("id", id);
                    }
                    $(this).find("[tipshtml]").show();
                    var tipsHeight = $(this).find("[tipshtml]").height() + 20;
                    $(this).find("[tipshtml]").css("margin-top", -tipsHeight / 2);
                });
                $(dom).on("mouseout", function () {
                    $(dom).find(".tipscon").hide();
                });

            });
        }
    },

    GetQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return decodeURIComponent(unescape(r[2]));
        return "";
    },
    //将null值转换成 空字符 
    StrNullToEmpty: function (val) {
        if (val == "" || val == "null" ||
            val == null || val == "NaN" ||
            val == NaN) {
            return "";
        }
        return val;
    },

    //初始化url上参数
    InitUrlParm: function () {
        var parm = window.location.search.replace("?", "").split("&");
        if (parm.length > 0) {
            for (var i = 0; i < parm.length; i++) {
                var tt = parm[i].split('=');
                if (tt.length > 1 && tt[1].length > 0) {
                    var input = $("[name=" + tt[0] + "]");
                    if (input.length > 0) {
                        var type = $(input).attr("type");
                        if (type == "number" || type == "text") {
                            tt[1] = decodeURIComponent(tt[1])
                            $(input).val(tt[1]);
                        } else if (type == "radio") {
                            tt[1] = decodeURIComponent(tt[1])
                            QC.Checked(tt[0], tt[1]);
                        } else if ($(input).prop("tagName").toLowerCase() == "select") {
                            tt[1] = decodeURIComponent(tt[1])
                            $(input).find("option[value=" + tt[1] + "]").attr("selected", "selected");
                        }
                    }
                }
            }
        }
    },
    ParseParams: function (data) {
        try {
            var tempArr = [];
            for (var i in data) {
                var key = encodeURIComponent(i);
                var value = encodeURIComponent(data[i]);
                tempArr.push(key + '=' + value);
            }
            var urlParamsStr = tempArr.join('&');
            return urlParamsStr;
        } catch (err) {
            return '';
        }
    },
    UrlToJson: function (data) {
        var parm = data.split("&");
        var obj = {};
        if (parm.length > 0) {
            for (var i = 0; i < parm.length; i++) {
                var tt = parm[i].split('=');
                if (tt.length > 1) {
                    obj[tt[0]] = tt[1]; 
                }
            }
        }
        return obj;
    },
    //添加选项卡
    AddTab: function (title, url) {
        if (top.location != self.location)
            parent.$.tabs.addTab(title, url, title);
        else {
            window.location.href = url;
        }
    },
    ToastOk: function (title) {
        LP.ToastMsg(title, 1)
    },
    ToastMsg: function (title, icon) {
        return layer.msg(title, {
            icon: icon,
            shade: this.shade,
            scrollbar: false,
            time: 3000,
            shadeClose: true
        });
    },
    ToastError: function (title) {
        LP.ToastMsg(title, 2)
    },
    ToastCode: function (r) {
        if (r.code == 100)
            LP.ToastMsg(r.message, 1);
        else LP.ToastMsg(r.message, 2);
    },
    ///初始化区域
    //省
    InitBaseArea: function () {
        if ($("[chain-province]").length > 0) {
            $("[chain-province]").each(function (i, dom) {
                var chainid = $(dom).attr("chain-province");
                var defval = $(dom).attr("def-val");
                var grade = $(dom).attr("chain-grade");
                $(dom).empty();
                var template2 = "<option value=\"{0}\" >{1}</option>";
                $(dom).append(template2.replace("{0}", "").replace("{1}", "请选择省"));
                $("[chain-city=" + chainid + "]").append(template2.replace("{0}", "").replace("{1}",
                    "请选择市"));
                ///初始化大区域或省
                $.get("/ashx/common.ashx", {
                    act: "GetBaseArea",
                    grade: 2
                }, function (data) {
                    for (var i = 0; i < data.attr.length; i++) {
                        var item = data.attr[i];

                        var template = "<option value=\"{0}\"  {2}>{1}</option>";
                        template = template.replace("{0}", item.Id).replace("{1}", item.Name);
                        if (defval != undefined && defval > 0) {
                            if (defval == item.Id)
                                template = template.replace("{2}", "selected='selected'");
                            else
                                template = template.replace("{2}", "");

                            LP.InitCity(defval, chainid);
                        }
                        $(dom).append(template);
                    }

                }, "json");
                //当一级区域变化的时候
                $(dom).on("change", function () {
                    var id = $(dom).val();
                    LP.InitCity(id, chainid);

                });
                //当一级区域变化的时候
                $("[chain-city=" + chainid + "]").on("change", function () {
                    var id = $(this).val();
                    LP.InitCounty(id, chainid);

                });

            });


        }

    },
    //市
    InitCity: function (id, chainid) {
        $.get("/ashx/common.ashx", {
            act: "GetBaseArea",
            id: id
        }, function (data) {
            $("[chain-city=" + chainid + "]").empty();
            var template2 = "<option value=\"{0}\" >{1}</option>";
            template2 = template2.replace("{0}", "").replace("{1}", "请选择市");
            $("[chain-city=" + chainid + "]").append(template2);
            var defval = $("[chain-city=" + chainid + "]").attr("def-val");
            for (var i = 0; i < data.attr.length; i++) {
                var item = data.attr[i];
                var template = "<option value=\"{0}\" {2}>{1}</option>";
                template = template.replace("{0}", item.Id).replace("{1}", item.Name);
                if (defval != undefined && defval > 0 && defval == item.Id) {
                    template = template.replace("{2}", "selected='selected'");
                    LP.InitCounty(defval, chainid);
                }
                $("[chain-city=" + chainid + "]").append(template);
            }
        }, "json");

    },
    //县
    InitCounty: function (id, chainid) {
        $.get("/ashx/common.ashx", {
            act: "GetBaseArea",
            id: id
        }, function (data) {
            $("[chain-county=" + chainid + "]").empty();
            var template2 = "<option value=\"{0}\" >{1}</option>";
            template2 = template2.replace("{0}", "").replace("{1}", "请选择县(区)");
            $("[chain-county=" + chainid + "]").append(template2);
            var defval = $("[chain-county=" + chainid + "]").attr("def-val");
            for (var i = 0; i < data.attr.length; i++) {
                var item = data.attr[i];
                var template = "<option value=\"{0}\" {2}>{1}</option>";
                template = template.replace("{0}", item.Id).replace("{1}", item.Name);
                if (defval != undefined && defval > 0 && defval == item.Id) {
                    template = template.replace("{2}", "selected='selected'")
                }
                $("[chain-county=" + chainid + "]").append(template);
            }
        }, "json");

    },

    //初始化省市县名称
    InitAreaName: function () {
        if ($("[basearea-id]").length > 0) {
            $("[basearea-id]").each(function (i, dom) {
                var id = $(dom).attr("basearea-id");
                ///初始化大区域或省
                $.get("/ashx/common.ashx", {
                    act: "GetBaseAreaById",
                    id: id
                }, function (data) {
                    $(dom).text(data.attr.Name);
                }, "json");


            });


        }
    },

    ///初始化默认值
    InitDefval: function () {
        if ($("[defval]").length > 0) {
            $("[defval]").each(function (i, dom) {
                var defval = $(dom).attr("defval");
                var type = $(dom).attr("type");
                var name = $(dom).attr("name");
                if (defval != null && defval != "") {
                    if (type != undefined && type != null && (type == "radio" || type == "checkbox")) {
                        var value = $(dom).val();
                        if (value == defval) $(dom).attr("checked", "checked");
                        else $(dom).removeAttr("checked");
                    } else {
                        $(dom).val(defval);
                    }
                }
            });
        }
    },


    DateFormat: function (fmt, date) {
        var o = {
            "M+": date.getMonth() + 1, //月份 
            "d+": date.getDate(), //日 
            "h+": date.getHours(), //小时 
            "m+": date.getMinutes(), //分 
            "s+": date.getSeconds(), //秒 
            "q+": Math.floor((date.getMonth() + 3) / 3), //季度 
            "S": date.getMilliseconds() //毫秒 
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[
                    k]).length)));
        return fmt;
    },
    //深复制对象方法    
    CloneObj: function (obj,setVal) {
        var newObj = {};
        if (obj instanceof Array) {
            newObj = [];
        }
        for (var key in obj) {
            var val = obj[key];
            //不复制值
            if (setVal == 0) { newObj[key] = typeof val === 'object' ? LP.CloneObj(val, setVal) : "";}
            else {
                newObj[key] = typeof val === 'object' ? LP.CloneObj(val, setVal) : val;
            }
        }
        return newObj;
    }

}
window.onload = () => {
    //layer.alert("LP...");

}


layui.define(["layer", "jquery"], function (exports) {
    exports("LP", LP);
});

//exports('LP', LP);
