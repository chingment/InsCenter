using System;
using System.Web;
using System.Web.Mvc;
using Lumos.Web.Mvc;
using System.Text;
using System.Web.Security;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lumos;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using WeiXinSdk;
using Lumos.DbRelay;
using WeiXinSdk.MsgPush;
using Lumos.BLL;

namespace WebInsApp2.Controllers
{
    public class HomeController : OwnBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        //获取JsApiConfig配置参数
        public CustomJsonResult<JsApiConfigParams> GetJsApiConfigParams(string url)
        {  
            return SdkFactory.Wx.GetJsApiConfigParams(this.CurrentAppInfo, url);
        }

        [AllowAnonymous]
        public RedirectResult Oauth2()
        {
            
            return Redirect("/Home/Oauth2");
        }

        [AllowAnonymous]
        public ContentResult PayResult()
        {
            return Content("");
        }

        [AllowAnonymous]
        public ActionResult LogJsError(string errorMessage, string scriptURI, string columnNumber, string errorObj)
        {
            CustomJsonResult res = new CustomJsonResult();
            StringBuilder sb = new StringBuilder();
            sb.Append("前端JS脚本错误：" + errorMessage + "\t\n");
            sb.Append("错误信息：" + errorMessage + "\t\n");
            sb.Append("出错文件：" + scriptURI + "\t\n");
            sb.Append("出错列号：" + columnNumber + "\t\n");
            sb.Append("错误详情：" + errorObj + "\t\n");
            sb.Append("浏览器agent：" + Lumos.CommonUtil.GetBrowserInfo() + "\t\n");
            LogUtil.Error(sb.ToString());
            return res;

        }

        [AllowAnonymous]
        public ActionResult NotifyEvent()
        {
            LogUtil.Info("开始接收事件推送通知");

            //if (Request.HttpMethod == "POST")
            //{

            //    Stream stream = Request.InputStream;
            //    stream.Seek(0, SeekOrigin.Begin);
            //    string xml = new StreamReader(stream).ReadToEnd();

            //    LogUtil.Info("接收事件推送内容:" + xml);

            //    var baseEventMsg = WxMsgFactory.CreateMessage(xml);
            //    string echoStr = "";
            //    string eventKey = null;
            //    LogUtil.Info("baseEventMsg内容:" + baseEventMsg);
            //    if (baseEventMsg != null)
            //    {
            //        var appId = Request.QueryString["appId"];
            //        var appInfo = BizFactory.Merchant.GetWxPaAppInfoConfig("");
            //        var userInfo_Result = SdkFactory.Wx.GetUserInfoByApiToken(appInfo, baseEventMsg.FromUserName);

            //        if (userInfo_Result.openid != null)
            //        {
            //            LogUtil.Info("userInfo_Result:" + JsonConvert.SerializeObject(userInfo_Result));

            //            var ropWxUserCheckedUser = new RopWxUserCheckedUser();

            //            ropWxUserCheckedUser.OpenId = userInfo_Result.openid;
            //            ropWxUserCheckedUser.Nickname = userInfo_Result.nickname;
            //            ropWxUserCheckedUser.Sex = userInfo_Result.sex.ToString();
            //            ropWxUserCheckedUser.Province = userInfo_Result.province;
            //            ropWxUserCheckedUser.City = userInfo_Result.city;
            //            ropWxUserCheckedUser.Country = userInfo_Result.country;
            //            ropWxUserCheckedUser.HeadImgUrl = userInfo_Result.headimgurl;
            //            ropWxUserCheckedUser.UnionId = userInfo_Result.unionid;

            //            var retWxUserCheckedUser = BizFactory.WxUser.CheckedUser(GuidUtil.New(), ropWxUserCheckedUser);

            //            if (retWxUserCheckedUser != null)
            //            {
            //                var wxAutoReply = new WxAutoReply();
            //                switch (baseEventMsg.MsgType)
            //                {
            //                    case MsgType.TEXT:
            //                        #region TEXT

            //                        LogUtil.Info("文本消息");

            //                        var textMsg = (TextMsg)baseEventMsg;

            //                        if (textMsg != null)
            //                        {
            //                            LogUtil.Info("文本消息:" + textMsg.Content);
            //                        }


            //                        #endregion
            //                        break;
            //                    case MsgType.EVENT:
            //                        #region EVENT
            //                        switch (baseEventMsg.Event)
            //                        {
            //                            case EventType.SUBSCRIBE://订阅
            //                                break;
            //                            case EventType.UNSUBSCRIBE://取消订阅
            //                                break;
            //                            case EventType.SCAN://扫描二维码
            //                            case EventType.CLICK://单击按钮
            //                            case EventType.VIEW://链接按钮
            //                                break;
            //                            case EventType.USER_GET_CARD://领取卡卷
            //                                #region  USER_GET_CARD


            //                                #endregion
            //                                break;
            //                            case EventType.USER_CONSUME_CARD://核销卡卷
            //                                #region USER_CONSUME_CARD


            //                                #endregion
            //                                break;
            //                        }
            //                        #endregion
            //                        break;
            //                }

            //                var wxMsgPushLog = new WxMsgPushLog();
            //                wxMsgPushLog.Id = GuidUtil.New();
            //                wxMsgPushLog.UserId = retWxUserCheckedUser.ClientUserId;
            //                wxMsgPushLog.ToUserName = baseEventMsg.ToUserName;
            //                wxMsgPushLog.FromUserName = baseEventMsg.FromUserName;
            //                wxMsgPushLog.CreateTime = DateTime.Now;
            //                wxMsgPushLog.ContentXml = xml;
            //                wxMsgPushLog.MsgId = baseEventMsg.MsgId;
            //                wxMsgPushLog.MsgType = baseEventMsg.MsgType.ToString();
            //                wxMsgPushLog.Event = baseEventMsg.Event.ToString();
            //                wxMsgPushLog.EventKey = eventKey;

            //                WxMsgPushLog(wxMsgPushLog);
            //            }
            //        }
            //    }

            //    LogUtil.Info(string.Format("接收事件推送之后回复内容:{0}", echoStr));

            //    Response.Write(echoStr);
            //}
            //else if (Request.HttpMethod == "GET") //微信服务器在首次验证时，需要进行一些验证，但。。。。  
            //{
            //    if (string.IsNullOrEmpty(Request["echostr"]))
            //    {
            //        Response.Write("无法获取微信接入信息，仅供测试！");

            //    }

            //    Response.Write(Request["echostr"].ToString());
            //}
            //else
            //{
            //    Response.Write("wrong");
            //}

            Response.End();

            return View();
        }

        public Task<bool> WxMsgPushLog(WxMsgPushLog wxMsgPushLog)
        {
            return Task.Run(() =>
            {

                CurrentDb.WxMsgPushLog.Add(wxMsgPushLog);
                CurrentDb.SaveChanges();


                return true;

            });
        }

        private bool CheckSignature()
        {
            string signature = Request.QueryString["signature"].ToString();
            string timestamp = Request.QueryString["timestamp"].ToString();
            string nonce = Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { SdkFactory.Wx.GetNotifyEventUrlToken(this.CurrentAppInfo), timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Bitmap CirclePhoto(string urlPath, int size)
        {

            try
            {
                System.Net.WebRequest webreq = System.Net.WebRequest.Create(urlPath);
                System.Net.WebResponse webres = webreq.GetResponse();
                System.IO.Stream stream = webres.GetResponseStream();
                Image img1 = System.Drawing.Image.FromStream(stream);
                stream.Dispose();

                Bitmap b = new Bitmap(size, size);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.DrawImage(img1, 0, 0, b.Width, b.Height);
                    int r = Math.Min(b.Width, b.Height) / 2;
                    PointF c = new PointF(b.Width / 2.0F, b.Height / 2.0F);
                    for (int h = 0; h < b.Height; h++)
                        for (int w = 0; w < b.Width; w++)
                            if ((int)Math.Pow(r, 2) < ((int)Math.Pow(w * 1.0 - c.X, 2) + (int)Math.Pow(h * 1.0 - c.Y, 2)))
                            {
                                b.SetPixel(w, h, Color.Transparent);
                            }
                    //画背景色圆
                    using (Pen p = new Pen(System.Drawing.SystemColors.Control))
                        g.DrawEllipse(p, 0, 0, b.Width, b.Height);
                }
                return b;
            }
            catch (Exception ex)
            {
                LogUtil.Error("CirclePhoto生成发生异常", ex);

                return null;
            }

        }
    }
}