using Bc.Bussiness.IService;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.Dto;
using System;

namespace Bc.Bussiness.Service
{
    public class SiteVirtualDirService : BaseService<SiteVirtualDir>, ISiteVirtualDirService
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int Save(SiteVirtualDir info)
        {
            try
            {
                if (info.SiteName.IsEmpty()) return 0;
                if (info.Id == 0)
                {

                    return this.Add(info);
                }
                else
                {
                    var temp = GetId(info.Id);
                    temp.VirtualName = info.VirtualName;
                    temp.VirtualDir = info.VirtualDir;

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
        public PagedInfo<SiteVirtualDir> QueryPages(SiteVirtualDirDto parm)
        {
            var expr = PredicateBuilder.True<SiteVirtualDir>();
            if (!parm.SiteNo.IsEmpty()) expr = expr.And(x => x.SiteNo == parm.SiteNo.Trim());
            if (!parm.SiteName.IsEmpty()) expr = expr.And(x => x.SiteName == parm.SiteName.Trim()); 
            var source = Db.Queryable<SiteVirtualDir>().Where(expr);
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
