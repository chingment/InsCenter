using Lumos;
using System.Web;
using System.Configuration;
using Lumos.Web.Http;
using Lumos.Session;

namespace WebApiMerch
{
    [OwnApiAuthorize]
    public class OwnApiBaseController : BaseController
    {
        private OwnApiHttpResult _result = new OwnApiHttpResult();

        public OwnApiHttpResult Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public OwnApiBaseController()
        {
            LogUtil.SetTrackId();
        }

        public OwnApiHttpResponse ResponseResult(OwnApiHttpResult result)
        {
            return new OwnApiHttpResponse(result);
        }

        public OwnApiHttpResponse ResponseResult(ResultType resultType, string resultCode, string message = null, object data = null)
        {
            _result.Result = resultType;
            _result.Code = resultCode;
            _result.Message = message;
            _result.Data = data;
            return new OwnApiHttpResponse(_result);
        }

        public string CurrentUserId
        {
            get
            {
                //var request = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request;
                //var token = request.QueryString["token"];
                //var tokenInfo = SSOUtil.GetTokenInfo(token);

                //return tokenInfo.UserId;
                return "00000000000000000000000000000001";
            }

        }

        public string CurrentMerchantId
        {
            get
            {
                //var request = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request;
                //var token = request.QueryString["token"];
                //var tokenInfo = SSOUtil.GetTokenInfo(token);

                //return tokenInfo.MerchantId;

                return "00000000000000000000000000000001";
            }

        }
    }
}