using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Models
{
    /// <summary>
    /// 共用模型物件
    /// </summary>
    public class BaseModels
    {
        /// <summary>
        /// 登入帳號的底層物件
        /// </summary>
        public class BaseAuth
        {

        }

        /// <summary>
        /// WebAPI所使用的回傳模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class BaseResult<T>
        {
            /// <summary>
            /// WebAPI所使用的回傳模型
            /// </summary>
            public BaseResult()
            {
                this.isSuccess = true;
                this.response = Code.Common.Success;
            }

            Enum _responseObject;

            /// <summary>
            /// 回傳內容物件
            /// </summary>
            public T body { get; set; }
            /// <summary>
            /// 執行是否成功
            /// </summary>
            public bool isSuccess { get; set; }
            /// <summary>
            /// 回傳訊息
            /// </summary>
            public string message => this.response.ToString();
            /// <summary>
            /// 執行結果代碼
            /// </summary>
            public string code => Convert.ToInt32(_responseObject).ToString().PadLeft(3, '0');
            /// <summary>
            /// 回應物件
            /// </summary>
            public Enum response { set { _responseObject = value; } get { return _responseObject; } }
        }

        /// <summary>
        /// WebAPI所使用的回傳模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class BaseResponse<T>
        {
            /// <summary>
            /// WebAPI所使用的回傳模型
            /// </summary>
            public BaseResponse()
            {
                this.isSuccess = true;
            }

            /// <summary>
            /// 回傳內容物件
            /// </summary>
            public T body { get; set; }
            /// <summary>
            /// 執行是否成功
            /// </summary>
            public bool isSuccess { get; set; }
            /// <summary>
            /// 回傳訊息
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 執行結果代碼
            /// </summary>
            public string code { get; set; }
        }

        /// <summary>
        /// 用於ApiLog存取用的Herder模型物件
        /// </summary>
        public class HeaderObject
        {
            public string Key { get; set; }
            public string[] Value { get; set; }
        }

        /// <summary>
        /// 分页查询返回对象
        /// </summary>
        /// <typeparam name="T">Data</typeparam>
        public class PageResult<T> : BaseResult<T>
        {
            /// <summary>
            /// 總筆數
            /// </summary>
            public int Records { get; set; }

            /// <summary>
            /// 目前頁碼
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// 每頁顯示筆數
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// 總頁數
            /// </summary>
            public int Total
            {
                get
                {
                    double d1 = double.Parse(Records.ToString());
                    double d2 = double.Parse((PageSize <= 0 ? 20 : PageSize).ToString());
                    int t = Convert.ToInt32(Math.Ceiling(d1 / d2));
                    return t;
                }
            }
        }
        /// <summary>
        /// 分頁查詢
        /// </summary>
        public class PageQuery: BaseAuth
        {
            /// <summary>
            /// 目前頁碼
            /// </summary>
            public int PageIndex { get; set; }

            /// <summary>
            /// 每頁筆數
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// 起始筆數
            /// </summary>
            public int StartRowIndex { get { return (PageIndex - 1) * PageSize; } }

            /// <summary>
            /// 结束记录数
            /// </summary>
            public int EndRowIndex { get { return PageIndex * PageSize; } }

            /// <summary>
            /// 排序欄位
            /// </summary>
            public string Sort { get; set; }

            /// <summary>
            /// 排序方式
            /// </summary>
            public Code.Sort SortExpress { get; set; }
        }

        /// <summary>
        /// 分页查询返回对象
        /// </summary>
        /// <typeparam name="T">Data</typeparam>
        public class PageResponse<T> : BaseResponse<T>
        {
            /// <summary>
            /// 總筆數
            /// </summary>
            public int Records { get; set; }

            /// <summary>
            /// 目前頁碼
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// 每頁顯示筆數
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// 總頁數
            /// </summary>
            public int Total
            {
                get
                {
                    double d1 = double.Parse(Records.ToString());
                    double d2 = double.Parse((PageSize <= 0 ? 20 : PageSize).ToString());
                    int t = Convert.ToInt32(Math.Ceiling(d1 / d2));
                    return t;
                }
            }
        }
    }
}
