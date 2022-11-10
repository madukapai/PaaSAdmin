using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.DataAccess
{
    public class DbUtility
    {
        /// <summary>
        /// 取得序號的動作
        /// </summary>
        /// <param name="strSeqName"></param>
        /// <returns></returns>
        public static long GetSequence(string strSeqName)
        {
            long nextVal = 0;

            using (PaaSAdminDbEntities db = new PaaSAdminDbEntities())
            {
                var rawQuery = db.Database.SqlQuery<long>($"SELECT NEXT VALUE FOR {strSeqName};");
                var task = rawQuery.SingleAsync();
                nextVal = task.Result;
            }

            return nextVal;
        }
    }
}
