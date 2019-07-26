using LocalS.BLL;
using Lumos;
using Lumos.DbRelay;
using Lumos.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LocalS.Service.Api.Merch
{
    public class UserService : BaseDbContext
    {
        public string GetStatusText(Lumos.DbRelay.Enumeration.UserStatus status)
        {
            string text = "";
            switch (status)
            {
                case Enumeration.UserStatus.Normal:
                    text = "正常";
                    break;
                case Enumeration.UserStatus.Disable:
                    text = "禁用";
                    break;
                default:
                    text = "未知";
                    break;
            }

            return text;
        }


        public CustomJsonResult GetList(string operater, string merchantId, RupUserGetList rup)
        {
            var result = new CustomJsonResult();

            var query = (from u in CurrentDb.SysMerchantUser
                         where (rup.UserName == null || u.UserName.Contains(rup.UserName)) &&
                         (rup.FullName == null || u.FullName.Contains(rup.FullName)) &&
                         u.IsDelete == false &&
                         u.IsCanDelete == true &&
                         u.MerchantId == merchantId
                         select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete, u.Status });


            int total = query.Count();

            int pageIndex = rup.Page - 1;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {

                olist.Add(new
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    FullName = item.FullName,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    Status = new { text = GetStatusText(item.Status), value = item.Status },
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime()
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, Total = total, Items = olist };

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", pageEntity);

            return result;
        }

        public CustomJsonResult Add(string operater, string merchantId, RopUserAdd rop)
        {
            var result = new CustomJsonResult();

            if (string.IsNullOrEmpty(rop.UserName))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "用户名不能为空");
            }

            if (string.IsNullOrEmpty(rop.Password))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "密码不能为空");
            }

            var isExistUserName = CurrentDb.SysUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();
            if (isExistUserName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("该用户名（{0}）已被使用", rop.UserName));
            }

            using (TransactionScope ts = new TransactionScope())
            {
                var user = new SysMerchantUser();
                user.Id = GuidUtil.New();
                user.UserName = rop.UserName;
                user.FullName = rop.FullName;
                user.PasswordHash = PassWordHelper.HashPassword(rop.Password);
                user.Email = rop.Email;
                user.PhoneNumber = rop.PhoneNumber;
                user.BelongSite = Enumeration.BelongSite.Merchant;
                user.IsDelete = false;
                user.IsCanDelete = true;
                user.Status = Enumeration.UserStatus.Normal;
                user.MerchantId = merchantId;
                user.Creator = operater;
                user.CreateTime = DateTime.Now;
                user.RegisterTime = DateTime.Now;
                user.Status = Enumeration.UserStatus.Normal;
                user.SecurityStamp = Guid.NewGuid().ToString().Replace("-", "");
                CurrentDb.SysMerchantUser.Add(user);


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "新建成功");

            }

            return result;
        }

        public CustomJsonResult InitEdit(string operater, string merchantId, string userId)
        {
            var result = new CustomJsonResult();

            var ret = new RetUserInitEdit();

            var user = CurrentDb.SysMerchantUser.Where(m => m.Id == userId).FirstOrDefault();

            ret.UserName = user.UserName;
            ret.PhoneNumber = user.PhoneNumber;
            ret.Email = user.Email;
            ret.FullName = user.UserName;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }
    }
}
