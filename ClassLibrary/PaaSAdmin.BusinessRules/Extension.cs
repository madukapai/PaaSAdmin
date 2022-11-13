using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.BusinessRules
{
    internal static class Extension
    {
        /// <summary>
        /// 排序用的延伸方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">查詢物件</param>
        /// <param name="SortField">排序欄位</param>
        /// <param name="sort">排序方法</param>
        /// <returns></returns>
        internal static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, Code.Sort sort)
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
        internal static IList<T> OrderByField<T>(this IList<T> q, string SortField, Code.Sort sort)
        {
            var type = typeof(T);
            var sortProperty = type.GetProperty(SortField);

            if (sort == Code.Sort.Asc)
                return q.OrderBy(p => sortProperty.GetValue(p, null)).ToList();
            else
                return q.OrderByDescending(p => sortProperty.GetValue(p, null)).ToList();
        }
    }
}
