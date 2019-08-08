using LocalS.Service.Api.Admin;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiAdmin.Controllers
{
    public class MerchRoleControllerr : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetList([FromUri]RupMerchRoleGetList rup)
        {
            IResult result = AdminServiceFactory.MerchRole.GetList(this.CurrentUserId, rup);
            return new OwnApiHttpResponse(result);
        }
        [HttpGet]
        public OwnApiHttpResponse InitAdd()
        {
            IResult result = AdminServiceFactory.MerchRole.InitAdd(this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Add([FromBody]RopMerchRoleAdd rop)
        {
            IResult result = AdminServiceFactory.MerchRole.Add(this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse InitEdit([FromUri]string roleId)
        {
            IResult result = AdminServiceFactory.MerchRole.InitEdit(this.CurrentUserId, roleId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Edit([FromBody]RopMerchRoleEdit rop)
        {
            IResult result = AdminServiceFactory.MerchRole.Edit(this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }
    }
}