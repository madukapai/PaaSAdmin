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
    public class AuthorizeController : ApiController
    {
        /// <summary>
        /// 登入帳號的動作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Login")]
        public BaseModels.BaseResult<AuthorizeModels.LoginResult> Login(AuthorizeModels.LoginQuery query) => new Autorize().Login(query);
    }
}
