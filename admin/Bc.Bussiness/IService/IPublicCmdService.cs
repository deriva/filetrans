using Bc.Common.Utilities;
using Bc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Bussiness.IService
{
    public interface IPublicCmdService
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        int Save(PublicCmd info, ref string msg);



        /// <summary>
        /// 查询发布指令列表页
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        List<PublicCmd> PublicCmdList(string no, string groupName);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        int Delete(int id);
    }
}
