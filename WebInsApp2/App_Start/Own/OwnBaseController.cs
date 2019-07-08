using Lumos;
using Lumos.Web.Mvc;
using Lumos.Web;
using System;
using System.Web.Mvc;
using WeiXinSdk;
using System.Configuration;
using LocalS.DAL;

namespace WebInsApp2
{
    [ValidateInput(false)]
    [OwnAuthorize]
    public abstract class OwnBaseController : BaseController
    {
        public OwnBaseController()
        {


        }

        public override string CurrentUserId
        {
            get
            {
                return OwnRequest.GetCurrentUserId();
            }
        }

        public WxAppInfoConfig CurrentAppInfo
        {
            get
            {
                return null;
            }
        }
    }

}