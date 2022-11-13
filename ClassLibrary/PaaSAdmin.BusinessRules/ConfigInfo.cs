using PaaSAdmin.DataAccess;
using PaaSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.BusinessRules
{
    public class ConfigInfo
    {
        PaaSAdminDbEntities db = new PaaSAdminDbEntities();

        /// <summary>
        /// 取得設定值
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<ConfigModels.GetConfigValueResult> GetConfig(ConfigModels.GetConfigValueQuery query)
        {
            BaseModels.BaseResult<ConfigModels.GetConfigValueResult> result = new BaseModels.BaseResult<ConfigModels.GetConfigValueResult>();

            string strValue = db.PaaSConfig.Where(x => x.ConfigName == query.ConfigName).Select(c => c.ConfigValue).FirstOrDefault() ?? "";

            result.body = new ConfigModels.GetConfigValueResult() { ConfigValue = strValue };

            return result;
        }
    }
}
