﻿using LocalS.Service.Api.Merch;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiAdmin.Controllers
{
    public class UserController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetList([FromUri]RupUserGetList rup)
        {
            IResult result = MerchServiceFactory.User.GetList(this.CurrentUserId, this.CurrentMerchantId, rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Add([FromBody]RopUserAdd rop)
        {
            IResult result = MerchServiceFactory.User.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse InitEdit([FromUri]string userId)
        {
            IResult result = MerchServiceFactory.User.InitEdit(this.CurrentUserId, this.CurrentMerchantId, userId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Edit([FromBody]RopUserEdit rop)
        {
            IResult result = MerchServiceFactory.User.Edit(this.CurrentUserId, this.CurrentMerchantId, rop);
            return new OwnApiHttpResponse(result);
        }
    }
}