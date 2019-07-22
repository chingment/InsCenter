using LocalS.BLL;
using Lumos;
using Lumos.DbRelay;
using Lumos.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Merch
{
    public class UserService : BaseDbContext
    {
        public CustomJsonResult LoginByAccount(RopUserLoginByAccount rop)
        {
            var result = new CustomJsonResult();
            var ret = new RetUserLoginByAccount();

            var merchantUser = CurrentDb.SysMerchantUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();

            if (merchantUser == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "账号不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(merchantUser.PasswordHash, rop.Password))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "账号密码不正确");
            }

            ret.Token = GuidUtil.New();

            SSOUtil.SetTokenInfo(ret.Token, new TokenInfo { UserId = merchantUser.Id, MerchantId = merchantUser.MerchantId });

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功", ret);

            return result;
        }

        public CustomJsonResult GetInfo(string userId)
        {
            var result = new CustomJsonResult();
            var ret = new RetUserGetInfo();

            var merchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == userId).FirstOrDefault();

            ret.Name = merchantUser.Nickname;
            ret.Avatar = merchantUser.Avatar;
            ret.Introduction = merchantUser.Introduction;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);

            return result;
        }

        public CustomJsonResult Logout(string userId)
        {
            var result = new CustomJsonResult();

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "退出成功");

            return result;
        }
    }
}
