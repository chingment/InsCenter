using System;
using System.Web;
using System.Web.Mvc;

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