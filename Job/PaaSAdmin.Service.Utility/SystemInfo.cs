using Microsoft.Web.Administration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Instrumentation;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaaSAdmin.Service.Utility
{
    public class SystemInfo
    {
        List<PerformanceCounter> objCPU, objDiskRead, objDiskWrite;
        Process proc;
        PerformanceCounter objAvailable, objCommit;

        /// <summary>
        /// 初始化的動作
        /// </summary>
        public SystemInfo()
        {
            // 建立Process的效能監視器
            objCPU = new List<PerformanceCounter>();
            for (int i = 0; i < Environment.ProcessorCount; i++)
                objCPU.Add(new PerformanceCounter("Processor", "% Processor Time", i.ToString(), true));
            objCPU.Add(new PerformanceCounter("Processor", "% Processor Time", "_Total", true));

            // 建立記憶體的效能監視器物件
            proc = Process.GetCurrentProcess();
            objAvailable = new PerformanceCounter("Memory", "Available MBytes", true);
            objCommit = new PerformanceCounter("Memory", "Committed Bytes", true);
        }

        /// <summary>
        /// 取得系統目前資訊
        /// </summary>
        public Models.SystemInfoModels.SystemInfoResult GetSystemInfo()
        {
            Models.SystemInfoModels.SystemInfoResult result = new Models.SystemInfoModels.SystemInfoResult();

            // 取得CPU的使用資訊
            result.ProcessInfo = this.ProcessDetect();

            // 取得記憶體資訊
            result.MemoryInfo = this.MemoryDetect();

            // 取得磁碟機的資訊
            result.DiskInfo = this.DiskDetect();

            // 取得應用程式的資訊
            result.ApplicationInfo = this.IISApplicationDetect();

            return result;
        }

        /// <summary>
        /// 探測CPU資訊並回傳資料
        /// </summary>
        /// <returns></returns>
        public List<Models.SystemInfoModels.SystemInfoResult.ProcessInfoItem> ProcessDetect()
        {
            // 放入CPU資訊並回傳
            List<Models.SystemInfoModels.SystemInfoResult.ProcessInfoItem> result = new List<Models.SystemInfoModels.SystemInfoResult.ProcessInfoItem>();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                result.Add(new Models.SystemInfoModels.SystemInfoResult.ProcessInfoItem()
                {
                    Core = i.ToString(),
                    Usage = (decimal)Math.Round(objCPU[i].NextValue(), 2),
                });
            }

            result.Add(new Models.SystemInfoModels.SystemInfoResult.ProcessInfoItem()
            {
                Core = "_Total",
                Usage = (decimal)Math.Round(objCPU[Environment.ProcessorCount].NextValue(), 2),
            });

            return result;
        }

        /// <summary>
        /// 探測記憶體資訊並回傳資料
        /// </summary>
        /// <param name="dtNow"></param>
        /// <param name="blWriteLog"></param>
        /// <returns></returns>
        public Models.SystemInfoModels.SystemInfoResult.MemoryInfoItem MemoryDetect()
        {
            decimal diUsage = Math.Round((decimal)proc.PrivateMemorySize64 / 1024 / 1024, 2);
            decimal diCommit = (decimal)Math.Round(objCommit.NextValue() / 1024 / 1024 / 1024, 2);
            decimal diFree = (decimal)Math.Round(objAvailable.NextValue() / 1024, 2);
            decimal diTotal = diCommit + diFree;

            Models.SystemInfoModels.SystemInfoResult.MemoryInfoItem result = new Models.SystemInfoModels.SystemInfoResult.MemoryInfoItem()
            {
                TotalGB = diTotal,
                UsageGB = diUsage,
                FreeGB = diFree,
                CommitGB = diCommit,
            };

            return result;
        }

        /// <summary>
        /// 取得磁碟機資訊
        /// </summary>
        /// <returns></returns>
        public List<Models.SystemInfoModels.SystemInfoResult.DiskInfoItem> DiskDetect()
        {
            List<Models.SystemInfoModels.SystemInfoResult.DiskInfoItem> result = new List<Models.SystemInfoModels.SystemInfoResult.DiskInfoItem>();

            // 判定是否要增加磁碟效能監視器
            bool blNeedCreatePerformanceCounter = false;
            if (objDiskRead == null || objDiskWrite == null)
            {
                blNeedCreatePerformanceCounter = true;
                objDiskRead = new List<PerformanceCounter>();
                objDiskWrite = new List<PerformanceCounter>();
            }


            // 加入磁碟機資訊
            var objDrives = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            int intIndex = 0;
            foreach (ManagementObject d in objDrives.Get())
            {
                // 先整理磁碟名稱與磁區代碼
                Models.SystemInfoModels.SystemInfoResult.DiskInfoItem objDisk = new Models.SystemInfoModels.SystemInfoResult.DiskInfoItem()
                {
                    PhysicalName = d.Properties["Name"].Value.ToString().Replace(@"\\.\PHYSICALDRIVE", ""),
                    Drive = new List<Models.SystemInfoModels.SystemInfoResult.DiskInfoItem.DriveItem>(),
                };

                // var deviceId = d.Properties["DeviceId"].Value;
                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText);
                
                // 處理個別磁區的資訊
                foreach (ManagementObject p in partitionQuery.Get())
                {
                    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath);
                    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                    foreach (ManagementObject ld in logicalDriveQuery.Get())
                    {
                        // 加入磁區
                        if (d.Properties["MediaType"].Value.ToString().ToLower() == "fixed hard disk media")
                        {
                            string strDriveName = ld.Properties["Name"].Value.ToString();
                            DriveInfo objDrive = new DriveInfo(strDriveName);

                            // 組合磁碟容量與使用空間大小
                            decimal diFreeGB = Math.Round((decimal)objDrive.TotalFreeSpace / 1024 / 1024 / 1024, 2);
                            decimal diTotalGB = Math.Round((decimal)objDrive.TotalSize / 1024 / 1024 / 1024, 2);
                            decimal diUsageGB = diTotalGB - diFreeGB;
                            objDisk.Drive.Add(new Models.SystemInfoModels.SystemInfoResult.DiskInfoItem.DriveItem()
                            {
                                DriveName = strDriveName,
                                FreeGB = diFreeGB,
                                TotalGB = diTotalGB,
                                UsageGB = diUsageGB,
                                Format = objDrive.DriveFormat,
                            });
                        }
                    }
                }

                // 處理磁碟讀取與寫入的資料量
                string strPhysicalDriveIndex = d.Properties["Name"].Value.ToString().Replace(@"\\.\PHYSICALDRIVE", "");
                string strPhysicalName = String.Join(" ", objDisk.Drive.Select(c => c.DriveName).ToArray());

                // 如果需要加入磁碟效能監視器
                if (blNeedCreatePerformanceCounter)
                {
                    objDiskRead.Add(new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", $"{strPhysicalDriveIndex} {strPhysicalName}", true));
                    objDiskWrite.Add(new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", $"{strPhysicalDriveIndex} {strPhysicalName}", true));
                }

                decimal diDiskMBRead = (decimal)Math.Round(objDiskRead[intIndex].NextValue() / 1024 / 1024, 2);
                decimal diDiskMBWrite = (decimal)Math.Round(objDiskRead[intIndex].NextValue() / 1024 / 1024, 2);

                objDisk.DiskReadMB = diDiskMBRead;
                objDisk.DiskWriteMB = diDiskMBWrite;

                result.Add(objDisk);

                intIndex += 1;
            }

            // 加入總使用量
            //decimal diDiskTotalMBRead = (decimal)Math.Round(new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total", true).NextValue() / 1024 / 1024, 2);
            //decimal diDiskTotalMBWrite = (decimal)Math.Round(new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total", true).NextValue() / 1024 / 1024, 2);

            return result;
        }

        /// <summary>
        /// 取得應用程式資訊
        /// </summary>
        /// <returns></returns>
        public List<Models.SystemInfoModels.SystemInfoResult.ApplicationInfoItem> IISApplicationDetect()
        {
            // 先取得IIS的應用程式集區名稱以及PID的對應
            Dictionary<int, string> DicProcessName = new Dictionary<int, string>();
            List<int> intProcessId = new List<int>();
            ServerManager serverManager = new ServerManager();
            foreach (WorkerProcess workerProcess in serverManager.WorkerProcesses)
            {
                Process toMonitor = Process.GetProcessById(workerProcess.ProcessId);
                DicProcessName.Add(workerProcess.ProcessId, workerProcess.AppPoolName);
                intProcessId.Add(workerProcess.ProcessId);
            }

            // 取得應用程式集區CPU與記憶體的使用量
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_PerfProc_Process");
            var objProcesses = searcher.Get().Cast<ManagementObject>().ToList();

            var result = objProcesses.Select(x => new Models.SystemInfoModels.SystemInfoResult.ApplicationInfoItem()
            {
                ApplicationPool = x.GetPropertyValue("Name").ToString(),
                PID = int.Parse(x.GetPropertyValue("IDProcess").ToString()),
                ProcessUsage = decimal.Parse(x.GetPropertyValue("PercentProcessorTime").ToString()),
                MemoryUsage = Math.Round(decimal.Parse(x.GetPropertyValue("WorkingSetPrivate").ToString()) / 1024 / 1024, 2),
            })
            .Where(x => intProcessId.Contains(x.PID))
            .ToList();

            result.ForEach(x => { x.ApplicationPool = DicProcessName[x.PID]; });

            return result;
        }

        /// <summary>
        /// 取得IP清單
        /// </summary>
        /// <returns></returns>
        public List<string> GetIPList()
        {
            List<string> strIPList = new List<string>();

            String strHostName = Dns.GetHostName();
            IPHostEntry objHostEntry = Dns.GetHostEntry(strHostName);

            strIPList = objHostEntry.AddressList.Select(c=>c.MapToIPv4().ToString()).ToList();

            return strIPList;
        }

        /// <summary>
        /// 取得MAC與IP清單
        /// </summary>
        /// <returns></returns>
        public void GetInterfaceInfo(out List<string> strIP, out List<string> strMAC)
        {
            strIP = new List<string>();
            strMAC = new List<string>();

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var nic in nics)
            {
                // 因為電腦中可能有很多的網卡(包含虛擬的網卡)，
                // 我只需要 Ethernet 網卡的 MAC
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    strMAC.Add(nic.GetPhysicalAddress().ToString());

                    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            strIP.Add(ip.Address.ToString());
                        }
                    }
                }
            }
        }
    }
}
