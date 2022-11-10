using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using PaaSAdmin.Models;

namespace PaaSAdmin.BusinessRules.Utilities
{
    internal class IISUtility
    {
        ServerManager objServerMng = new ServerManager();
        Win32Utility objWin32 = new Win32Utility();

        /// <summary>
        /// 建立站台的動作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal bool CreateWebSite(WebSitesModels.CreateWebSitesQuery query)
        {
            bool blIsSuccess = true;
            bool blUseCustomAccount = false;

            #region // 建立應用程式集區
            bool blIsExits = objServerMng.ApplicationPools.Where(x => x.Name == query.WebSiteName).Any();

            if (!blIsExits)
            {
                ApplicationPool newPool = objServerMng.ApplicationPools.Add(query.WebSiteName);
                newPool.ManagedRuntimeVersion = query.RuntimeVersion;
                newPool.Enable32BitAppOnWin64 = query.IsEnable32Bit;
                newPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                // 停用失敗保護
                newPool.Failure.RapidFailProtectionMaxCrashes = 60;
                newPool.Failure.RapidFailProtection = false;

                // 設定閒置分鐘數
                newPool.ProcessModel.IdleTimeout = TimeSpan.FromMinutes(60);
                newPool.ProcessModel.IdleTimeoutAction = IdleTimeoutAction.Suspend;

                // 指定識別方式，並指定帳號與密碼
                if (blUseCustomAccount)
                {
                    newPool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
                    newPool.ProcessModel.UserName = query.UserName;
                    newPool.ProcessModel.Password = query.UserPassword;
                }
                else
                {
                    newPool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                }

                // 清除回收時間
                newPool.Recycling.PeriodicRestart.Time = TimeSpan.FromMinutes(query.RecycleMinutes);

                // 指定可使用的記憶體大小，上限為4G
                newPool.Recycling.PeriodicRestart.PrivateMemory = (1024 * 1024 * query.MaxMemoryGB);

                // 設定最大使用的執行緒
                newPool.ProcessModel.MaxProcesses = query.MaxInstance;

                objServerMng.CommitChanges();
            }
            #endregion

            #region // 建立站台
            string strWebsitename = query.WebSiteName;
            string strApplicationPool = query.WebSiteName;
            string strBindingInfo = $"{query.IP}:{query.Port}:{query.Domain}";
            string strPortocol = (query.Port == 443) ? "https" : "http";

            bool blIsExitsWebsite = IsWebsiteExists(strWebsitename);

            if (!blIsExitsWebsite)
            {
                Site mySite = objServerMng.Sites.Add(strWebsitename, strPortocol, strBindingInfo, query.PhysicalPath);
                mySite.ApplicationDefaults.ApplicationPoolName = strApplicationPool;
                objServerMng.CommitChanges();
            }
            #endregion

            return blIsSuccess;
        }

        /// <summary>
        /// 修改站台的動作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal bool UpdateWebSite(WebSitesModels.UpdateWebSitesQuery query)
        {
            bool blIsSuccess = false;
            bool blUseCustomAccount = false;

            #region // 建立自訂的執行帳號
            if (!string.IsNullOrEmpty(query.UserName) && !string.IsNullOrEmpty(query.UserPassword))
            {
                objWin32.CreateAccount(query.UserName, query.UserPassword, new List<string>() { "IIS_IUSRS" });
                blUseCustomAccount = true;
            }
            #endregion

            #region // 修改應用程式集區
            ApplicationPool objPool = objServerMng.ApplicationPools.Where(x => x.Name == query.WebSiteName).FirstOrDefault();

            if (objPool != null)
            {
                objPool.ManagedRuntimeVersion = query.RuntimeVersion;
                objPool.Enable32BitAppOnWin64 = query.IsEnable32Bit;
                objPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                // 停用失敗保護
                objPool.Failure.RapidFailProtectionMaxCrashes = 60;
                objPool.Failure.RapidFailProtection = false;

                // 設定閒置分鐘數
                objPool.ProcessModel.IdleTimeout = TimeSpan.FromMinutes(60);
                objPool.ProcessModel.IdleTimeoutAction = IdleTimeoutAction.Suspend;

                // 指定識別方式，並指定帳號與密碼
                if (blUseCustomAccount)
                {
                    objPool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
                    objPool.ProcessModel.UserName = query.UserName;
                    objPool.ProcessModel.Password = query.UserPassword;
                }
                else
                {
                    objPool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                }

                // 清除回收時間
                objPool.Recycling.PeriodicRestart.Time = TimeSpan.FromMinutes(query.RecycleMinutes);

                // 指定可使用的記憶體大小，上限為4G
                objPool.Recycling.PeriodicRestart.PrivateMemory = (1024 * 1024 * query.MaxMemoryGB);

                // 設定最大使用的執行緒
                objPool.ProcessModel.MaxProcesses = query.MaxInstance;

                objServerMng.CommitChanges();
            }
            #endregion

            #region // 修改IIS設定
            Site mySite = objServerMng.Sites.Where(x => x.Name == query.WebSiteName).FirstOrDefault();
            if (mySite != null)
            {
                string strBindingInfo = $"{query.IP}:{query.Port}:{query.Domain}";
                var objBinding = mySite.Bindings.FirstOrDefault();
                objBinding.Protocol = (query.Port == 443) ? "https" : "http";
                objBinding.BindingInformation = strBindingInfo;
            }
            #endregion

            return blIsSuccess;
        }

        /// <summary>
        /// 刪除站台的動作
        /// </summary>
        /// <param name="strWebSite"></param>
        /// <returns></returns>
        internal bool DeleteWebSite(string strWebSite)
        {
            bool blIsSuccess = true;

            objServerMng.ApplicationPools.Remove(objServerMng.ApplicationPools.Where(x => x.Name == strWebSite).FirstOrDefault());
            objServerMng.Sites.Remove(objServerMng.Sites.Where(x => x.Name == strWebSite).FirstOrDefault());

            return blIsSuccess;
        }

        /// <summary>
        /// 確認IIS站台是否存在
        /// </summary>
        /// <param name="strWebsitename"></param>
        /// <returns></returns>
        internal bool IsWebsiteExists(string strWebsitename)
        {
            Boolean flagset = false;
            SiteCollection sitecollection = objServerMng.Sites;
            foreach (Site site in sitecollection)
            {
                if (site.Name == strWebsitename.ToString())
                {
                    flagset = true;
                    break;
                }
                else
                {
                    flagset = false;
                }
            }
            return flagset;
        }
    }
}
