using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Models
{
    public class AuthorizeModels
    {
        /// <summary>
        /// 登入物件
        /// </summary>
        public class LoginQuery
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        /// <summary>
        /// 登入結果物件
        /// </summary>
        public class LoginResult
        {
            /// <summary>
            /// 使用者代碼
            /// </summary>
            public string UserId { get; set; }
            /// <summary>
            /// Token
            /// </summary>
            public string Token { get; set; }
            /// <summary>
            /// 登入者姓名
            /// </summary>
            public string Username { get; set; }
        }
    }
}
