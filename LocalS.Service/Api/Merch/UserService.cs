using LocalS.BLL;
using Lumos;
using Lumos.DbRelay;
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

            if (merchantUser==null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "账号不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(merchantUser.PasswordHash, rop.Password))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "账号密码不正确"); 
            }

            ret.MId = merchantUser.MerchantId;
            ret.UId = merchantUser.Id;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }
    }
}
