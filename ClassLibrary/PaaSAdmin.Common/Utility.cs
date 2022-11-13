using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PaaSAdmin.Common
{
    public class Utility
    {
        /// <summary>
        /// 呼叫API的動作
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strHttpMethod"></param>
        /// <param name="objHeader"></param>
        /// <param name="Content"></param>
        /// <param name="code"></param>
        /// <param name="strContentType"></param>
        /// <param name="objEncode"></param>
        /// <param name="intTimeoutSecond"></param>
        /// <returns></returns>
        public static Stream CallAPI(string strUrl, string strHttpMethod, Dictionary<string, string> objHeader, byte[] Content, out HttpStatusCode code, Encoding objEncode, string strContentType = "application/json; charset=utf-8", int intTimeoutSecond = 100)
        {
            System.GC.Collect();
            HttpWebRequest request = HttpWebRequest.Create(strUrl) as HttpWebRequest;
            request.Method = strHttpMethod;
            request.Timeout = intTimeoutSecond * 1000;

            foreach (var item in objHeader)
                request.Headers.Add(item.Key, item.Value);

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.Headers.AllKeys.Contains("FlowId"))
                    request.Headers.Add("FlowId", HttpContext.Current.Request.Headers.GetValues("FlowId").First());
            }

            code = HttpStatusCode.OK;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (Content != null)
            {
                request.KeepAlive = false;
                request.ContentType = strContentType;
                if (strHttpMethod != "GET")
                {
                    Stream reqStream = request.GetRequestStream();
                    reqStream.Write(Content, 0, Content.Length);
                }
            }

            Stream objStream = new MemoryStream();

            try
            {
                if (strUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                    SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (var respStream = response.GetResponseStream())
                    {
                        response.GetResponseStream().CopyTo(objStream);
                        objStream.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            catch (Exception e)
            {
                objStream = new MemoryStream(objEncode.GetBytes(e.Message));
                code = HttpStatusCode.NotFound;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }

            return objStream;
        }

        /// <summary>
        /// 呼叫API的動作
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strHttpMethod"></param>
        /// <param name="objHeader"></param>
        /// <param name="Content"></param>
        /// <param name="code"></param>
        /// <param name="strContentType"></param>
        /// <param name="intTimeoutSecond"></param>
        /// <returns></returns>
        public static string CallAPI(string strUrl, string strHttpMethod, Dictionary<string, string> objHeader, byte[] Content, out HttpStatusCode code, string strContentType = "application/json; charset=utf-8", int intTimeoutSecond = 100)
        {
            Stream objResult = CallAPI(strUrl, strHttpMethod, objHeader, Content, out code, Encoding.UTF8, strContentType, intTimeoutSecond);
            StreamReader reader = new StreamReader(objResult);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 呼叫API的動作
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strHttpMethod"></param>
        /// <param name="objHeader"></param>
        /// <param name="strContent"></param>
        /// <param name="code"></param>
        /// <param name="strContentType"></param>
        /// <param name="intTimeoutSecond"></param>
        /// <returns></returns>
        public static string CallAPI(string strUrl, string strHttpMethod, Dictionary<string, string> objHeader, string strContent, out HttpStatusCode code, string strContentType = "application/json; charset=utf-8", int intTimeoutSecond = 100) => CallAPI(strUrl, strHttpMethod, objHeader, Encoding.UTF8.GetBytes(strContent), out code, strContentType, intTimeoutSecond);

        /// <summary>
        /// 呼叫WebAPI的動作
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strHttpMethod"></param>
        /// <param name="Content"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CallAPI(string strUrl, string strHttpMethod, byte[] Content, out HttpStatusCode code) => CallAPI(strUrl, strHttpMethod, new Dictionary<string, string>(), Content, out code);

        /// <summary>
        /// 呼叫WebAPI的動作
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strHttpMethod"></param>
        /// <param name="Content"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CallAPI(string strUrl, string strHttpMethod, string Content, out HttpStatusCode code) => CallAPI(strUrl, strHttpMethod, new Dictionary<string, string>(), Encoding.UTF8.GetBytes(Content), out code);


    }
}
