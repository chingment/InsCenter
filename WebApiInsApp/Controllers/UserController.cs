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
    public class UserController : ApiController
    {
        [HttpGet]
        public OwnApiHttpResponse LoginByUrlParams([FromUri]string mId, [FromUri]string uId)
        {
            IResult result = InsAppServiceFactory.User.LoginByUrlParams(mId, uId);

            return new OwnApiHttpResponse(result);
        }
    }
}
