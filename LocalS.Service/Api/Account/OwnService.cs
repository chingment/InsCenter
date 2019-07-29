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
        private void LoginLog(string operater, string userId, Enumeration.LoginResult loginResult, Enumeration.LoginWay loginWay, string ip, string location, string description)
        {
            var userLoginHis = new SysUserLoginHis();

            userLoginHis.Id = GuidUtil.New();
            userLoginHis.Ip = ip;
            userLoginHis.UserId = userId;
            userLoginHis.LoginWay = loginWay;
            userLoginHis.LoginTime = DateTime.Now;
            userLoginHis.Location = location;
            userLoginHis.Result = loginResult;
            userLoginHis.Description = description;
            userLoginHis.CreateTime = DateTime.Now;
            userLoginHis.Creator = operater;

            CurrentDb.SysUserLoginHis.Add(userLoginHis);
            CurrentDb.SaveChanges();
        }
        public CustomJsonResult LoginByAccount(RopOwnLoginByAccount rop)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnLoginByAccount();

            var sysUser = CurrentDb.SysUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();

            if (sysUser == null)
            {
                LoginLog("", "", Enumeration.LoginResult.Failure, rop.LoginWay, rop.Ip, "", "登录失败，账号不存在");
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败，账号不存在");
            }

            if (!PassWordHelper.VerifyHashedPassword(sysUser.PasswordHash, rop.Password))
            {
                LoginLog(sysUser.Id, sysUser.Id, Enumeration.LoginResult.Failure, rop.LoginWay, rop.Ip, "", "登录失败，密码不正确");
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败，密码不正确");
            }

            if (sysUser.IsDisable)
            {
                LoginLog(sysUser.Id, sysUser.Id, Enumeration.LoginResult.Failure, rop.LoginWay, rop.Ip, "", "登录失败，账号已被禁用");
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败，账号已被禁用");
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


            LoginLog(sysUser.Id, sysUser.Id, Enumeration.LoginResult.Success, rop.LoginWay, rop.Ip, "", "登录成功");

            SSOUtil.SetTokenInfo(ret.Token, tokenInfo);

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功", ret);

            return result;
        }

        public CustomJsonResult GetInfo(string operater, string userId, RupOwnGetInfo rup)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnGetInfo();

            var sysUser = CurrentDb.SysUser.Where(m => m.Id == userId).FirstOrDefault();

            ret.Name = sysUser.Nickname;
            ret.Avatar = sysUser.Avatar;
            ret.Introduction = sysUser.Introduction;

            switch(rup.WebSite)
            {
                case "admin":
                    break;
                case "merch":
                    var menus = new List<Menu>();

                    var menu1 = new Menu();
                    menu1.Name = "User";
                    menu1.Path = "/user";
                    menu1.Meta = new MenuMeta { Title = "用户管理", Icon = "example" };
                    menu1.Children.Add(new MenuChild { Name = "List", Path = "list", Meta = new MenuMeta { Title = "用户列表", Icon = "table" } });
                    menu1.Children.Add(new MenuChild { Name = "Add", Path = "add", Meta = new MenuMeta { Title = "新建用户", Icon = "table" } });

                    menus.Add(menu1);

                    ret.Menus = menus;
                    break;
            }
            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);

            return result;
        }

        public CustomJsonResult Logout(string operater, string userId, string token)
        {
            var result = new CustomJsonResult();


            SSOUtil.Quit(token);

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "退出成功");

            return result;
        }

        public CustomJsonResult CheckPermission(string operater, string userId, RupOwnCheckPermission rop)
        {
            var result = new CustomJsonResult();



            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "检查成功");

            return result;
        }
    }
}
