using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaaSAdmin.Models;

namespace PaaSAdmin.BusinessRules
{
    public class MenuInfo
    {
        /// <summary>
        /// 列出系統選單物件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseModels.BaseResult<MenuModels.ListSystemMenuResult> ListSystemMenu(BaseModels.BaseAuth query)
        {
            BaseModels.BaseResult<MenuModels.ListSystemMenuResult> result = new BaseModels.BaseResult<MenuModels.ListSystemMenuResult>();

            // 定義系統與選單陣列
            List<MenuModels.ListSystemMenuResult.SystemItem> objSystem = new List<MenuModels.ListSystemMenuResult.SystemItem>()
            {
                // IIS站台管理
                new MenuModels.ListSystemMenuResult.SystemItem(){
                    SystemName = "PaaS環境管理",
                    ListFunc = new List<MenuModels.ListSystemMenuResult.MenuItem>()
                    {
                        new MenuModels.ListSystemMenuResult.MenuItem(){
                            FuncName = "應用程式管理", 
                            NavigateUrl = "", 
                            IconName = "fas fa-tasks", 
                            Seq = 1,
                            ListFunc = new List<MenuModels.ListSystemMenuResult.MenuItem>()
                            {
                                new MenuModels.ListSystemMenuResult.MenuItem(){FuncName = "IIS站台管理", NavigateUrl = "/Page/Application/WebSitesList.html", IconName = "fas fa-globe", Seq = 1 },
                                new MenuModels.ListSystemMenuResult.MenuItem(){FuncName = "FTP站台管理", NavigateUrl = "/Page/Application/FtpSitesList.html", IconName = "fas fa-file", Seq = 2 }
                            }
                        },

                        new MenuModels.ListSystemMenuResult.MenuItem(){
                            FuncName = "資料庫管理",
                            NavigateUrl = "",
                            IconName = "fas fa-server",
                            Seq = 2,
                            ListFunc = new List<MenuModels.ListSystemMenuResult.MenuItem>()
                            {
                                new MenuModels.ListSystemMenuResult.MenuItem(){FuncName = "Microsoft SQL Server", NavigateUrl = "/Page/Database/SQLList.html", IconName = "fas fa-database", Seq = 1 }
                            }
                        }
                    }
                },

                // 權限管理
                //new MenuModels.ListSystemMenuResult.SystemItem(){
                //    SystemName = "權限管理",
                //},

                // 系統設定與配置
                //new MenuModels.ListSystemMenuResult.SystemItem(){
                //    SystemName = "系統設定與配置",
                //},
            };

            // 放入系統選單
            result.body = new MenuModels.ListSystemMenuResult()
            {
                ListSystem = objSystem,
            };

            return result;
        }

    }
}
