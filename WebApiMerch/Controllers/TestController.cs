using LocalS.Service.Api.Merch;
using Lumos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApiTerm.Controllers
{
    public class TestController : Controller
    {
        private string key = "test";
        private string secret = "6ZB97cdVz211O08EKZ6yriAYrHXFBowC";
        private long timespan = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;

        private string host = "";

        public static string GetQueryString(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return "";

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }

            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);

            return s;
        }


        public static string GetQueryString2(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return "";

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }



            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);

            return s;
        }

        private decimal GetDecimal(decimal d)
        {
            return Math.Round(d, 2);
        }

        Dictionary<string, string> model = new Dictionary<string, string>();

        public ActionResult Index()
        {

            MerchServiceFactory.User.GetList("31","00000000000000000000000000000001", new RupUserGetList { Page = 0, Limit = 10 });
            host = "http://localhost:16664/";

            model.Add("用户登录", UserLoginByAccount());

            return View(model);
        }


        public string UserLoginByAccount()
        {

            RopOwnLoginByAccount pms = new RopOwnLoginByAccount();
            pms.UserName = "Dadada";
            pms.Password = "Dadada";


            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/User/LoginByAccount",Newtonsoft.Json.JsonConvert.SerializeObject(pms), null);

            return respon_data4;

        }


       
    }
}