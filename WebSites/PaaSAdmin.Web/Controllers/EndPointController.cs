using PaaSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaaSAdmin.Web.Controllers
{
    public class EndPointController : ApiController
    {
        /// <summary>
        /// 上傳健康狀態的資訊
        /// </summary>
        [HttpPost]
        [ActionName("UploadHealth")]
        public BaseModels.BaseResult<bool> UploadHealth(EndPointModels.UploadHealthQuery query) => new BusinessRules.EndPoint().UploadHealth(query);
    }
}
