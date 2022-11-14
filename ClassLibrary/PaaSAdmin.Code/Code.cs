using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Code
{
    public enum Common
    {
        Success,
        Fail,
        FtpAccountIsExits,
        WebSiteIsExits,
        BindingIsExits,

        Exception,
    }

    public enum Sort
    {
        Asc,
        Desc,
    }
}
