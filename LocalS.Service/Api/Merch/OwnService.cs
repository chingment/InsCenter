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

namespace LocalS.Service.Api.Merch
{
    public class OwnService : BaseDbContext
    {
        public CustomJsonResult LoginByAccount(RopOwnLoginByAccount rop)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnLoginByAccount();

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

        public CustomJsonResult GetInfo(string operater, string userId)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnGetInfo();

            var merchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == userId).FirstOrDefault();

            ret.Name = merchantUser.Nickname;
            ret.Avatar = merchantUser.Avatar;
            ret.Introduction = merchantUser.Introduction;



            var menus = new List<Menu>();

            var menu1 = new Menu();
            menu1.Name = "User";
            menu1.Path = "/user";
            menu1.Meta = new MenuMeta { Title = "用户管理", Icon = "example" };
            menu1.Children.Add(new MenuChild { Name = "List", Path = "list", Meta = new MenuMeta { Title = "用户列表", Icon = "table" } });
            menu1.Children.Add(new MenuChild { Name = "Add", Path = "add", Meta = new MenuMeta { Title = "新建用户", Icon = "table" } });

            menus.Add(menu1);

            ret.Menus = menus;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);

            return result;
        }

        public CustomJsonResult Logout(string operater, string userId)
        {
            var result = new CustomJsonResult();

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "退出成功");

            return result;
        }
    }
}
