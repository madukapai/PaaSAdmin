using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Common
{
    public static class Extension
    {
        /// <summary>
        /// 排序用的延伸方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">查詢物件</param>
        /// <param name="SortField">排序欄位</param>
        /// <param name="sort">排序方法</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, Code.Sort sort)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = (sort == Code.Sort.Asc) ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        /// <summary>
        /// List物件排序用的延伸方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">查詢物件</param>
        /// <param name="SortField">排序欄位</param>
        /// <param name="sort">排序方法</param>
        /// <returns></returns>
        public static IList<T> OrderByField<T>(this IList<T> q, string SortField, Code.Sort sort)
        {
            var type = typeof(T);
            var sortProperty = type.GetProperty(SortField);

            if (sort == Code.Sort.Asc)
                return q.OrderBy(p => sortProperty.GetValue(p, null)).ToList();
            else
                return q.OrderByDescending(p => sortProperty.GetValue(p, null)).ToList();
        }


        static Dictionary<string, string> _dicDecode = new Dictionary<string, string>();
        static Dictionary<string, string> _dicEncode = new Dictionary<string, string>();
        private static object objDecodeLock = new object(), objEncodeLock = new object();

        static Dictionary<string, string> _dicAESDecode = new Dictionary<string, string>();
        static Dictionary<string, string> _dicAESEncode = new Dictionary<string, string>();
        private static object objAESDecodeLock = new object(), objAESEncodeLock = new object();

        #region // DESEncode
        /// <summary>
        /// 將數字進行DES編碼的動作
        /// </summary>
        /// <param name="intSource">來源數字</param>
        /// <returns></returns>
        public static string ToEncode(this int intSource) => GetEncode(intSource.ToString());

        /// <summary>
        /// 將數字進行DES編碼的動作
        /// </summary>
        /// <param name="longSource">來源數字</param>
        /// <returns></returns>
        public static string ToEncode(this long longSource) => GetEncode(longSource.ToString());

        /// <summary>
        /// 將字串進行DES編碼的動作
        /// </summary>
        /// <param name="strSource">來源字串</param>
        /// <returns></returns>
        public static string ToEncode(this string strSource) => GetEncode(strSource);

        /// <summary>
        /// 取得編碼後的結果
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private static string GetEncode(string strSource)
        {
            lock (objEncodeLock)
            {
                if (!_dicEncode.ContainsKey(strSource))
                {
                    try
                    {
                        _dicEncode.Add(strSource, new Encrypt().DESEncrypt(strSource));
                    }
                    catch
                    {

                    }
                }
            }

            return _dicEncode[strSource];
        }
        #endregion

        #region // DESDecode
        /// <summary>
        /// 將DES編碼的字串解碼為數字
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static int ToIntDecode(this string strSource) => int.Parse(GetDecode(strSource));

        /// <summary>
        /// 將DES編碼的字串解碼為數字
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static long ToBigIntDecode(this string strSource) => long.Parse(GetDecode(strSource));

        /// <summary>
        /// 將DES編碼的字串解碼為文字
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string ToDecode(this string strSource) => GetDecode(strSource);

        /// <summary>
        /// 取得解碼後的結果
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private static string GetDecode(string strSource)
        {
            lock (objDecodeLock)
            {
                if (!_dicDecode.ContainsKey(strSource))
                {
                    try
                    {
                        _dicDecode.Add(strSource, new Encrypt().DESDecrypt(strSource));
                    }
                    catch
                    {

                    }
                }
            }

            return _dicDecode[strSource];
        }

        /// <summary>
        /// 嘗試針對來源字串進行解碼的動作，若是成功則回傳True，失敗則為False
        /// </summary>
        /// <param name="strSource">來源字串</param>
        /// <returns></returns>
        public static bool TryDecode(this string strSource)
        {
            try
            {
                strSource.ToIntDecode();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 嘗試進行編碼後的字串進行解碼並轉換回數字
        /// </summary>
        /// <param name="strSource">來源編碼字串</param>
        /// <param name="intDecode">解碼成功後回傳的數字</param>
        /// <returns></returns>
        public static bool TryIntDecode(this string strSource, out int intDecode)
        {
            intDecode = -1;
            try
            {
                intDecode = strSource.ToIntDecode();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 將來源字串轉換成為Guid的格式
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static Guid ToGuidcode(this string strSource) => Guid.Parse(strSource);

        /// <summary>
        /// 將來源字串進行SHA512的編碼
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string ToSHAcode(this string strSource) => new Encrypt().SHA512Encrypt(strSource);

        /// <summary>
        /// 將來源字串進行AES的編碼
        /// </summary>
        /// <param name="strSource">來源字串</param>
        /// <param name="strKey">編碼用的KEY</param>
        /// <returns></returns>
        public static string ToAESEncode(this string strSource, string strKey)
        {
            lock (objAESEncodeLock)
            {
                if (string.IsNullOrEmpty(strSource))
                    return null;
                else
                {
                    string strDicKey = strSource + "####" + strKey;

                    if (!_dicAESEncode.ContainsKey(strDicKey))
                    {
                        try
                        {
                            _dicAESEncode.Add(strDicKey, new Encrypt().AESEncrypt(strSource, strKey));
                        }
                        catch
                        {

                        }
                    }

                    return _dicAESEncode[strDicKey];
                }
            }
        }

        /// <summary>
        /// 將來源字串進行AES的解碼
        /// </summary>
        /// <param name="strSource">來源字串</param>
        /// <param name="strKey">編碼用的KEY</param>
        /// <returns></returns>
        public static string ToAESDecode(this string strSource, string strKey)
        {
            lock (objAESDecodeLock)
            {
                if (string.IsNullOrEmpty(strSource))
                    return null;
                else
                {
                    string strDicKey = strSource + "####" + strKey;

                    if (!_dicAESDecode.ContainsKey(strDicKey))
                    {
                        try
                        {
                            _dicAESDecode.Add(strDicKey, new Encrypt().AESDecrypt(strSource, strKey));
                        }
                        catch
                        {

                        }
                    }

                    return _dicAESDecode[strDicKey];
                }
            }
        }

        /// <summary>
        /// 將指定的列舉型別物件轉換為數字代碼
        /// </summary>
        /// <param name="enumValue">列舉型別物件</param>
        /// <returns></returns>
        public static int ToInt(this Enum enumValue) => (int)((object)enumValue);

        /// <summary>
        /// 嘗試將數字轉換回列舉型別，並於輸出的參數中回傳
        /// </summary>
        /// <typeparam name="T">嘗試轉換的列舉型別物件</typeparam>
        /// <param name="intSource">要轉換的數字</param>
        /// <param name="result">嘗試轉換的結果</param>
        /// <returns></returns>
        public static bool TryToEnum<T>(this int intSource, out T result) where T : Enum
        {
            var enumType = typeof(T);
            var success = Enum.IsDefined(enumType, intSource);
            if (success)
            {
                result = (T)Enum.ToObject(enumType, intSource);
            }
            else
            {
                result = default(T);
            }
            return success;
        }

        /// <summary>
        /// 取靠右方指定字串的動作
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
                return s.Substring(s.Length - length, length);
            else
                return s;
        }

        /// <summary>
        /// 取靠左指定字串的動作
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string s, int length) => s.Substring(0, Math.Min(s.Length, length));
    }
}
