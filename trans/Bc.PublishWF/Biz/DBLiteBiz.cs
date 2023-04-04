using Bc.PublishWF.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Bc.PublishWF.Biz
{
    public class DBLiteBiz
    {
        private string ConnectionString()
        {
            return "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "DB\\Config.db";
        }
        public List<siteinfo> GetSiteData()
        {
            var dt = SQLiteHelper.ExecuteDataTable("select * from site", ConnectionString());
            return ModelConvertHelper<siteinfo>.ConvertToModel(dt).ToList();
        }

        public int SaveSite(siteinfo info, ref string msg)
        {
            var ss = GetSiteByName(info.sitename);
            var sqlfr = "insert into site(id,sitename,compressname,sitedir,targetdir,targetbackupdir,targetserver,targetserver2,targetserver3,iscompress)values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9})";
            if (ss != null && info.id > 0)
            {
                sqlfr = "update  site set sitename='{1}'," +
                    "compressname='{2}'," +
                    "sitedir='{3}'," +
                    "targetdir='{4}'," +
                    "targetbackupdir='{5}'," +
                    "targetserver='{6}'," +
                     "targetserver2='{7}'," +
                      "targetserver3='{8}'," +
                       "iscompress={9}" +
                       " where id={0}";
            }
            else
            {
                var id = 1;
                var dt = SQLiteHelper.ExecuteDataTable("select max(id)+1 from site", ConnectionString());
                if (dt != null && dt.Rows.Count > 0)
                { id = dt.Rows[0][0].ToInt2(); }
                info.id = id;
            }

            var sql = string.Format(sqlfr, info.id, info.sitename, info.compressname, info.sitedir, info.targetdir, info.targetbackupdir, info.targetserver, info.targetserver2, info.targetserver3, info.iscompress.ToInt2());
            return SQLiteHelper.ExecuteNonQuery(sql, ConnectionString(), CommandType.Text);
        }
        public int DelteSite(string sitename)
        {
            if (string.IsNullOrWhiteSpace(sitename)) return 0;
            var sql = @"delete from site where sitename='" + sitename.Trims() + "';";
            sql += @"delete from excludedir where sitename='" + sitename.Trims() + "';";
            return SQLiteHelper.ExecuteNonQuery(sql, ConnectionString());
        }

        public siteinfo GetSiteByName(string sitename)
        {
            var dt = SQLiteHelper.ExecuteDataTable("select * from site where sitename='" + sitename + "'", ConnectionString());
            return ModelConvertHelper<siteinfo>.ConvertToModel(dt).FirstOrDefault();
        }


        public List<excludedirinfo> GetExcludeData(string sitename)
        {
            var dt = SQLiteHelper.ExecuteDataTable("select * from excludedir where sitename='" + sitename + "'", ConnectionString());
            return ModelConvertHelper<excludedirinfo>.ConvertToModel(dt).ToList();
        }
        public excludedirinfo GetExcludeDataById(int id)
        {
            var dt = SQLiteHelper.ExecuteDataTable("select * from excludedir where Id=" + id, ConnectionString());
            return ModelConvertHelper<excludedirinfo>.ConvertToModel(dt).FirstOrDefault();
        }
        public int SaveExclude(excludedirinfo info, ref string msg)
        {
            var ss = GetExcludeDataById(info.id);
            var sqlfr = "";
            var sql = "";
            if (ss != null && info.id > 0)
            {
                sqlfr = "update  excludedir set excludedir='{1}'  " +
                       " where id={0}";
                sql = string.Format(sqlfr, info.id, info.excludedir);
            }
            else
            {
                var id = 1;
                var dt = SQLiteHelper.ExecuteDataTable("select max(id)+1 from excludedir", ConnectionString());
                if (dt != null && dt.Rows.Count > 0)
                { id = dt.Rows[0][0].ToInt2(); }
                sqlfr = "insert into excludedir(id,sitename,excludedir)values({0},'{1}','{2}')";
                sql = string.Format(sqlfr, id, info.sitename, info.excludedir);
            }

            return SQLiteHelper.ExecuteNonQuery(sql, ConnectionString(), CommandType.Text);
        }

        public int CopyExcludeSite(string sitename)
        {
            var lst = GetExcludeData("manage.bcunite.com");
            var lstSitename = new List<string>() {
                "wx.bcunite.com","supply.bcunite.com","cserp.bcunite.com",
                "cswx.bcunite.com","wx.bcunite.com","api.bcunite.com"
                ,"csapi.bcunite.com"
            };
            var msg = "";
            foreach (var it in lstSitename)
            {
                foreach (var sub in lst)
                {
                    var isss = sub.excludedir.Replace("manage.bcunite.com", it);
                    SaveExclude(new excludedirinfo() { sitename = it, excludedir = isss },ref msg);
                }
            }
            return 1;
            //return SQLiteHelper.ExecuteNonQuery(sql, ConnectionString());
        }
        public int DelteExclude(int id)
        {
            if (id == 0) return 0;
            return SQLiteHelper.ExecuteNonQuery("delete from excludedir where id=" + id, ConnectionString());
        }
    }
}
