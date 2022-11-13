using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaaSAdmin.Code;
using PaaSAdmin.Models;
using PaaSAdmin.BusinessRules.Utilities;
using PaaSAdmin.DataAccess;
using PaaSAdmin.Common;

namespace PaaSAdmin.BusinessRules
{
    public class WebSites
    {
        IISUtility objIIS = new IISUtility();
        FtpUtility objFtp = new FtpUtility();
        Win32Utility objWin32 = new Win32Utility();
        PaaSAdminDbEntities db = new PaaSAdminDbEntities();
        string strWebSiteRootPath = System.Configuration.ConfigurationManager.AppSettings["WebSiteRootPath"].ToString();

        public WebSites()
        {
            // 處理根目錄的字串
            if (strWebSiteRootPath.Substring(strWebSiteRootPath.Length - 1, 1) == @"\")
                strWebSiteRootPath = strWebSiteRootPath.Substring(0, strWebSiteRootPath.Length - 1);
        }

        /// <summary>
        /// 建立新的站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<bool> CreateWebSites(WebSitesModels.CreateWebSitesQuery query)
        {
            BaseModels.BaseResult<bool> result = new BaseModels.BaseResult<bool>();
            bool blCreateIIS = false, blCreateFtp = false, blCreateUser = false;

            // 實體路徑增加本機設定的根目錄
            string strSourcePhysicalPath = query.PhysicalPath;
            query.PhysicalPath = strWebSiteRootPath + query.PhysicalPath;

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
                    PhysicalPath = strSourcePhysicalPath,
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
            BaseModels.BaseResult<bool> result = new BaseModels.BaseResult<bool>();

            long intIISWebSitesId = query.IISWebSitesId.ToBigIntDecode();

            PaaSWebSites data = db.PaaSWebSites.Where(x => x.IISWebSitesId == intIISWebSitesId).FirstOrDefault();

            // 更新IIS設定
            bool blIsSuccess = objIIS.UpdateWebSite(query);

            // 寫入資料庫
            if (blIsSuccess)
            {
                data.Domain = query.Domain;
                data.IP = query.IP;
                data.Port = query.Port;
                data.IsEnable32Bit = query.IsEnable32Bit;
                data.MaxInstance = query.MaxInstance;
                data.MaxMemoryGB = query.MaxMemoryGB;
                data.RecycleMinutes = query.RecycleMinutes;
                data.RuntimeVersion = query.RuntimeVersion;

                db.SaveChanges();
            }

            result.isSuccess = blIsSuccess;
            result.body = result.isSuccess;
            return result;
        }

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.PageResult<List<WebSitesModels.ListWebSitesResult>> ListWebSites(WebSitesModels.ListWebSitesQuery query)
        {
            BaseModels.PageResult<List<WebSitesModels.ListWebSitesResult>> result = new BaseModels.PageResult<List<WebSitesModels.ListWebSitesResult>>()
            {
                Page = query.PageIndex,
                PageSize = query.PageSize
            };

            var data = db.PaaSWebSites.AsQueryable();

            if (!string.IsNullOrEmpty(query.IP))
                data = data.Where(x => x.IP.Contains(query.IP));

            if (!string.IsNullOrEmpty(query.Domain))
                data = data.Where(x => x.Domain.Contains(query.Domain));

            if (query.IsEnable32Bit.HasValue)
                data = data.Where(x => x.IsEnable32Bit == query.IsEnable32Bit);

            if (query.MaxInstance.HasValue)
                data = data.Where(x => x.MaxInstance == query.MaxInstance);

            if (query.MaxMemoryGB.HasValue)
                data = data.Where(x => x.MaxMemoryGB == query.MaxMemoryGB);

            if (!string.IsNullOrEmpty(query.PhysicalPath))
                data = data.Where(x => x.PhysicalPath.Contains(query.PhysicalPath));

            if (query.Port.HasValue)
                data = data.Where(x => x.Port == query.Port);

            if (!string.IsNullOrEmpty(query.Product))
                data = data.Where(x => x.Product.Contains(query.Product));

            if (query.RecycleMinutes.HasValue)
                data = data.Where(x => x.RecycleMinutes == query.RecycleMinutes);

            if (!string.IsNullOrEmpty(query.RuntimeVersion))
                data = data.Where(x => x.RuntimeVersion.Contains(query.RuntimeVersion));

            if (!string.IsNullOrEmpty(query.UserName))
                data = data.Where(x => x.UserName.Contains(query.UserName));

            if (!string.IsNullOrEmpty(query.WebSiteName))
                data = data.Where(x => x.WebSiteName.Contains(query.WebSiteName));

            // 處理排序的動作
            if (!string.IsNullOrEmpty(query.Sort))
                data = data.OrderByField(query.Sort, query.SortExpress);

            // 處理分頁結束
            result.Records = data.Count();
            result.body = data.Skip(query.StartRowIndex)
                              .Take(query.PageSize)
                              .AsEnumerable()
                              .Select(c => new WebSitesModels.ListWebSitesResult()
                              {
                                  Domain = c.Domain,
                                  IISWebSitesId = c.IISWebSitesId.ToEncode(),
                                  IP = c.IP,
                                  IsEnable32Bit = c.IsEnable32Bit,
                                  MaxInstance = c.MaxInstance,
                                  MaxMemoryGB = c.MaxMemoryGB,
                                  PhysicalPath = c.PhysicalPath,
                                  Port = c.Port,
                                  Product = c.Product,
                                  RecycleMinutes = c.RecycleMinutes,
                                  RuntimeVersion = c.RuntimeVersion,
                                  UserName = c.UserName,
                                  WebSiteName = c.WebSiteName,
                              })
                              .ToList();
            return result;
        }

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<WebSitesModels.GetWebSitesResult> GetWebSites(WebSitesModels.GetWebSitesQuery query)
        {
            BaseModels.BaseResult<WebSitesModels.GetWebSitesResult> result = new BaseModels.BaseResult<WebSitesModels.GetWebSitesResult>();

            long intIISWebSitesId = query.IISWebSitesId.ToBigIntDecode();

            result.body = db.PaaSWebSites.Where(x => x.IISWebSitesId == intIISWebSitesId)
                                        .AsEnumerable()
                                        .Select(c => new WebSitesModels.GetWebSitesResult()
                                        {
                                            Domain = c.Domain,
                                            IISWebSitesId = c.IISWebSitesId.ToEncode(),
                                            IP = c.IP,
                                            IsEnable32Bit = c.IsEnable32Bit,
                                            MaxInstance = c.MaxInstance,
                                            MaxMemoryGB = c.MaxMemoryGB,
                                            PhysicalPath = c.PhysicalPath,
                                            Port = c.Port,
                                            Product = c.Product,
                                            RecycleMinutes = c.RecycleMinutes,
                                            RuntimeVersion = c.RuntimeVersion,
                                            UserName = c.UserName,
                                            WebSiteName = c.WebSiteName,
                                            UserPassword = c.UserPassword,
                                        })
                                        .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 刪除站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<bool> DeleteWebSites(WebSitesModels.DeleteWebSitesQuery query)
        {
            BaseModels.BaseResult<bool> result = new BaseModels.BaseResult<bool>();

            // 找出資料
            for (int i = 0; i < query.IISWebSitesId.Count; i++)
            {
                long intIISWebSitesId = query.IISWebSitesId[i].ToBigIntDecode();
                var data = db.PaaSWebSites.Where(x => x.IISWebSitesId == intIISWebSitesId).FirstOrDefault();

                if (data != null)
                {
                    // 刪除帳號
                    objWin32.DeleteAccount(data.UserName);

                    // 刪除站台
                    objIIS.DeleteWebSite(data.WebSiteName);

                    // 刪除Ftp
                    objFtp.DeleteVirtualDirectory(data.UserName);

                    // 刪除實體路徑
                    System.IO.Directory.Delete(strWebSiteRootPath + data.PhysicalPath);

                    // 刪除資料
                    db.PaaSWebSites.Remove(data);
                    db.SaveChanges();
                }
            }

            result.body = result.isSuccess;
            return result;
        }
    }
}
