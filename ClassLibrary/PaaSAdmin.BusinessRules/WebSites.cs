using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaaSAdmin.Code;
using PaaSAdmin.Models;
using PaaSAdmin.BusinessRules.Utilities;
using PaaSAdmin.DataAccess;

namespace PaaSAdmin.BusinessRules
{
    public class WebSites
    {
        IISUtility objIIS = new IISUtility();
        FtpUtility objFtp = new FtpUtility();
        Win32Utility objWin32 = new Win32Utility();
        PaaSAdminDbEntities db = new PaaSAdminDbEntities();

        /// <summary>
        /// 建立新的站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<bool> CreateWebSites(WebSitesModels.CreateWebSitesQuery query)
        {
            BaseModels.BaseResult<bool> result = new BaseModels.BaseResult<bool>();
            bool blCreateIIS = false, blCreateFtp = false, blCreateUser = false;

            // 建立實體目錄
            System.IO.Directory.CreateDirectory(query.PhysicalPath);

            if (!string.IsNullOrEmpty(query.UserName) && !string.IsNullOrEmpty(query.UserPassword))
                blCreateUser = objWin32.CreateAccount(query.UserName, query.UserPassword, new List<string>() { "IIS_IUSRS" });

            // 建立IIS站台
            if (blCreateUser)
                blCreateIIS = objIIS.CreateWebSite(query);
            else
                objWin32.DeleteAccount(query.UserName);

            // 建立Ftp站台
            if (blCreateIIS)
                blCreateFtp = objFtp.CreteVirtualDirectory(query);
            else
            {
                // 如果FTP建立失敗，就刪除IIS站台與帳號
                objWin32.DeleteAccount(query.UserName);
                objFtp.DeleteVirtualDirectory(query.UserName);
            }

            // 系統建立成功，寫入資料庫
            if (blCreateFtp)
            {
                PaaSWebSites objData = new PaaSWebSites()
                {
                    Domain = query.Domain,
                    IISWebSitesId = DbUtility.GetSequence("IISWebSitesId"),
                    IP = query.IP,
                    IsEnable32Bit = query.IsEnable32Bit,
                    MaxInstance = query.MaxInstance,
                    MaxMemoryGB = query.MaxMemoryGB,
                    PhysicalPath = query.PhysicalPath,
                    Port = query.Port,
                    Product = query.Product,
                    RecycleMinutes = query.RecycleMinutes,
                    RuntimeVersion = query.RuntimeVersion,
                    UserName = query.UserName,
                    UserPassword = query.UserPassword,
                    WebSiteName = query.WebSiteName,
                };
                db.PaaSWebSites.Add(objData);
                db.SaveChanges();
            }

            result.isSuccess = (blCreateFtp && blCreateIIS && blCreateUser);
            result.body = result.isSuccess;
            return result;
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

            //objIIS.DeleteWebSite(query.IISWebSitesId);
            //objFtp.DeleteVirtualDirectory(query.UserName);
            //objWin32.DeleteAccount(query.UserName);
        }
    }
}
