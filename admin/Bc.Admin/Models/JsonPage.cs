using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bc.Admin.Models
{
    public class JsonPage
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public object data { get; set; }
    }
}
