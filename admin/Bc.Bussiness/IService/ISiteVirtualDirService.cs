using Bc.Model;
using Bc.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Bussiness.IService
{
    public interface ISiteVirtualDirService
    { 
        int Delete(int id);
        int Save(SiteVirtualDir info);
        PagedInfo<SiteVirtualDir> QueryPages(SiteVirtualDirDto parm);
    }
}
