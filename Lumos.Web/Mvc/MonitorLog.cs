﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lumos.Web.Mvc
{
    /// <summary>
    /// 监控日志对象
    /// </summary>
    public static class MonitorLog
    {
        //private static string GetPostData(Stream inputStream)
        //{
        //    string s = "";

        //    if (inputStream == null)
        //        return s;

        //    try
        //    {
        //        Stream stream = inputStream;
        //        stream.Seek(0, SeekOrigin.Begin);
        //        s = new StreamReader(stream).ReadToEnd();
        //    }
        //    catch
        //    {
        //        s = "";
        //    }

        //    return s;
        //}


        private static string GetPostData(Stream inputStream)
        {
            string s = "";

            if (inputStream == null)
                return s;

            try
            {
                Stream stream = inputStream;
                stream.Seek(0, SeekOrigin.Begin);
                s = new StreamReader(stream).ReadToEnd();
            }
            catch
            {
                s = "";
            }

            return s;
        }

        public static void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                Log(filterContext.RequestContext.HttpContext.Request);
            }
            catch (Exception ex)
            {
                LogUtil.Error("错误", ex);
            }
        }
        public static void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                Log(filterContext.RequestContext.HttpContext.Request, filterContext.Result);
            }
            catch (Exception ex)
            {
                LogUtil.Error("错误", ex);
            }
        }

        private static void Log(HttpRequestBase request, ActionResult result = null)
        {
            try
            {
                LogUtil.Info("Log Start");

                var sb = new StringBuilder();
                sb.Append("Url: " + request.RawUrl + Environment.NewLine);
                sb.Append("IP: " + CommonUtil.GetIP() + Environment.NewLine);
                sb.Append("Method: " + request.HttpMethod + Environment.NewLine);
                sb.Append("ContentType: " + request.ContentType + Environment.NewLine);
                sb.Append("UserAgent: " + request.UserAgent + Environment.NewLine);

                NameValueCollection headers = request.Headers;

                sb.Append("Header.CurrentUserId: " + headers["CurrentUserId"] + Environment.NewLine);


                if (request.HttpMethod == "POST")
                {
                    if (request.ContentType.IndexOf("multipart/form-data") < 0)
                    {
                        sb.Append("PostData: " + GetPostData(request.InputStream) + Environment.NewLine);
                    }
                }

                if (result != null)
                {
                    sb.Append("Response: " + result.ToString() + Environment.NewLine);
                }

                LogUtil.Info(sb.ToString());
                LogUtil.Info("Log End");
            }
            catch(Exception ex)
            {
                LogUtil.Error("错误",ex);
            }
        }

        //public static void OnResultExecuting(ResultExecutingContext filterContext)
        //{

        //}
        //public static void OnResultExecuted(string userId, ResultExecutedContext filterContext)
        //{

        //}
    }

}
