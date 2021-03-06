﻿using log4net;
using Lumos;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YsyInscarSdk
{
    /// <summary>
    /// 网络工具类。
    /// </summary>
    public sealed class WebUtils
    {
        private int _timeout = 60000;
        private int _readWriteTimeout = 60000;
        private bool _ignoreSSLCheck = true;

        /// <summary>
        /// 等待请求开始返回的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        /// <summary>
        /// 等待读取数据完成的超时时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return this._readWriteTimeout; }
            set { this._readWriteTimeout = value; }
        }

        /// <summary>
        /// 是否忽略SSL检查
        /// </summary>
        public bool IgnoreSSLCheck
        {
            get { return this._ignoreSSLCheck; }
            set { this._ignoreSSLCheck = value; }
        }

        ///// <summary>
        ///// 执行HTTP POST请求。
        ///// </summary>
        ///// <param name="url">请求地址</param>
        ///// <param name="textParams">请求文本参数</param>
        ///// <returns>HTTP响应</returns>
        //public string DoPost(string url,IDictionary<string, string> urlParams, IDictionary<string, string> textParams)
        //{
        //    return DoPost(url, urlParams,textParams, null);
        //}

        ///// <summary>
        ///// 执行HTTP POST请求。
        ///// </summary>
        ///// <param name="url">请求地址</param>
        ///// <param name="textParams">请求文本参数</param>
        ///// <param name="headerParams">请求头部参数</param>
        ///// <returns>HTTP响应</returns>
        //public string DoPost(string url, IDictionary<string, string> urlParams, IDictionary<string, string> textParams, IDictionary<string, string> headerParams)
        //{

        //    if (urlParams != null && urlParams.Count > 0)
        //    {
        //        url = BuildRequestUrl(url, urlParams);
        //    }

        //    HttpWebRequest req = GetWebRequest(url, "POST", headerParams);
        //    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

        //    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(textParams));
        //    System.IO.Stream reqStream = req.GetRequestStream();
        //    reqStream.Write(postData, 0, postData.Length);
        //    reqStream.Close();

        //    HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        //    Encoding encoding = GetResponseEncoding(rsp);
        //    return GetResponseAsString(rsp, encoding);
        //}


        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> urlParams, string postdata, IDictionary<string, string> headerParams)
        {

            if (urlParams != null && urlParams.Count > 0)
            {
                url = BuildRequestUrl(url, urlParams);
            }


            LogUtil.Info("Ydt-request-url>>>>" + url);
            if (postdata != null)
            {
                LogUtil.Info("Ydt-request-postData>>>>" + postdata);
            }

            HttpWebRequest req = GetWebRequest(url, "POST", headerParams);
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";


            byte[] postData = Encoding.UTF8.GetBytes(postdata);
            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            string result = GetResponseAsString(rsp, encoding);

            LogUtil.Info("Ydt-request-result>>>>" + result);

            return result;
        }

        public string DoPostFile(string url, IDictionary<string, string> urlParams, string fileName, Stream postData)
        {

            try
            {
                if (urlParams != null && urlParams.Count > 0)
                {
                    url = BuildRequestUrl(url, urlParams);
                }


                string boundary = DateTime.Now.Ticks.ToString("x");
                HttpWebRequest uploadRequest = (HttpWebRequest)WebRequest.Create(url);//url为上传的地址
                uploadRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                uploadRequest.Method = "POST";
                uploadRequest.Accept = "*/*";
                uploadRequest.KeepAlive = true;
                uploadRequest.Headers.Add("Accept-Language", "zh-cn");
                uploadRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                uploadRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;


                WebResponse reponse;
                //创建一个内存流
                Stream memStream = new MemoryStream();

                //确定上传的文件路径

                boundary = "--" + boundary;

                //添加上传文件参数格式边界
                string paramFormat = boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}\r\n";


                //添加上传文件数据格式边界
                string dataFormat = boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type:application/octet-stream\r\n\r\n";
                string header = string.Format(dataFormat, "Filedata", fileName);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                memStream.Write(headerbytes, 0, headerbytes.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = 0;

                //将文件内容写进内存流
                while ((bytesRead = postData.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }

                //添加文件结束边界
                byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n\n" + boundary + "\r\nContent-Disposition: form-data; name=\"Upload\"\r\n\nSubmit Query\r\n" + boundary + "--");
                memStream.Write(boundarybytes, 0, boundarybytes.Length);


                //设置请求长度
                uploadRequest.ContentLength = memStream.Length;

                //获取请求写入流
                Stream requestStream = uploadRequest.GetRequestStream();

                //将内存流数据读取位置归零
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();

                //将内存流中的buffer写入到请求写入流
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();


                reponse = uploadRequest.GetResponse();
                StreamReader reader = new StreamReader(reponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();




                return content;
            }
            catch(Exception ex)
            {
                LogUtil.Error("DoPostFile Ex", ex);

                return null; 
            }
        }



        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> textParams)
        {
            return DoGet(url, textParams, null);
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> textParams, IDictionary<string, string> headerParams)
        {
            if (textParams != null && textParams.Count > 0)
            {
                url = BuildRequestUrl(url, textParams);
            }

            LogUtil.Info("Ydt->url:" + url);

            HttpWebRequest req = GetWebRequest(url, "GET", headerParams);
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, encoding);
        }

        ///// <summary>
        ///// 执行带文件上传的HTTP POST请求。
        ///// </summary>
        ///// <param name="url">请求地址</param>
        ///// <param name="textParams">请求文本参数</param>
        ///// <param name="fileParams">请求文件参数</param>
        ///// <param name="headerParams">请求头部参数</param>
        ///// <returns>HTTP响应</returns>
        //public string DoPost(string url, IDictionary<string, string> urlParams, IDictionary<string, string> textParams, IDictionary<string, FileItem> fileParams, IDictionary<string, string> headerParams)
        //{
        //    // 如果没有文件参数，则走普通POST请求
        //    if (fileParams == null || fileParams.Count == 0)
        //    {
        //        return DoPost(url, textParams, headerParams);
        //    }

        //    string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线

        //    HttpWebRequest req = GetWebRequest(url, "POST", headerParams);
        //    req.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;

        //    System.IO.Stream reqStream = req.GetRequestStream();
        //    byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
        //    byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

        //    // 组装文本请求参数
        //    string textTemplate = "Content-Disposition:form-data;name=\"{0}\"\r\nContent-Type:text/plain\r\n\r\n{1}";
        //    foreach (KeyValuePair<string, string> kv in textParams)
        //    {
        //        string textEntry = string.Format(textTemplate, kv.Key, kv.Value);
        //        byte[] itemBytes = Encoding.UTF8.GetBytes(textEntry);
        //        reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
        //        reqStream.Write(itemBytes, 0, itemBytes.Length);
        //    }

        //    // 组装文件请求参数
        //    string fileTemplate = "Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
        //    foreach (KeyValuePair<string, FileItem> kv in fileParams)
        //    {
        //        string key = kv.Key;
        //        FileItem fileItem = kv.Value;
        //        string fileEntry = string.Format(fileTemplate, key, fileItem.GetFileName(), fileItem.GetMimeType());
        //        byte[] itemBytes = Encoding.UTF8.GetBytes(fileEntry);
        //        reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
        //        reqStream.Write(itemBytes, 0, itemBytes.Length);

        //        byte[] fileBytes = fileItem.GetContent();
        //        reqStream.Write(fileBytes, 0, fileBytes.Length);
        //    }

        //    reqStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
        //    reqStream.Close();

        //    HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        //    Encoding encoding = GetResponseEncoding(rsp);
        //    return GetResponseAsString(rsp, encoding);
        //}

        /// <summary>
        /// 执行带body体的POST请求。
        /// </summary>
        /// <param name="url">请求地址，含URL参数</param>
        /// <param name="body">请求body体字节流</param>
        /// <param name="contentType">body内容类型</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, byte[] body, string contentType, IDictionary<string, string> headerParams)
        {
            HttpWebRequest req = GetWebRequest(url, "POST", headerParams);
            req.ContentType = contentType;
            if (body != null)
            {
                System.IO.Stream reqStream = req.GetRequestStream();
                reqStream.Write(body, 0, body.Length);
                reqStream.Close();
            }
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, encoding);
        }

        public HttpWebRequest GetWebRequest(string url, string method, IDictionary<string, string> headerParams)
        {
            HttpWebRequest req = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                if (this._ignoreSSLCheck)
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(TrustAllValidationCallback);
                }
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }

            if (headerParams != null && headerParams.Count > 0)
            {
                foreach (string key in headerParams.Keys)
                {
                    req.Headers.Add(key, headerParams[key]);
                }
            }

            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "top-sdk-net";
            req.Accept = "text/xml,text/javascript";
            req.Timeout = this._timeout;
            req.ReadWriteTimeout = this._readWriteTimeout;

            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                if (Constants.CONTENT_ENCODING_GZIP.Equals(rsp.ContentEncoding, StringComparison.OrdinalIgnoreCase))
                {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                }
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// 组装含参数的请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数映射</param>
        /// <returns>带参数的请求URL</returns>
        public static string BuildRequestUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                return BuildRequestUrl(url, BuildQuery(parameters));
            }
            return url;
        }

        /// <summary>
        /// 组装含参数的请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queries">一个或多个经过URL编码后的请求参数串</param>
        /// <returns>带参数的请求URL</returns>
        public static string BuildRequestUrl(string url, params string[] queries)
        {
            if (queries == null || queries.Length == 0)
            {
                return url;
            }

            StringBuilder newUrl = new StringBuilder(url);
            bool hasQuery = url.Contains("?");
            bool hasPrepend = url.EndsWith("?") || url.EndsWith("&");

            foreach (string query in queries)
            {
                if (!string.IsNullOrEmpty(query))
                {
                    if (!hasPrepend)
                    {
                        if (hasQuery)
                        {
                            newUrl.Append("&");
                        }
                        else
                        {
                            newUrl.Append("?");
                            hasQuery = true;
                        }
                    }
                    newUrl.Append(query);
                    hasPrepend = false;
                }
            }
            return newUrl.ToString();
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return null;
            }

            StringBuilder query = new StringBuilder();
            bool hasParam = false;

            foreach (KeyValuePair<string, string> kv in parameters)
            {
                string name = kv.Key;
                string value = kv.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        query.Append("&");
                    }

                    query.Append(name);
                    query.Append("=");
                    query.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }

            return query.ToString();
        }

        private Encoding GetResponseEncoding(HttpWebResponse rsp)
        {
            string charset = rsp.CharacterSet;
            if (string.IsNullOrEmpty(charset))
            {
                charset = Constants.CHARSET_UTF8;
            }
            return Encoding.GetEncoding(charset);
        }

        private static bool TrustAllValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; // 忽略SSL证书检查
        }
    }
}
