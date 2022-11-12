using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Models
{
    public class ConfigModels
    {
        /// <summary>
        /// 取得設定值物件
        /// </summary>
        public class GetConfigValueQuery
        {
            /// <summary>
            /// 設定值名稱
            /// </summary>
            public string ConfigName { get; set; }
        }

        /// <summary>
        /// 取得設定值物件結果
        /// </summary>
        public class GetConfigValueResult
        {
            /// <summary>
            /// 設定值
            /// </summary>
            public string ConfigValue { get; set; }
        }
    }
}
