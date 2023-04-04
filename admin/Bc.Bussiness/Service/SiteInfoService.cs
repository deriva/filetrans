using Bc.Bussiness.IService;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bc.Bussiness.Service
{
    public class SiteInfoService : BaseService<SiteInfo>, ISiteInfoService
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int SaveSiteInfo(SiteInfo info)
        {
            try
            {
                if (info.SiteName.IsEmpty()) return 0;
                if (info.Id == 0)
                {
                    info.No = CaptchaUtil.GetRandomEnDigitalText(10);
                    info.PublicTime = DateTime.Now;
                    info.Status = 1;
                    info.PublicTime = DateTime.Now;
                    info.UpdateTime = DateTime.Now;

                    return this.Add(info);
                }
                else
                {
                    var temp = GetId(info.Id);
                    temp.IsCompress = info.IsCompress;
                    temp.ServerIP = info.ServerIP;
                    temp.SiteDir = info.SiteDir;
                    temp.Env = info.Env;
                    temp.SiteName = info.SiteName;
                    temp.Port = info.Port;
                    temp.CLRVeersion = info.CLRVeersion;
                    temp.UpdateTime = DateTime.Now;
                    temp.Status = info.Status;
                    temp.Protocol = info.Protocol;
                    return this.Update(temp);
                }

            }
            catch (Exception ex) { }
            return 0;

        }
        public int Cancel(int id)
        {
            var r = 0;
            var parm = GetId(id);
            var source = Db.Queryable<PublicToSite>().Where(x => x.ServerNo == parm.No).ToList();
            if (source.Count() > 0)
           r+=     Db.Deleteable<PublicToSite>(source).ExecuteCommand();
            r += Db.Deleteable<SiteInfo>(parm).ExecuteCommand();

            return r;
        }
        /// <summary>
        /// 查询vin信息（分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<SiteInfo> QueryPages(SiteInfoDto parm)
        {
            var expr = PredicateBuilder.True<SiteInfo>();
            if (!parm.No.IsEmpty()) expr = expr.And(x => x.No == parm.No.Trim());
            if (!parm.SiteName.IsEmpty()) expr = expr.And(x => x.SiteName == parm.SiteName.Trim());
            //if (!parm.Level.IsEmpty()) expr = expr.And(x => x.Level == parm.Level.Trim());
            //if (!parm.IPAddress.IsEmpty()) expr = expr.And(x => x.IPAddress == parm.IPAddress.Trim());
            if (parm.Env > 0) expr = expr.And(x => x.Env == parm.Env);
            //if (parm.CreateTime1.HasValue) expr = expr.And(x => x.CreateTime <= parm.CreateTime1.Value);
            var source = Db.Queryable<SiteInfo>().Where(expr);
            parm.OrderBy = "Id";
            parm.Sort = "descending";
            return source.ToPage(new PageParm { Page = parm.Page, PageSize = parm.PageSize, OrderBy = parm.OrderBy, Sort = parm.Sort });
        }

        /// <summary>
        /// 查询站点信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public SiteInfo GetSiteInfo(string no)
        {
            var expr = PredicateBuilder.True<SiteInfo>();
            expr = expr.And(x => x.No == no.Trim());
            var source = Db.Queryable<SiteInfo>().Where(expr).First();

            return source;
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int SavePublicFileDir(PublicFileDir info)
        {
            try
            {
                if (info.Id == 0)
                {

                    return this.Db.Insertable<PublicFileDir>(info).ExecuteCommand();
                }
                else
                {
                    return this.Db.Updateable<PublicFileDir>(info).ExecuteCommand();
                }

            }
            catch (Exception ex) { }
            return 1;

        }

        /// <summary>
        /// 查询 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PublicFileDir> PublicFileDirQueryPages(PublicFileDirDto parm)
        {
            var expr = PredicateBuilder.True<PublicFileDir>();
 
            if (parm.Id > 0) expr = expr.And(x => x.Id == parm.Id);
            if (!parm.SiteName.IsEmpty()) expr = expr.And(x => x.SiteName == parm.SiteName.Trim());
            if (!parm.PublicNo.IsEmpty()) expr = expr.And(x => x.PublicNo == parm.PublicNo.Trim());
            //if (!parm.Level.IsEmpty()) expr = expr.And(x => x.Level == parm.Level.Trim());
            //if (!parm.IPAddress.IsEmpty()) expr = expr.And(x => x.IPAddress == parm.IPAddress.Trim());
            //if (parm.CreateTime.HasValue) expr = expr.And(x => x.CreateTime >= parm.CreateTime.Value);
            //if (parm.CreateTime1.HasValue) expr = expr.And(x => x.CreateTime <= parm.CreateTime1.Value);
            var source = Db.Queryable<PublicFileDir>().Where(expr);
            parm.OrderBy = "Id";
            parm.Sort = "descending";
            return source.ToPage(new PageParm { Page = parm.Page, PageSize = parm.PageSize, OrderBy = parm.OrderBy, Sort = parm.Sort });
        }

        /// <summary>
        /// 查询发布目录对站点 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<PublicToSite> PublicToSiteList(PublicToSite parm)
        {
            var expr = PredicateBuilder.True<PublicToSite>();
            if (!parm.ServerSiteName.IsEmpty()) expr = expr.And(x => x.ServerSiteName == parm.ServerSiteName.Trim());
            if (parm.Id > 0) expr = expr.And(x => x.Id == parm.Id);
            if (!parm.ServerNo.IsEmpty()) expr = expr.And(x => x.ServerNo == parm.ServerNo.Trim());
            if (!parm.PublicNo.IsEmpty()) expr = expr.And(x => x.PublicNo == parm.PublicNo.Trim());
            var source = Db.Queryable<PublicToSite>().Where(expr).ToList();
            return source;
        }
        /// <summary>
        /// 查询发布目录对站点 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public int SavePublicToSite(PublicToSite parm)
        {
            var source = Db.Queryable<PublicToSite>().Where(x => x.ServerNo == parm.ServerNo && x.PublicNo == parm.PublicNo).First();
            if (source == null)
            {
                return Db.Insertable<PublicToSite>(parm).ExecuteCommand();
            }
            else
            {
                source.ServerSiteName = parm.ServerSiteName;

                return Db.Updateable<PublicToSite>(source).ExecuteCommand();
            }

        }
        /// <summary>
        /// 查询发布目录对站点 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public int DeletePublicToSite(PublicToSite parm)
        {
            return Db.Deleteable<PublicToSite>(x => x.Id == parm.Id).ExecuteCommand();
        }
 

        /// <summary>
        ///  获取发布站点目录V2版本
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PublicFileDirDto GetPublicFileDirByNoV2(string publicNo, int env)
        {
            var info = this.Db.Queryable<PublicFileDir>().Where(x => x.PublicNo == publicNo).First();
            var servernos = Db.Queryable<PublicToSite>().Where(x => x.PublicNo == publicNo).Select(x => x.ServerNo).ToList();
            var lstSiteInfo = Db.Queryable<SiteInfo>().Where(x => servernos.Contains(x.No) && x.Env == env).ToList();

            var mod = TransExpV2Helper<PublicFileDir, PublicFileDirDto>.Trans(info);
            //对应的目标站点
            mod.LstSiteInfo = lstSiteInfo;
            mod.LstSitePublicExcludeDir = this.Db.Queryable<SitePublicExcludeDir>().Where(x => publicNo == (x.No)).ToList();

            return mod;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int SaveSitePublicExcludeDir(SitePublicExcludeDir info)
        {
            try
            {
                if (info.Id == 0)
                {

                    return this.Db.Insertable<SitePublicExcludeDir>(info).ExecuteCommand();
                }
                else
                {
                    return this.Db.Updateable<SitePublicExcludeDir>(info).ExecuteCommand();
                }

            }
            catch (Exception ex) { }
            return 1;

        }
        /// <summary>
        /// 查询排除列表数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<SitePublicExcludeDir> SitePublicExcludeDirList(SitePublicExcludeDirDto parm)
        {
            var expr = PredicateBuilder.True<SitePublicExcludeDir>();
            if (!parm.No.IsEmpty()) expr = expr.And(x => x.No == parm.No.Trim());
            if (!parm.PublicNo.IsEmpty()) expr = expr.And(x => x.PublicNo == parm.PublicNo.Trim());
            var source = Db.Queryable<SitePublicExcludeDir>().Where(expr).ToList();
            return source;
        }
        /// <summary>
        /// 查询 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<SitePublicExcludeDir> SitePublicExcludeDirQueryPages(SitePublicExcludeDirDto parm)
        {
            var expr = PredicateBuilder.True<SitePublicExcludeDir>();
            if (!parm.No.IsEmpty()) expr = expr.And(x => x.No == parm.No.Trim());
            if (!parm.PublicNo.IsEmpty()) expr = expr.And(x => x.PublicNo == parm.PublicNo.Trim());
            //if (!parm.Level.IsEmpty()) expr = expr.And(x => x.Level == parm.Level.Trim());
            //if (!parm.IPAddress.IsEmpty()) expr = expr.And(x => x.IPAddress == parm.IPAddress.Trim());
            //if (parm.CreateTime.HasValue) expr = expr.And(x => x.CreateTime >= parm.CreateTime.Value);
            //if (parm.CreateTime1.HasValue) expr = expr.And(x => x.CreateTime <= parm.CreateTime1.Value);
            var source = Db.Queryable<SitePublicExcludeDir>().Where(expr);
            parm.OrderBy = "Id";
            parm.Sort = "descending";
            return source.ToPage(new PageParm { Page = parm.Page, PageSize = parm.PageSize, OrderBy = parm.OrderBy, Sort = parm.Sort });
        }

        public List<SiteInfo> GetPublicDestSite(string publicno, int env)
        {
            var servernos = Db.Queryable<PublicToSite>().Where(x => x.PublicNo == publicno).Select(x => x.ServerNo).ToList();
            return Db.Queryable<SiteInfo>().Where(x => servernos.Contains(x.No) && x.Env == env).ToList();
        }

    }
}
