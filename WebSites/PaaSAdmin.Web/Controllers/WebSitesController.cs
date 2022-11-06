using PaaSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaaSAdmin.Web.Controllers
{
    public class WebSitesController : ApiController
    {
        /// <summary>
        /// 建立新的站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CreateWebSites")]
        public BaseModels.BaseResult<bool> CreateWebSites(WebSitesModels.CreateWebSitesQuery query) => new BusinessRules.WebSites().CreateWebSites(query);

        /// <summary>
        /// 修改站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("UpdateWebSites")]
        public BaseModels.BaseResult<bool> UpdateWebSites(WebSitesModels.UpdateWebSitesQuery query) => new BusinessRules.WebSites().UpdateWebSites(query);

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ListWebSites")]
        public BaseModels.PageResult<List<WebSitesModels.ListWebSitesResult>> ListWebSites([FromUri] WebSitesModels.ListWebSitesQuery query) => new BusinessRules.WebSites().ListWebSites(query);

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetWebSites")]
        public BaseModels.BaseResult<WebSitesModels.GetWebSitesResult> GetWebSites([FromUri] WebSitesModels.GetWebSitesQuery query) => new BusinessRules.WebSites().GetWebSites(query);

        /// <summary>
        /// 刪除站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteWebSites")]
        public BaseModels.BaseResult<bool> DeleteWebSites(WebSitesModels.DeleteWebSitesQuery query) => new BusinessRules.WebSites().DeleteWebSites(query);
    }
}
