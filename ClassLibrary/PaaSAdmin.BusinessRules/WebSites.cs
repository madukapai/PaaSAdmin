using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaaSAdmin.Code;
using PaaSAdmin.Models;

namespace PaaSAdmin.BusinessRules
{
    public class WebSites
    {
        /// <summary>
        /// 建立新的站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<bool> CreateWebSites(WebSitesModels.CreateWebSitesQuery query)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 修改站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<bool> UpdateWebSites(WebSitesModels.UpdateWebSitesQuery query)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.PageResult<List<WebSitesModels.ListWebSitesResult>> ListWebSites(WebSitesModels.ListWebSitesQuery query)
        {
            throw new System.NotImplementedException();

        }

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<WebSitesModels.GetWebSitesResult> GetWebSites(WebSitesModels.GetWebSitesQuery query)
        {
            throw new System.NotImplementedException();

        }

        /// <summary>
        /// 刪除站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<bool> DeleteWebSites(WebSitesModels.DeleteWebSitesQuery query)
        {
            throw new System.NotImplementedException();

        }
    }
}
