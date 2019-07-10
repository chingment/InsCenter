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
    public class UserService : BaseDbContext
    {
        public CustomJsonResult LoginByUrlParams(string mId, string uId)
        {
            var result = new CustomJsonResult();
            var ret = new RetUserLoginByUrlParams();

            var merchant = CurrentDb.Merchant.Where(m => m.Id == mId).FirstOrDefault();

            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该商户不存在");
            }

            var merchantUser = CurrentDb.SysMerchantUser.Where(m => m.MerchantId == mId && m.TppId == uId).FirstOrDefault();
            if (merchantUser == null)
            {
                merchantUser = new SysMerchantUser();
                merchantUser.Id = GuidUtil.New();
                merchantUser.UserName = GuidUtil.New();
                merchantUser.PasswordHash = PassWordHelper.HashPassword("Caskujn");
                merchantUser.SecurityStamp = GuidUtil.New();
                merchantUser.RegisterTime = DateTime.Now;
                merchantUser.Status = Enumeration.UserStatus.Normal;
                merchantUser.BelongSite = Enumeration.BelongSite.Merchant;
                merchantUser.IsCanDelete = false;
                merchantUser.CreateTime = DateTime.Now;
                merchantUser.Creator = merchantUser.Id;
                merchantUser.MerchantId = merchant.Id;
                merchantUser.TppId = uId;
                CurrentDb.SysMerchantUser.Add(merchantUser);
                CurrentDb.SaveChanges();
            }

            ret.UserId = merchantUser.Id;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }
    }
}
