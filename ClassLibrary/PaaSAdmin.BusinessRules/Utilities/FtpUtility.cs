using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Web.Administration;
using PaaSAdmin.Models;

namespace PaaSAdmin.BusinessRules.Utilities
{
    internal class FtpUtility
    {
        string strFtpSiteName = ConfigurationManager.AppSettings["FtpSiteName"].ToString();
        ServerManager objServerMng = new ServerManager();

        /// <summary>
        /// 建立Ftp的目錄
        /// </summary>
        /// <param name="strDirectoryName">目錄名稱</param>
        /// <param name="strPhysicalPath">實體目錄</param>
        /// <param name="strUserName">登入帳號</param>
        /// <param name="strPassword">登入密碼</param>
        /// <returns></returns>
        internal bool CreteVirtualDirectory(WebSitesModels.CreateWebSitesQuery query)
        {
            bool blIsSuccess = true;

            // 建立目錄
            string strPathName = $"/{query.UserName}";
            var app = objServerMng.Sites[strFtpSiteName].Applications[0];
            app.VirtualDirectories.Add(strPathName, query.PhysicalPath);

            // 取出目錄並設定登入權限
            VirtualDirectory objFileDir = app.VirtualDirectories.Where(x => x.Path == strPathName).FirstOrDefault();
            objFileDir.LogonMethod = AuthenticationLogonMethod.Batch;
            objFileDir.UserName = query.UserName;
            objFileDir.Password = query.UserPassword;
            objServerMng.CommitChanges();

            return blIsSuccess;
        }

        /// <summary>
        /// 刪除Ftp的目錄
        /// </summary>
        /// <param name="strUserName">登入帳號名稱</param>
        /// <returns></returns>
        internal bool DeleteVirtualDirectory(string strUserName)
        {
            bool blIsSuccess = true;

            string strPathName = $"/{strUserName}";
            var app = objServerMng.Sites[strFtpSiteName].Applications[0];
            var objDir = app.VirtualDirectories.Where(x => x.Path == strPathName).FirstOrDefault();
            if (objDir != null)
            {
                app.VirtualDirectories.Remove(objDir);
                objServerMng.CommitChanges();
            }

            return blIsSuccess;
        }

        /// <summary>
        /// 修改Ftp的目錄
        /// </summary>
        /// <param name="strDirectoryName">目錄名稱</param>
        /// <param name="strPhysicalPath">實體目錄</param>
        /// <param name="strUserName">登入帳號</param>
        /// <param name="strPassword">登入密碼</param>
        /// <returns></returns>
        internal bool UpdateVirtualDirectory(WebSitesModels.UpdateWebSitesQuery query)
        {
            bool blIsSuccess = true;

            string strPathName = $"/{query.UserName}";
            var app = objServerMng.Sites[strFtpSiteName].Applications[0];
            var objDir = app.VirtualDirectories.Where(x => x.Path == strPathName).FirstOrDefault();
            if (objDir != null)
            {
                objDir.LogonMethod = AuthenticationLogonMethod.Batch;
                objDir.UserName = query.UserName;
                objDir.Password = query.UserPassword;
                objDir.PhysicalPath = query.PhysicalPath;

                objServerMng.CommitChanges();
            }

            return blIsSuccess;
        }
    }
}
