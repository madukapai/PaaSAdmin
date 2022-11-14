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
    public class FtpSitesController : ApiController
    {
        /// <summary>
        /// 建立新的站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CreateFtpSites")]
        public BaseModels.BaseResult<bool> CreateFtpSites(FtpSitesModels.CreateFtpSitesQuery query) => new FtpSites().CreateFtpSites(query);

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ListFtpSites")]
        public BaseModels.PageResult<List<FtpSitesModels.ListFtpSitesResult>> ListFtpSites([FromUri]FtpSitesModels.ListFtpSitesQuery query) => new FtpSites().ListFtpSites(query);

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetFtpSites")]
        public BaseModels.BaseResult<FtpSitesModels.GetFtpSitesResult> GetFtpSites([FromUri]FtpSitesModels.GetFtpSitesQuery query) => new FtpSites().GetFtpSites(query);

        /// <summary>
        /// 刪除站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteFtpSites")]
        public BaseModels.BaseResult<bool> DeleteFtpSites(FtpSitesModels.DeleteFtpSitesQuery query) => new FtpSites().DeleteFtpSites(query);
    }
}
