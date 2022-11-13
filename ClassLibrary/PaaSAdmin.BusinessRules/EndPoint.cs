using PaaSAdmin.DataAccess;
using PaaSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.BusinessRules
{
    public class EndPoint
    {
        PaaSAdminDbEntities db = new PaaSAdminDbEntities();

        /// <summary>
        /// 上傳健康狀態的資訊
        /// </summary>
        public BaseModels.BaseResult<bool> UploadHealth(EndPointModels.UploadHealthQuery query)
        {
            BaseModels.BaseResult<bool> result = new BaseModels.BaseResult<bool>();
            DateTime dtReportUtcDateTime = DateTime.UtcNow;

            #region // 寫入健康狀態回報主檔
            var objTotalProcessInfo = query.ProcessInfo.Where(x => x.Core == "_Total").FirstOrDefault();
            decimal diDiskFree = 0, diDiskTotal = 0;
            for (int p = 0; p < query.DiskInfo.Count; p++)
            {
                for (int d = 0; d < query.DiskInfo[p].Drive.Count; d++)
                {
                    diDiskFree += query.DiskInfo[p].Drive[d].FreeGB;
                    diDiskTotal += query.DiskInfo[p].Drive[d].TotalGB;
                }
            }

            long intPaaSDeviceHealthId = DbUtility.GetSequence("PaaSDeviceHealthCpuId");

            PaaSDeviceHealth objHealth = new PaaSDeviceHealth()
            {
                PaaSDeviceHealthId = intPaaSDeviceHealthId,
                ReportUtcDateTime = dtReportUtcDateTime,
                CpuUsagePercent = objTotalProcessInfo.Usage,
                DiskUsagePercent = ((diDiskTotal - diDiskFree) / diDiskTotal) * 100,
                MemoryTotalGB = query.MemoryInfo.TotalGB,
                MemoryUsageGB = (query.MemoryInfo.TotalGB - query.MemoryInfo.FreeGB),
                MemoryUsagePercent = ((query.MemoryInfo.TotalGB - query.MemoryInfo.FreeGB) / query.MemoryInfo.TotalGB) * 100,
            };
            db.PaaSDeviceHealth.Add(objHealth);
            #endregion

            #region // 寫入CPU資訊
            List<PaaSDeviceHealthCpu> objProcesses = query.ProcessInfo.Where(x => x.Core != "_Total").Select(c => new PaaSDeviceHealthCpu()
            {
                Core = int.Parse(c.Core),
                CpuUsagePercent = c.Usage,
                PaaSDeviceHealthCpuId = DbUtility.GetSequence("PaaSDeviceHealthCpuId"),
                PaaSDeviceHealthId = intPaaSDeviceHealthId,

            }).ToList();
            db.PaaSDeviceHealthCpu.AddRange(objProcesses);
            #endregion

            #region // 寫入磁碟使用
            List<PaaSDeviceHealthDisk> objDisks = query.DiskInfo.SelectMany(c => c.Drive, (c, Drive) => new PaaSDeviceHealthDisk()
            {
                DriveFormat = Drive.Format,
                DriveName = Drive.DriveName,
                FreeGB = Drive.FreeGB,
                PaaSDeviceHealthDiskId = DbUtility.GetSequence("PaaSDeviceHealthDiskId"),
                PaaSDeviceHealthId = intPaaSDeviceHealthId,
                PhysicalName = c.PhysicalName,
                TotalGB = Drive.TotalGB,
                UsageGB = Drive.UsageGB,
            }).ToList();
            db.PaaSDeviceHealthDisk.AddRange(objDisks);
            #endregion

            #region // 寫入記憶體
            PaaSDeviceHealthMemory objMemory = new PaaSDeviceHealthMemory()
            {
                PaaSDeviceHealthId = intPaaSDeviceHealthId,
                PaaSDeviceHealthMemoryId = DbUtility.GetSequence("PaaSDeviceHealthMemoryId"),
                TotalGB = query.MemoryInfo.TotalGB,
                UsageGB = query.MemoryInfo.TotalGB - query.MemoryInfo.FreeGB,
            };
            db.PaaSDeviceHealthMemory.Add(objMemory);
            #endregion

            #region // 寫入應用程式
            List<PaaSDeviceHealthApp> objApp = query.ApplicationInfo.Select(c => new PaaSDeviceHealthApp()
            {
                ApplicationPool = c.ApplicationPool,
                CpuUsagePercent = c.ProcessUsage,
                MemoryUsageMB = c.MemoryUsage,
                PaaSDeviceHealthAppId = DbUtility.GetSequence("PaaSDeviceHealthAppId"),
                PaaSDeviceHealthId = intPaaSDeviceHealthId,
                PID = c.PID,
            }).ToList();
            db.PaaSDeviceHealthApp.AddRange(objApp);
            #endregion

            db.SaveChanges();

            result.body = result.isSuccess;
            return result;
        }
    }
}
