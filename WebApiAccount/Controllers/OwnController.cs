using LocalS.Service.Api.Account;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiAccount.Controllers
{
    public class OwnController : OwnApiBaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public OwnApiHttpResponse LoginByAccount(RopOwnLoginByAccount rop)
        {
            IResult result = AccountServiceFactory.Own.LoginByAccount(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse GetInfo()
        {
            IResult result = AccountServiceFactory.Own.GetInfo(this.CurrentUserId, this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Logout()
        {
            IResult result = AccountServiceFactory.Own.Logout(this.CurrentUserId, this.CurrentUserId,this.Token);
            return new OwnApiHttpResponse(result);
        }

    }
}
