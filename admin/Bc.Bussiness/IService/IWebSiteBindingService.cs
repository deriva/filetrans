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
    public interface IWebSiteBindingService
    { 
        int Delete(int id);
        int Save(WebSiteBinding info);
        PagedInfo<WebSiteBinding> QueryPages(WebSiteBindingDto parm);
    }
}
