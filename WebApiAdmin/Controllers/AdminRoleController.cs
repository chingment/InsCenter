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
    public class AdminRoleController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetList([FromUri]RupAdminRoleGetList rup)
        {
            IResult result = AdminServiceFactory.AdminRole.GetList(this.CurrentUserId, rup);
            return new OwnApiHttpResponse(result);
        }
        [HttpGet]
        public OwnApiHttpResponse InitAdd()
        {
            IResult result = AdminServiceFactory.AdminRole.InitAdd(this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Add([FromBody]RopAdminRoleAdd rop)
        {
            IResult result = AdminServiceFactory.AdminRole.Add(this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse InitEdit([FromUri]string roleId)
        {
            IResult result = AdminServiceFactory.AdminRole.InitEdit(this.CurrentUserId, roleId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Edit([FromBody]RopAdminRoleEdit rop)
        {
            IResult result = AdminServiceFactory.AdminRole.Edit(this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }
    }
}
