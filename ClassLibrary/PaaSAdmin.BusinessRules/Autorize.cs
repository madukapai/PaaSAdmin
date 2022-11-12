using PaaSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.BusinessRules
{
    public class Autorize
    {
        /// <summary>
        /// 登入帳號的動作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<AuthorizeModels.LoginResult> Login(AuthorizeModels.LoginQuery query)
        {
            BaseModels.BaseResult<AuthorizeModels.LoginResult> result = new BaseModels.BaseResult<AuthorizeModels.LoginResult>();

            // TODO:登入帳號部份尚待處理
            result.body = new AuthorizeModels.LoginResult()
            {
                Token = "",
                UserId = "",
                Username = "",
            };


            return result;
        }
    }
}
