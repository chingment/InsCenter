using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lumos
{
    public static class CommonUtil
    {
        #region "获取Ip"
        /// <summary>
        /// 获取Ip
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public static string GetIP()
        {
            string userIP = "";
            try
            {
                HttpContext rq = HttpContext.Current;
                HttpRequest Request = HttpContext.Current.Request;
                // 如果使用代理，获取真实IP
                if (rq.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                {
                    userIP = rq.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    userIP = rq.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (userIP == null || userIP == "")
                {
                    userIP = rq.Request.UserHostAddress;
                }
            }
            catch
            {
                userIP = "error ip";
            }
            return userIP;

        }
        #endregion
    }
}
