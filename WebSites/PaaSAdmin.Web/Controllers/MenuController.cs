using PaaSAdmin.BusinessRules;
using PaaSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaaSAdmin.Web.Controllers
{
    public class MenuController : ApiController
    {
        /// <summary>
        /// 透過登入帳號取的指定的系統以及選單資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ListSystemMenu")]
        public BaseModels.BaseResult<MenuModels.ListSystemMenuResult> ListSystemMenu([FromUri] BaseModels.BaseAuth query) => new MenuInfo().ListSystemMenu(query);

    }
}
