using PaaSAdmin.BusinessRules.Utilities;
using PaaSAdmin.Common;
using PaaSAdmin.DataAccess;
using PaaSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.BusinessRules
{
    public class FtpSites
    {
        FtpUtility objFtp = new FtpUtility();
        Win32Utility objWin32 = new Win32Utility();
        PaaSAdminDbEntities db = new PaaSAdminDbEntities();
        string strFtpSiteRootPath = System.Configuration.ConfigurationManager.AppSettings["FtpSiteRootPath"].ToString();

        public FtpSites()
        {
            // 處理根目錄的字串
            if (strFtpSiteRootPath.Substring(strFtpSiteRootPath.Length - 1, 1) == @"\")
                strFtpSiteRootPath = strFtpSiteRootPath.Substring(0, strFtpSiteRootPath.Length - 1);
        }

        /// <summary>
        /// 建立新的站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<bool> CreateFtpSites(FtpSitesModels.CreateFtpSitesQuery query)
        {
            BaseModels.BaseResult<bool> result = new BaseModels.BaseResult<bool>();
            bool blCreateFtp = false, blCreateUser = false;

            // 確認虛擬目錄是否有重複
            bool blIsFtpExits = db.PaaSFtpSites.Where(x => x.UserName == query.UserName).Any();
            bool blIsWebExits = db.PaaSWebSites.Where(x=>x.UserName == query.UserName).Any();

            if (blIsFtpExits || blIsWebExits)
            {
                result.isSuccess = false;
                result.body = false;
                result.response = Code.Common.FtpAccountIsExits;
                return result;
            }

            // 實體路徑增加本機設定的根目錄
            string strSourcePhysicalPath = query.PhysicalPath;
            query.PhysicalPath = strFtpSiteRootPath + query.PhysicalPath;

            // 建立實體目錄
            System.IO.Directory.CreateDirectory(query.PhysicalPath);

            if (!string.IsNullOrEmpty(query.UserName) && !string.IsNullOrEmpty(query.UserPassword))
                blCreateUser = objWin32.CreateAccount(query.UserName, query.UserPassword, new List<string>() { "IIS_IUSRS" });

            // 建立Ftp站台
            if (blCreateUser)
            {
                WebSitesModels.CreateWebSitesQuery objQuery = new WebSitesModels.CreateWebSitesQuery()
                {
                    UserName = query.UserName,
                    PhysicalPath = query.PhysicalPath,
                    UserPassword = query.UserPassword,
                };

                blCreateFtp = objFtp.CreteVirtualDirectory(objQuery);
            }
            else
            {
                // 如果FTP建立失敗，就刪除帳號
                objWin32.DeleteAccount(query.UserName);
            }

            // 系統建立成功，寫入資料庫
            if (blCreateFtp)
            {
                PaaSFtpSites objData = new PaaSFtpSites()
                {
                    PhysicalPath = strSourcePhysicalPath,
                    Product = query.Product,
                    UserName = query.UserName,
                    UserPassword = query.UserPassword,
                    PaaSFtpSitesId = DbUtility.GetSequence("PaaSFtpSitesId"),
                };
                db.PaaSFtpSites.Add(objData);
                db.SaveChanges();
            }

            result.isSuccess = (blCreateFtp && blCreateUser);
            result.body = result.isSuccess;
            return result;
        }

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.PageResult<List<FtpSitesModels.ListFtpSitesResult>> ListFtpSites(FtpSitesModels.ListFtpSitesQuery query)
        {
            BaseModels.PageResult<List<FtpSitesModels.ListFtpSitesResult>> result = new BaseModels.PageResult<List<FtpSitesModels.ListFtpSitesResult>>()
            {
                Page = query.PageIndex,
                PageSize = query.PageSize
            };

            var data = db.PaaSFtpSites.AsQueryable();

            if (!string.IsNullOrEmpty(query.PhysicalPath))
                data = data.Where(x => x.PhysicalPath.Contains(query.PhysicalPath));

            if (!string.IsNullOrEmpty(query.Product))
                data = data.Where(x => x.Product.Contains(query.Product));

            if (!string.IsNullOrEmpty(query.UserName))
                data = data.Where(x => x.UserName.Contains(query.UserName));

            // 處理排序的動作
            if (!string.IsNullOrEmpty(query.Sort))
                data = data.OrderByField(query.Sort, query.SortExpress);

            // 處理分頁結束
            result.Records = data.Count();
            result.body = data.Skip(query.StartRowIndex)
                              .Take(query.PageSize)
                              .AsEnumerable()
                              .Select(c => new FtpSitesModels.ListFtpSitesResult()
                              {
                                  PhysicalPath = c.PhysicalPath,
                                  Product = c.Product,
                                  UserName = c.UserName,
                                  UserPassword = c.UserPassword,
                                  PaaSFtpSitesId = c.PaaSFtpSitesId.ToEncode(),
                              })
                              .ToList();
            return result;
        }

        /// <summary>
        /// 查詢站台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<FtpSitesModels.GetFtpSitesResult> GetFtpSites(FtpSitesModels.GetFtpSitesQuery query)
        {
            BaseModels.BaseResult<FtpSitesModels.GetFtpSitesResult> result = new BaseModels.BaseResult<FtpSitesModels.GetFtpSitesResult>();

            long intIISFtpSitesId = query.PaaSFtpSitesId.ToBigIntDecode();

            result.body = db.PaaSFtpSites.Where(x => x.PaaSFtpSitesId == intIISFtpSitesId)
                                        .AsEnumerable()
                                        .Select(c => new FtpSitesModels.GetFtpSitesResult()
                                        {
                                            PhysicalPath = c.PhysicalPath,
                                            Product = c.Product,
                                            UserName = c.UserName,
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
        public BaseModels.BaseResult<bool> DeleteFtpSites(FtpSitesModels.DeleteFtpSitesQuery query)
        {
            BaseModels.BaseResult<bool> result = new BaseModels.BaseResult<bool>();

            // 找出資料
            for (int i = 0; i < query.PaaSFtpSitesId.Count; i++)
            {
                long intIISFtpSitesId = query.PaaSFtpSitesId[i].ToBigIntDecode();
                var data = db.PaaSFtpSites.Where(x => x.PaaSFtpSitesId == intIISFtpSitesId).FirstOrDefault();

                if (data != null)
                {
                    // 刪除帳號
                    objWin32.DeleteAccount(data.UserName);

                    // 刪除Ftp
                    objFtp.DeleteVirtualDirectory(data.UserName);

                    // 刪除實體路徑
                    System.IO.Directory.Delete(strFtpSiteRootPath + data.PhysicalPath);

                    // 刪除資料
                    db.PaaSFtpSites.Remove(data);
                    db.SaveChanges();
                }
            }

            result.body = result.isSuccess;
            return result;
        }
    }
}
