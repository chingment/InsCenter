using LocalS.Service.Api.InsApp;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiInsApp.Controllers
{
    public class InsCarController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetIndexPageData()
        {
            IResult result = InsAppServiceFactory.InsCar.GetIndexPageData();

            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse SearchCarInfo([FromUri]RupSearchCarInfo rup)
        {
            IResult result = InsAppServiceFactory.InsCar.SearchCarInfo(rup);

            return new OwnApiHttpResponse(result);
        }
    }
}
