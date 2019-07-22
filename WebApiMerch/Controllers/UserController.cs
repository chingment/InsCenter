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
    public class UserController : OwnApiBaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public OwnApiHttpResponse LoginByAccount(RopUserLoginByAccount rop)
        {
            IResult result = MerchServiceFactory.User.LoginByAccount(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse GetInfo()
        {
            IResult result = MerchServiceFactory.User.GetInfo(this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Logout()
        {
            IResult result = MerchServiceFactory.User.Logout(this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }

    }
}
