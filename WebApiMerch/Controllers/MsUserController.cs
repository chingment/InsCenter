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
    public class MsUserController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetList([FromUri]RupMsUserGetList rup)
        {
            IResult result = MerchServiceFactory.MsUser.GetList(this.CurrentMerchantId, rup);
            return new OwnApiHttpResponse(result);
        }
    }
}
