using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaaSAdmin.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using static System.Net.WebRequestMethods;

namespace PaaSAdmin.Service.Utility
{
    public class Utility
    {
        SystemInfo objSystem = new SystemInfo();
        string strPaaSAdminWebSite = System.Configuration.ConfigurationManager.AppSettings["PaaSAdminWebSite"];

        /// <summary>
        /// 上送健康狀態的動作
        /// </summary>
        public bool UploadHealth()
        {
            // 取得系統狀態的資訊
            var objSystemInfo = objSystem.GetSystemInfo();

            // 放入上送Model
            EndPointModels.UploadHealthQuery objQuery = new EndPointModels.UploadHealthQuery();

            // 放入記憶體使用資訊
            objQuery.MemoryInfo = new EndPointModels.UploadHealthQuery.MemoryInfoItem()
            {
                CommitGB = objSystemInfo.MemoryInfo.CommitGB,
                FreeGB = objSystemInfo.MemoryInfo.FreeGB,
                TotalGB = objSystemInfo.MemoryInfo.TotalGB,
                UsageGB = objSystemInfo.MemoryInfo.UsageGB,
            };

            // 放入CPU資訊
            objQuery.ProcessInfo = objSystemInfo.ProcessInfo.Select(c => new EndPointModels.UploadHealthQuery.ProcessInfoItem()
                                                            {
                                                                Core = c.Core,
                                                                Usage = c.Usage,
                                                            })
                                                            .ToList();
            // 放入磁碟資訊
            objQuery.DiskInfo = objSystemInfo.DiskInfo.Select(c => new EndPointModels.UploadHealthQuery.DiskInfoItem()
                                                        {
                                                            DiskReadMB = c.DiskReadMB,
                                                            DiskWriteMB = c.DiskWriteMB,
                                                            Drive = c.Drive.Select(d => new EndPointModels.UploadHealthQuery.DiskInfoItem.DriveItem()
                                                            {
                                                                DriveName = d.DriveName,
                                                                Format = d.Format,
                                                                FreeGB = d.FreeGB,
                                                                TotalGB = d.TotalGB,
                                                                UsageGB = d.UsageGB,
                                                            }).ToList(),
                                                            PhysicalName = c.PhysicalName
                                                        })
                                                        .ToList();

            // 放入應用程式資訊
            objQuery.ApplicationInfo = objSystemInfo.ApplicationInfo.Select(c => new EndPointModels.UploadHealthQuery.ApplicationInfoItem()
                                                                    {
                                                                        ApplicationPool = c.ApplicationPool,
                                                                        MemoryUsage = c.MemoryUsage,
                                                                        PID = c.PID,
                                                                        ProcessUsage = c.ProcessUsage,
                                                                    })
                                                                    .ToList();
            // 上傳資訊
            string strUrl = strPaaSAdminWebSite + "/api/EndPoint/UploadHealth";
            string strResponse = Common.Utility.CallAPI(strUrl, "POST", JsonConvert.SerializeObject(objQuery), out System.Net.HttpStatusCode code);

            // 執行成功就更新Command的清單資訊
            bool blIsSuccess = true;
            if (code == System.Net.HttpStatusCode.OK)
            {
                BaseModels.BaseResponse<bool> objResponse = JsonConvert.DeserializeObject<BaseModels.BaseResponse<bool>>(strResponse);
                blIsSuccess = objResponse.isSuccess;
            }

            return blIsSuccess;
        }

    }
}
