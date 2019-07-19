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
    public class UserController : ApiController
    {
        [HttpPost]
        public OwnApiHttpResponse LoginByAccount(RopUserLoginByAccount rop)
        {
            IResult result = MerchServiceFactory.User.LoginByAccount(rop);

            return new OwnApiHttpResponse(result);
        }
    }
}
