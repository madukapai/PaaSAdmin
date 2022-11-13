using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Service
{
    public partial class ServiceProcess : ServiceBase
    {
        private System.Timers.Timer tiProcess;
        private int intMinuteSecond = 60000;
        private Utility.Utility objUtility;

        public ServiceProcess()
        {
            InitializeComponent();

            objUtility = new Utility.Utility();

            this.tiProcess = new System.Timers.Timer(1 * intMinuteSecond);
            this.tiProcess.AutoReset = true;
            this.tiProcess.Elapsed += new System.Timers.ElapsedEventHandler(this.tiProcess_Tick);
        }

        /// <summary>
        /// 每一分鐘Timer執行的動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tiProcess_Tick(object sender, EventArgs e)
        {
            // 上送裝置健康狀態
            bool blIsSuccess = objUtility.UploadHealth();

            // 如果沒成功就停止服務
            if (!blIsSuccess)
                this.Stop();
        }

        /// <summary>
        /// 啟動的動作
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            tiProcess.Enabled = true;
            this.tiProcess.Start();
        }

        /// <summary>
        /// 停止服務的動作
        /// </summary>
        protected override void OnStop()
        {
            tiProcess.Enabled = false;
            this.tiProcess.Stop();
        }
    }
}
