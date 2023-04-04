using Bc.Bussiness.IService;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.Dto;
using Bc.Model.Dto.System;
using System;

namespace Bc.Bussiness.Service
{
    public class ServerAppService : BaseService<ServerApp>, IServerAppService
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int Save(ServerApp info)
        {
            try
            {
                var temp = GetFirst(x => x.IP == info.IP && x.Port==info.Port);
                if (info.No.IsEmpty()) return 0;
                if (temp==null)
                {
                    info.UpdateTime = DateTime.Now;
                    return this.Add(info);
                }
                else
                { 
                    temp.ServerName = info.ServerName;
                    temp.IP = info.IP; 
                    temp.Port = info.Port;
                    temp.Env = info.Env;
                    temp.UpdateTime = DateTime.Now;  
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
        public PagedInfo<ServerApp> QueryPages(ServerAppDto parm)
        {
            var expr = PredicateBuilder.True<ServerApp>();
            if (!parm.No.IsEmpty()) expr = expr.And(x => x.No == parm.No.Trim());
            if (!parm.ServerName.IsEmpty()) expr = expr.And(x => x.ServerName == parm.ServerName.Trim()); 
            var source = Db.Queryable<ServerApp>().Where(expr);
            parm.OrderBy = "Id";
            parm.Sort = "descending";
            return source.ToPage(new PageParm { Page = parm.Page, PageSize = parm.PageSize, OrderBy = parm.OrderBy, Sort = parm.Sort });
        }

        public int Delete(int id)
        {
            return Db.Deleteable<ServerApp>(x => x.Id == id).ExecuteCommand();
        }

        public int Delete(string ip,string port)
        {
            return Db.Deleteable<ServerApp>(x => x.IP == ip && x.Port==port).ExecuteCommand();
        }


    }
}
