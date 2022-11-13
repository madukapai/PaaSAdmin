using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Models
{
    public class WebSitesModels
    {
        /// <summary>
        /// 建立站台的動作
        /// </summary>
        public class CreateWebSitesQuery : BaseModels.BaseAuth
        {
            /// <summary>
            /// 站台名稱
            /// </summary>
            public string WebSiteName { get; set; }
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// Port號
            /// </summary>
            public int Port { get; set; }
            /// <summary>
            /// IP
            /// </summary>
            public string IP { get; set; }
            /// <summary>
            /// 主機標題名稱
            /// </summary>
            public string Domain { get; set; }
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
            /// <summary>
            /// 使用記憶體上限
            /// </summary>
            public int MaxMemoryGB { get; set; }
            /// <summary>
            /// 使用的執行緒上限
            /// </summary>
            public int MaxInstance { get; set; }
            /// <summary>
            /// 回收分鐘數
            /// </summary>
            public int RecycleMinutes { get; set; }
            /// <summary>
            /// .NET版本
            /// </summary>
            public string RuntimeVersion { get; set; }
            /// <summary>
            /// 是否啟用32位元執行緒
            /// </summary>
            public bool IsEnable32Bit { get; set; }
        }

        /// <summary>
        /// 修改站台的動作
        /// </summary>
        public class UpdateWebSitesQuery : BaseModels.BaseAuth
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public string IISWebSitesId { get; set; }
            /// <summary>
            /// 站台名稱
            /// </summary>
            public string WebSiteName { get; set; }
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// Port號
            /// </summary>
            public int Port { get; set; }
            /// <summary>
            /// IP
            /// </summary>
            public string IP { get; set; }
            /// <summary>
            /// 主機標題名稱
            /// </summary>
            public string Domain { get; set; }
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
            /// <summary>
            /// 使用記憶體上限
            /// </summary>
            public int MaxMemoryGB { get; set; }
            /// <summary>
            /// 使用的執行緒上限
            /// </summary>
            public int MaxInstance { get; set; }
            /// <summary>
            /// 回收分鐘數
            /// </summary>
            public int RecycleMinutes { get; set; }
            /// <summary>
            /// .NET版本
            /// </summary>
            public string RuntimeVersion { get; set; }
            /// <summary>
            /// 是否啟用32位元執行緒
            /// </summary>
            public bool IsEnable32Bit { get; set; }
        }

        /// <summary>
        /// 查詢站台的動作
        /// </summary>
        public class ListWebSitesQuery : BaseModels.PageQuery
        {
            /// <summary>
            /// 站台名稱
            /// </summary>
            public string WebSiteName { get; set; }
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// Port號
            /// </summary>
            public int? Port { get; set; }
            /// <summary>
            /// IP
            /// </summary>
            public string IP { get; set; }
            /// <summary>
            /// 主機標題名稱
            /// </summary>
            public string Domain { get; set; }
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
            /// <summary>
            /// 使用記憶體上限
            /// </summary>
            public int? MaxMemoryGB { get; set; }
            /// <summary>
            /// 使用的執行緒上限
            /// </summary>
            public int? MaxInstance { get; set; }
            /// <summary>
            /// 回收分鐘數
            /// </summary>
            public int? RecycleMinutes { get; set; }
            /// <summary>
            /// .NET版本
            /// </summary>
            public string RuntimeVersion { get; set; }
            /// <summary>
            /// 是否啟用32位元執行緒
            /// </summary>
            public bool? IsEnable32Bit { get; set; }
        }

        /// <summary>
        /// 查詢站台的結果
        /// </summary>
        public class ListWebSitesResult
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public string IISWebSitesId { get; set; }
            /// <summary>
            /// 站台名稱
            /// </summary>
            public string WebSiteName { get; set; }
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// Port號
            /// </summary>
            public int Port { get; set; }
            /// <summary>
            /// IP
            /// </summary>
            public string IP { get; set; }
            /// <summary>
            /// 主機標題名稱
            /// </summary>
            public string Domain { get; set; }
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
            /// <summary>
            /// 使用記憶體上限
            /// </summary>
            public decimal MaxMemoryGB { get; set; }
            /// <summary>
            /// 使用的執行緒上限
            /// </summary>
            public int MaxInstance { get; set; }
            /// <summary>
            /// 回收分鐘數
            /// </summary>
            public int RecycleMinutes { get; set; }
            /// <summary>
            /// .NET版本
            /// </summary>
            public string RuntimeVersion { get; set; }
            /// <summary>
            /// 是否啟用32位元執行緒
            /// </summary>
            public bool IsEnable32Bit { get; set; }
        }

        /// <summary>
        /// 查詢站台的物件
        /// </summary>
        public class GetWebSitesQuery : BaseModels.BaseAuth
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public string IISWebSitesId { get; set; }
        }

        /// <summary>
        /// 查詢站台的結果
        /// </summary>
        public class GetWebSitesResult
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public string IISWebSitesId { get; set; }
            /// <summary>
            /// 站台名稱
            /// </summary>
            public string WebSiteName { get; set; }
            /// <summary>
            /// 對應產品
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// Port號
            /// </summary>
            public int Port { get; set; }
            /// <summary>
            /// IP
            /// </summary>
            public string IP { get; set; }
            /// <summary>
            /// 主機標題名稱
            /// </summary>
            public string Domain { get; set; }
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
            /// <summary>
            /// 使用記憶體上限
            /// </summary>
            public decimal MaxMemoryGB { get; set; }
            /// <summary>
            /// 使用的執行緒上限
            /// </summary>
            public int MaxInstance { get; set; }
            /// <summary>
            /// 回收分鐘數
            /// </summary>
            public int RecycleMinutes { get; set; }
            /// <summary>
            /// .NET版本
            /// </summary>
            public string RuntimeVersion { get; set; }
            /// <summary>
            /// 是否啟用32位元執行緒
            /// </summary>
            public bool IsEnable32Bit { get; set; }
        }

        /// <summary>
        /// 刪除站台的動作
        /// </summary>
        public class DeleteWebSitesQuery : BaseModels.BaseAuth
        {
            /// <summary>
            /// 站台代碼
            /// </summary>
            public List<string> IISWebSitesId { get; set; }
        }
    }
}
