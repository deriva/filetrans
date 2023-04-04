using Aliyun.Acs.Core;
using Bc.Extensions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bc.Extensions
{
    public interface IAliyunSmsServices
    {
        CommonResponse TemplateSmsSend(TemplateSmsSendDto parm);
    }
}
