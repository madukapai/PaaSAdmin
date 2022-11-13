using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Models
{
    public class EndPointModels
    {
        /// <summary>
        /// 上送健康狀態的物件
        /// </summary>
        public class UploadHealthQuery
        {
            /// <summary>
            /// 處理器資訊
            /// </summary>
            public List<ProcessInfoItem> ProcessInfo { get; set; }
            /// <summary>
            /// 記憶體資訊
            /// </summary>
            public MemoryInfoItem MemoryInfo { get; set; }
            /// <summary>
            /// 磁碟機資訊
            /// </summary>
            public List<DiskInfoItem> DiskInfo { get; set; }
            /// <summary>
            /// 應用程式資訊
            /// </summary>
            public List<ApplicationInfoItem> ApplicationInfo { get; set; }

            /// <summary>
            /// 應用程式資訊
            /// </summary>
            public class ApplicationInfoItem
            {
                /// <summary>
                /// 應用程式的PID
                /// </summary>
                public int PID { get; set; }
                /// <summary>
                /// 應用程式集區名稱
                /// </summary>
                public string ApplicationPool { get; set; }
                /// <summary>
                /// CPU使用量
                /// </summary>
                public decimal ProcessUsage { get; set; }
                /// <summary>
                /// 記憶體使用量
                /// </summary>
                public decimal MemoryUsage { get; set; }
            }
            /// <summary>
            ///處理器資訊項目
            /// </summary>
            public class ProcessInfoItem
            {
                /// <summary>
                /// 處理器序號
                /// </summary>
                public string Core { get; set; }
                /// <summary>
                /// CPU使用的百分比
                /// </summary>
                public decimal Usage { get; set; }
            }

            /// <summary>
            /// 記憶體資訊物件
            /// </summary>
            public class MemoryInfoItem
            {
                /// <summary>
                /// 完整主機記憶體(MB)
                /// </summary>
                public decimal TotalGB { get; set; }
                /// <summary>
                /// 使用的記憶體大小(MB)
                /// </summary>
                public decimal UsageGB { get; set; }
                /// <summary>
                /// 已認可的記憶體大小(MB)
                /// </summary>
                public decimal CommitGB { get; set; }
                /// <summary>
                /// 未使用的記憶體大小(MB)
                /// </summary>
                public decimal FreeGB { get; set; }
            }

            /// <summary>
            /// 磁碟資訊
            /// </summary>
            public class DiskInfoItem
            {
                /// <summary>
                /// 實體名稱
                /// </summary>
                public string PhysicalName { get; set; }
                /// <summary>
                /// 磁區資訊
                /// </summary>
                public List<DriveItem> Drive { get; set; }
                /// <summary>
                /// 磁碟讀取資料量
                /// </summary>
                public decimal DiskReadMB { get; set; }
                /// <summary>
                /// 磁碟寫入資料量
                /// </summary>
                public decimal DiskWriteMB { get; set; }
                /// <summary>
                /// 磁區資訊
                /// </summary>
                public class DriveItem
                {
                    /// <summary>
                    /// 磁碟名稱
                    /// </summary>
                    public string DriveName { get; set; }
                    /// <summary>
                    /// 磁碟容量(GB)
                    /// </summary>
                    public decimal TotalGB { get; set; }
                    /// <summary>
                    /// 已使用大小(GB)
                    /// </summary>
                    public decimal UsageGB { get; set; }
                    /// <summary>
                    /// 可用容量(GB)
                    /// </summary>
                    public decimal FreeGB { get; set; }
                    /// <summary>
                    /// 磁碟機格式
                    /// </summary>
                    public string Format { get; set; }
                }
            }
        }
    }
}
