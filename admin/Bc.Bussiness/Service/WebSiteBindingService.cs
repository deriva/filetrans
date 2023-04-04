using Bc.Bussiness.IService;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.Dto;
using Bc.Model.Dto.System;
using System;

namespace Bc.Bussiness.Service
{
    public class WebSiteBindingService : BaseService<WebSiteBinding>, IWebSiteBindingService
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int Save(WebSiteBinding info)
        {
            try
            {
                 
                if (info.Id == 0)
                {

                    return this.Add(info);
                }
                else
                {
                    var temp = GetId(info.Id);
                   
                    return this.Update(temp);
                }

            }
            catch (Exception ex) { }
            return 1;

        }

        /// <summary>
        /// 查询vin信息（分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<WebSiteBinding> QueryPages(WebSiteBindingDto parm)
        {
            var expr = PredicateBuilder.True<WebSiteBinding>();
            if (!parm.No.IsEmpty()) expr = expr.And(x => x.No == parm.No.Trim()); 
            var source = Db.Queryable<WebSiteBinding>().Where(expr);
            parm.OrderBy = "Id";
            parm.Sort = "descending";
            return source.ToPage(new PageParm { Page = parm.Page, PageSize = parm.PageSize, OrderBy = parm.OrderBy, Sort = parm.Sort });
        }

        public int Delete(int id)
        {
            return Db.Deleteable<SiteVirtualDir>(x => x.Id == id).ExecuteCommand();
        }



    }
}
