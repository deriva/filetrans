/*单页面的增删改查*/

var BJPL = {
    Init: function () {
        //初始化url参数到搜索对象里
        //	Bind.Init(ban.$data);
        //	BJPL.BindVue();
        BJPL.InitUrlParmToSearch();
        BJPL.SearchData(0);

    },
    //初始化url参数到搜索对象里
    InitUrlParmToSearch: () => {
        var parm = window.location.search.replace("?", "").split("&");
        if (parm.length > 0) {
            for (var i = 0; i < parm.length; i++) {
                var tt = parm[i].split('=');
                if (tt.length > 1 && tt[1].length > 0) {
                    bfUnilt._setVmVal(ban.$data, "Search." + tt[0], decodeURIComponent(tt[1]));
                    //ban.$data["Search." + tt[0]] = decodeURIComponent(tt[1]);
                }
            }
        }
    },
    SearchData: (changepage) => { //初始化搜索数据
        if (changepage == 0) {
            ban.$data.Search.Page = 1;
        }
        layer.closeAll();
        ban.$data.Lst = [];
        http.get(ban.$data.SearchUrl + "?" + LP.ParseParams(ban.$data.Search), "").then(r => {
            var lst = [];
            var totalCount = 0;
            layer.closeAll();
            if (r.code == 100) {
                if (r.attr && "DataSource" in r.attr) {
                    lst = r.attr.DataSource;
                    totalCount = r.attr.TotalCount;
                } else {
                    lst = r.Items;
                    totalCount = r.TotalCount;
                }
                ban.$data.Lst = lst;
                if (ban.$data.LstCb) {//回调处理下数据
                    eval(ban.$data.LstCb + "('" + JSON.stringify(r) + "')");
                }


                if (ban.$data.searchcb) {
                    eval(ban.$data.searchcb + "('" + JSON.stringify(r) + "')");
                }
                BJPL.ReaderData();
                BJPL.ReaderPage(changepage, totalCount);
                if (ban.$data.lstreadercb) {//渲染后回调
                    eval(ban.$data.lstreadercb + "()");
                }
            } else {
                BJPL.ReaderData();
                if (!r.message) r.message = "暂无数据";
                top.message.Warn(r.message);
            }
        });
    },
    //渲染分页
    ReaderPage: (changepage, totalcount) => {
        if (changepage == 0) {
            layui.use('laypage', function () {
                var laypage = layui.laypage;
                laypage.render({
                    elem: 'pagezone', //分页容器的id 
                    count: totalcount, //数据总数
                    limit: ban.$data.Search.PageSize, //每页显示的数据条数
                    skin: '#1E9FFF', //自定义选中色值 
                    layout: ['page', 'count'],
                    jump: function (obj, first) {
                        if (!first) {
                            ban.$data.Search.Page = obj.curr;
                            BJPL.SearchData(1);
                        }
                    }
                });
            });
        }
    },
    //渲染列表数据
    ReaderData: () => {
        LP.TmplReader({
            lst: JSON.parse(JSON.stringify(ban.$data.Lst))
        }, 'tpl_lsttable', 'listtable');
        //涉及到权限的  在此重新渲染
    },
    //编辑保存数据
    Submit: function () {
        var data = $("#fmedit").serializeJson();
        LP.CommonAjax(ban.$data.EditSaveUrl, data, "确认操作吗?", function (r) {
            if (r.code == 100) {
                BJPL.SearchData(0);
            } else {
                top.LP.ToastError(r.message);

            }
        });
    },
    //弹窗:编辑 
    DialogEdit: (i) => {
        var data = ban.$data.Edit;
        if (i >= 0) {
            ban.$data.Edit = ban.$data.Lst[i];
            data = ban.$data.Edit;
        }
        else {
            data = {};
        }
        var html = template('tpl_edit', data);
        var w = 0; h = 0;
        if (ban.$data.EditW) w = ban.$data.EditW;
        LP.DialogRb(html, w, h, "", 3);
        LP.InitDefval();

    },
    //弹窗:查看
    DialogView: (i) => {
        var html = template('tpl_view', ban.$data.Lst[i]);
        var w = 700; h = 0;
        if (ban.$data.DetailW) w = ban.$data.DetailW;
        LP.DialogRb(html, w, h, "", 3);
    },
    //取消
    Cancel: (obj) => {
        LP.CommonAjax(ban.$data.CancelUrl + "?" + LP.ParseParams(obj), "", "确认操作吗?", function (r) {
            if (r.code == 100) { 
                BJPL.SearchData(0);
            } else {
                top.message.Error(r.message);
            }
        });
    },
}

$(() => {

    BJPL.Init();
});
