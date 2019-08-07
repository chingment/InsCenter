using LocalS.BLL;
using Lumos;
using Lumos.DbRelay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.InsApp
{
    public class OwnService : BaseDbContext
    {
        public CustomJsonResult LoginByUrlParams(string mId, string tppId)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnLoginByUrlParams();


            if (string.IsNullOrEmpty(mId))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "您好，应用无法访问，造成的原因：商户标识参数为空");
            }

            if (string.IsNullOrEmpty(tppId))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "您好，应用无法访问，造成的原因：用户标识参数为空");
            }

            var merchant = CurrentDb.Merchant.Where(m => m.Id == mId).FirstOrDefault();

            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "您好，应用无法访问，造成的原因：商户信息无法解释");
            }


            var merchantUser = CurrentDb.SysMerchantUser.Where(m => m.MerchantId == mId && m.TppId == tppId).FirstOrDefault();
            if (merchantUser == null)
            {
                merchantUser = new SysMerchantUser();
                merchantUser.Id = GuidUtil.New();
                merchantUser.UserName = GuidUtil.New();
                merchantUser.PasswordHash = PassWordHelper.HashPassword("Caskujn");
                merchantUser.SecurityStamp = GuidUtil.New();
                merchantUser.RegisterTime = DateTime.Now;
                merchantUser.IsDisable = false;
                merchantUser.BelongSite = Enumeration.BelongSite.Agent;
                merchantUser.IsCanDelete = false;
                merchantUser.CreateTime = DateTime.Now;
                merchantUser.Creator = merchantUser.Id;
                merchantUser.MerchantId = merchant.Id;
                merchantUser.TppId = tppId;
                CurrentDb.SysMerchantUser.Add(merchantUser);
                CurrentDb.SaveChanges();
            }

            ret.MId = merchantUser.MerchantId;
            ret.UId = merchantUser.Id;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult LoginByAccount(RopOwnLoginByAccount rop)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnLoginByUrlParams();

            var merchantUser = CurrentDb.SysMerchantUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();

            if (merchantUser == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "账号不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(merchantUser.PasswordHash, rop.Password))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "账号密码不正确");
            }

            if (merchantUser.IsDisable)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该账号已被禁用");
            }

            ret.MId = merchantUser.MerchantId;
            ret.UId = merchantUser.Id;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }
    }
}
