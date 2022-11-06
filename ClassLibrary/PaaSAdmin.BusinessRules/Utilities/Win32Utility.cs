using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.BusinessRules.Utilities
{
    internal class Win32Utility
    {
        /// <summary>
        /// 建立帳號的動作
        /// </summary>
        /// <param name="strAccount">帳號名稱</param>
        /// <param name="strPassword">密碼</param>
        /// <param name="strGroups">加入群組</param>
        /// <returns></returns>
        internal bool CreateAccount(string strAccount, string strPassword, List<string> strGroups)
        {
            bool blIsSuccess = false;

            // 確認帳號是否存在
            string strADPath = "WinNT://" + Environment.MachineName;

            bool blAccountIsExits = false;
            using (var computerEntry = new DirectoryEntry(strADPath))
            {
                foreach (DirectoryEntry childEntry in computerEntry.Children)
                {
                    if (childEntry.SchemaClassName == "User")
                    {
                        if (childEntry.Name == strAccount)
                            blAccountIsExits = true;
                    }
                }
            }

            // 建立新帳號並指定群組
            if (!blAccountIsExits)
            {
                DirectoryEntry AD = new DirectoryEntry(strADPath + ",computer");
                DirectoryEntry NewUser = AD.Children.Add(strAccount, "user");
                NewUser.Invoke("SetPassword", new object[] { strPassword });
                NewUser.Properties["userFlags"].Value = 0x10000;
                NewUser.Invoke("Put", new object[] { "Description", "Create Account for IIS" });
                NewUser.CommitChanges();

                // 附加至群組中
                for (int i = 0; i < strGroups.Count; i++)
                {
                    DirectoryEntry objGroup = AD.Children.Find(strGroups[i], "group");
                    if (objGroup != null) { objGroup.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                }

                blIsSuccess = true;
            }

            return blIsSuccess;
        }
    }
}
