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
    public class ConfigController : ApiController
    {
        /// <summary>
        /// 取得設定值
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetConfig")]
        public BaseModels.BaseResult<ConfigModels.GetConfigValueResult> GetConfig([FromUri]ConfigModels.GetConfigValueQuery query) => new ConfigInfo().GetConfig(query);
    }
}
