//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
//     author MEIAM
// </auto-generated>
//------------------------------------------------------------------------------
using Bc.Model;
using Bc.Model.Dto;
using Bc.Common.Utilities;
using Bc.Bussiness.IService;
using SqlSugar;

namespace Bc.Bussiness.Service
{
    public class GroupInfoService : BaseService<GroupInfo>, IGroupInfoService
    {

        #region CustomInterface 
        /// <summary>   
        /// 查询（分页）
        /// </summary>  
        /// <param name=\"parm\"></param>  
        /// <returns></returns>   \r\n" +
        public PagedInfo<GroupInfo> QueryPages(GroupInfoDto parm)  
        {    
            var expr = Expressionable.Create<GroupInfo>(); 
        
            if (parm.ID.ToInt2()>0) expr = expr.And(x => x.ID==parm.ID);
if (!parm.GroupNo.IsEmpty()) expr = expr.And(x => x.GroupNo.Contains(parm.GroupNo));
if (!parm.GroupName.IsEmpty()) expr = expr.And(x => x.GroupName.Contains(parm.GroupName));
if (!parm.UserNo.IsEmpty()) expr = expr.And(x => x.UserNo.Contains(parm.UserNo));
if (!parm.ConfigNo.IsEmpty()) expr = expr.And(x => x.ConfigNo.Contains(parm.ConfigNo));

            var source = Db.Queryable<GroupInfo>().Where(expr.ToExpression());  
            parm.OrderBy = "Id";  
            parm.Sort = "descending";  
            return source.ToPage(new PageParm { Page = parm.Page, PageSize = parm.PageSize, OrderBy = parm.OrderBy, Sort = parm.Sort }); 
        }  
        #endregion

    }
}
