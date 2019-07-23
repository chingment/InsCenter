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
        [HttpPost]
        [AllowAnonymous]
        public OwnApiHttpResponse LoginByAccount(RopOwnLoginByAccount rop)
        {
            IResult result = MerchServiceFactory.Own.LoginByAccount(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse GetInfo()
        {
            IResult result = MerchServiceFactory.Own.GetInfo(this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Logout()
        {
            IResult result = MerchServiceFactory.Own.Logout(this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }

    }
}
