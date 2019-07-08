using System;
using System.Web.Mvc;
using Lumos;
using Lumos.Web;

namespace WebInsApp2
{
    #region 授权过滤器
    // 摘要:
    //     继承Authorize属性
    //     扩展Permission权限代码,用来控制用户是否拥有该类或方法的权限
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OwnAuthorizeAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogUtil.SetTrackId();

            base.OnActionExecuting(filterContext);

            var request = filterContext.HttpContext.Request;

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
            {
                return;
            }

            var userInfo = OwnRequest.GetUserInfo();

            bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();

            string userAgent = filterContext.HttpContext.Request.UserAgent;

            string returnUrl = "";

            if (isAjaxRequest)
            {
                returnUrl = request.UrlReferrer.PathAndQuery;
            }
            else
            {
                returnUrl = request.Url.PathAndQuery;
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = System.Web.HttpUtility.UrlEncode(returnUrl);
            }

            if (userInfo == null)
            {
                LogUtil.Info("用户没有登录或登录超时");

                string redirectUrl = OwnWebSettingUtils.GetLoginPage(returnUrl);

                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    LogUtil.Info("去往微信浏览器授权页面验证");
                    redirectUrl = OwnWebSettingUtils.WxOauth2(returnUrl);
                }
                else
                {
                    LogUtil.Info("去往用户账号登录页面验证");
                }

                if (isAjaxRequest)
                {
                    MessageBox messageBox = new MessageBox();
                    messageBox.No = Guid.NewGuid().ToString();
                    messageBox.Type = MessageBoxType.Failure;
                    messageBox.Title = "请登录";
                    messageBox.RedirectUrl = redirectUrl;
                    CustomJsonResult jsonResult = new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "", messageBox);
                    filterContext.Result = jsonResult;
                    filterContext.Result.ExecuteResult(filterContext);
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    filterContext.Result = new RedirectResult(redirectUrl);
                }
            }
            else
            {
                LogUtil.Info("用户Id:" + userInfo.UserId);
            }

        }
    }
    #endregion
}