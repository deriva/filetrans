using Bc.Model;
using Bc.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Bussiness.IService
{
    public interface ISiteInfoService
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        int SaveSiteInfo(SiteInfo info);
        int Cancel(int id);
        /// <summary>
        /// 查询站点信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        PagedInfo<SiteInfo> QueryPages(SiteInfoDto parm);
        /// <summary>
        /// 查询站点信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
         SiteInfo GetSiteInfo(string no);
 
        /// <summary>
        ///  获取发布站点目录V2版本
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        PublicFileDirDto GetPublicFileDirByNoV2(string publicNo, int env);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        int SavePublicFileDir(PublicFileDir info);

        /// <summary>
        /// 查询 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        PagedInfo<PublicFileDir> PublicFileDirQueryPages(PublicFileDirDto parm);


        /// <summary>
        ///  
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        int SaveSitePublicExcludeDir(SitePublicExcludeDir info);

        /// <summary>
        /// 查询 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        PagedInfo<SitePublicExcludeDir> SitePublicExcludeDirQueryPages(SitePublicExcludeDirDto parm);
        /// <summary>
        /// 查询排除列表数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        List<SitePublicExcludeDir> SitePublicExcludeDirList(SitePublicExcludeDirDto parm);


        /// <summary>
        /// 查询发布目录对站点 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
         List<PublicToSite> PublicToSiteList(PublicToSite parm);
        /// <summary>
        /// 查询发布目录对站点 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
         int SavePublicToSite(PublicToSite parm);
        /// <summary>
        /// 查询发布目录对站点 （分页）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
         int DeletePublicToSite(PublicToSite parm);
    }
}
