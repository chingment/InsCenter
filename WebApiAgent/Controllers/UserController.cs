using LocalS.Service.Api.Agent;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiAgent.Controllers
{
    public class UserController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetList([FromUri]RupUserGetList rup)
        {
            IResult result = AgentServiceFactory.User.GetList(this.CurrentUserId, rup);
            return new OwnApiHttpResponse(result);
        }
        [HttpGet]
        public OwnApiHttpResponse InitAdd()
        {
            IResult result = AgentServiceFactory.User.InitAdd(this.CurrentUserId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Add([FromBody]RopUserAdd rop)
        {
            IResult result = AgentServiceFactory.User.Add(this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse InitEdit([FromUri]string userId)
        {
            IResult result = AgentServiceFactory.User.InitEdit(this.CurrentUserId, userId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Edit([FromBody]RopUserEdit rop)
        {
            IResult result = AgentServiceFactory.User.Edit(this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }
    }
}
