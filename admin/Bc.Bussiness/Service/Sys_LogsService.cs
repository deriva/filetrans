using Bc.Common;
using Bc.Common.Helpers;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Bussiness.Service
{

    public class Sys_LogsService : BaseService<Sys_Logs>, ISys_LogsService
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int AddInfo(Sys_Logs info)
        {
            try
            {
                info.CreateTime = DateTime.Now;
                info.UserAgent = info.UserAgent.SubLen(300);
                info.QueryString = info.QueryString.SubLen(400);
                this.Add(info);
            }
            catch (Exception ex) { }
            return 1;

        }

        /// <summary>
        /// 查询vin信息（分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<Sys_Logs> QueryPages(Sys_LogsDto parm)
        {
            var expr = PredicateBuilder.True<Sys_Logs>();
            if (!parm.Host.IsEmpty()) expr = expr.And(x => x.Host == parm.Host.Trim());
            if (!parm.Level.IsEmpty()) expr = expr.And(x => x.Level == parm.Level.Trim());
            if (!parm.IPAddress.IsEmpty()) expr = expr.And(x => x.IPAddress == parm.IPAddress.Trim());
            if (parm.CreateTime.HasValue) expr = expr.And(x => x.CreateTime>= parm.CreateTime.Value);
            if (parm.CreateTime1.HasValue) expr = expr.And(x => x.CreateTime <= parm.CreateTime1.Value);
            var source = Db.Queryable<Sys_Logs>().Where(expr);
            parm.OrderBy = "CreateTime";
            parm.Sort = "descending";
            return source.ToPage(new PageParm { Page = parm.Page, PageSize = parm.PageSize, OrderBy = parm.OrderBy, Sort = parm.Sort });
        }



    }
}
