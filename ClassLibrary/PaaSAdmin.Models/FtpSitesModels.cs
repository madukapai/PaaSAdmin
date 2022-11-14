using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Models
{
    public class FtpSitesModels
    {
        /// <summary>
        /// 建立站台的動作
        /// </summary>
        public class CreateFtpSitesQuery : BaseModels.BaseAuth
        {
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// 自訂執行帳號
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 自動執行密碼
            /// </summary>
            public string UserPassword { get; set; }
            /// <summary>
            /// 站台實體路徑
            /// </summary>
            public string PhysicalPath { get; set; }
        }

        /// <summary>
        /// 修改站台的動作
        /// </summary>
        public class UpdateFtpSitesQuery : BaseModels.BaseAuth
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public string PaaSFtpSitesId { get; set; }
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// 自訂執行帳號
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 自動執行密碼
            /// </summary>
            public string UserPassword { get; set; }
            /// <summary>
            /// 站台實體路徑
            /// </summary>
            public string PhysicalPath { get; set; }
        }

        /// <summary>
        /// 查詢站台的動作
        /// </summary>
        public class ListFtpSitesQuery : BaseModels.PageQuery
        {
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// 自訂執行帳號
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 自動執行密碼
            /// </summary>
            public string UserPassword { get; set; }
            /// <summary>
            /// 站台實體路徑
            /// </summary>
            public string PhysicalPath { get; set; }
        }

        /// <summary>
        /// 查詢站台的結果
        /// </summary>
        public class ListFtpSitesResult
        {
            /// <summary>
            /// 站台名稱
            /// </summary>
            public string PaaSFtpSitesId { get; set; }
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// 自訂執行帳號
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 自動執行密碼
            /// </summary>
            public string UserPassword { get; set; }
            /// <summary>
            /// 站台實體路徑
            /// </summary>
            public string PhysicalPath { get; set; }
        }

        /// <summary>
        /// 查詢站台的物件
        /// </summary>
        public class GetFtpSitesQuery : BaseModels.BaseAuth
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public string PaaSFtpSitesId { get; set; }
        }

        /// <summary>
        /// 查詢站台的結果
        /// </summary>
        public class GetFtpSitesResult
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public string PaaSFtpSitesId { get; set; }
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// 自訂執行帳號
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 自動執行密碼
            /// </summary>
            public string UserPassword { get; set; }
            /// <summary>
            /// 站台實體路徑
            /// </summary>
            public string PhysicalPath { get; set; }
        }

        /// <summary>
        /// 刪除站台的動作
        /// </summary>
        public class DeleteFtpSitesQuery : BaseModels.BaseAuth
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public List<string> PaaSFtpSitesId { get; set; }
        }
    }
}
