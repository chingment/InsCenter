using LocalS.Service.Api.Merch;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiMerch.Controllers
{
    public class OwnController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetInfo()
        {
            IResult result = MerchServiceFactory.Own.GetInfo(this.CurrentUserId, this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }
    }
}
