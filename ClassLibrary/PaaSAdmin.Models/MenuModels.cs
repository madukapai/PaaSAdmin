using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Models
{
    public class MenuModels
    {
        /// <summary>
        /// 系統選單
        /// </summary>
        public class ListSystemMenuResult
        {
            public List<SystemItem> ListSystem { get; set; }

            public class SystemItem : ListSystemResult
            {
                public List<MenuItem> ListFunc { get; set; }
            }

            public class MenuItem
            {
                public int FuncId { get; set; }
                public string SystemId { get; set; }
                public int ParentFuncId { get; set; }
                public string FuncName { get; set; }
                public string NavigateUrl { get; set; }
                public string DbConfig { get; set; }
                public string IconName { get; set; }
                public int Seq { get; set; }
                public string Description { get; set; }
                public List<MenuItem> ListFunc { get; set; }
            }
        }

        /// <summary>
        /// 可用系統的結果物件
        /// </summary>
        public class ListSystemResult
        {
            public string CompanyId { get; set; }
            public string SystemId { get; set; }
            public string SystemNo { get; set; }
            public string SystemName { get; set; }
            public string Description { get; set; }
        }
    }
}
