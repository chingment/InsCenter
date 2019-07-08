using System.Web;
using System.Web.Mvc;
using WebInsApp2.Controllers;

namespace WebInsApp2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //监控引用
            filters.Add(new OwnExceptionAttribute());
            filters.Add(new OwnStatisticsTrackerAttribute());
        }
    }
}
