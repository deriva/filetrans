using Bc.Model;
using Bc.Model.Dto;
using Bc.Model.Dto.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Bussiness.IService
{
    public interface IServerAppService
    { 
        int Delete(int id);
        int Save(ServerApp info);
        PagedInfo<ServerApp> QueryPages(ServerAppDto parm);
        int Delete(string ip, string port);
    }
}
