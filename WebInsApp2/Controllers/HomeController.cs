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
    }
}