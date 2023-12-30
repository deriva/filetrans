using Bc.Bussiness.IService;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bc.Bussiness.Service
{
    public class PublicCmdService : BaseService<PublicCmd>, IPublicCmdService
    {


        /// <summary>
        ///  
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int Save(PublicCmd info, ref string msg)
        {
            try
            {
                info.Cmd=info.Cmd.Trim();
                if (info.Id == 0)
                {

                    return this.Db.Insertable<PublicCmd>(info).ExecuteCommand();
                }
                else
                {
                    return this.Db.Updateable<PublicCmd>(info).ExecuteCommand();
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return 0;

        }



        /// <summary>
        /// 查询发布指令列表页
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<PublicCmd> PublicCmdList(string no, string groupName)
        {
            var expr = PredicateBuilder.True<PublicCmd>();
            expr = expr.And(x => x.PublicNo == no);
            if (!groupName.IsEmpty()) expr = expr.And(x => x.GroupName == groupName);
            var source = Db.Queryable<PublicCmd>().Where(expr).ToList().OrderByDescending(x=>x.Sorter).ToList();
            return source;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            return Db.Deleteable<PublicCmd>(x => x.Id == id).ExecuteCommand();
        }

    }
}
