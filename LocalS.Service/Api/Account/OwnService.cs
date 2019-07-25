using LocalS.BLL;
using LocalS.Service.UI;
using Lumos;
using Lumos.DbRelay;
using Lumos.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Account
{
    public class OwnService : BaseDbContext
    {
        public CustomJsonResult LoginByAccount(RopOwnLoginByAccount rop)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnLoginByAccount();

            var sysUser = CurrentDb.SysUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();

            if (sysUser == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "账号不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(sysUser.PasswordHash, rop.Password))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "账号密码不正确");
            }

            ret.Token = GuidUtil.New();

            var tokenInfo = new TokenInfo();

            tokenInfo.UserId = sysUser.Id;

            switch (sysUser.BelongSite)
            {
                case Enumeration.BelongSite.Merchant:
                    var merchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == sysUser.Id).FirstOrDefault();
                    if (merchantUser != null)
                    {
                        tokenInfo.MerchantId = merchantUser.MerchantId;
                    }
                    break;
            }

            SSOUtil.SetTokenInfo(ret.Token, tokenInfo);

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功", ret);

            return result;
        }

        public CustomJsonResult GetInfo(string operater, string userId)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnGetInfo();

            var sysUser = CurrentDb.SysUser.Where(m => m.Id == userId).FirstOrDefault();

            ret.Name = sysUser.Nickname;
            ret.Avatar = sysUser.Avatar;
            ret.Introduction = sysUser.Introduction;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);

            return result;
        }

        public CustomJsonResult Logout(string operater,string userId, string token)
        {
            var result = new CustomJsonResult();


            SSOUtil.Quit(token);

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "退出成功");

            return result;
        }
    }
}
