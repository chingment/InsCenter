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
                case Enumeration.BelongSite.Merch:
                    var merchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == sysUser.Id).FirstOrDefault();
                    if (merchantUser != null)
                    {
                        tokenInfo.MerchantId = merchantUser.MerchantId;
                    }
                    break;
            }


            LoginLog(sysUser.Id, sysUser.Id, Enumeration.LoginResult.Success, rop.LoginWay, rop.Ip, "", "登录成功");

            SSOUtil.SetTokenInfo(ret.Token, tokenInfo, new TimeSpan(1, 0, 0));

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功", ret);

            return result;
        }

        private List<Menu> GetMenus(Enumeration.BelongSite belongSite)
        {
            var menus = new List<Menu>();

            var sysMenus = CurrentDb.SysMenu.Where(m => m.BelongSite == belongSite).ToList();

            var sysMenusDept1 = from c in sysMenus where c.Dept == 1 select c;

            foreach (var sysMenuDept1 in sysMenusDept1)
            {
                var menu1 = new Menu();
               
                menu1.Path = sysMenuDept1.Path;
                menu1.Component = null;
                menu1.Meta = new MenuMeta { Title = sysMenuDept1.Title, Icon = sysMenuDept1.Icon };

                var sysMenusDept2 = (from c in sysMenus where c.PId == sysMenuDept1.Id select c).ToList();

                if (sysMenusDept2.Count == 0)
                {
                    menu1.Name = null;
                    menu1.Children.Add(new MenuChild { Name = sysMenuDept1.Name, Path = sysMenuDept1.Path, Component = sysMenuDept1.Component, Meta = new MenuMeta { Title = sysMenuDept1.Title, Icon = sysMenuDept1.Icon } });
                }
                else
                {
                    menu1.Name = sysMenuDept1.Name;
                    foreach (var sysMenuDept2 in sysMenusDept2)
                    {
                        menu1.Children.Add(new MenuChild { Name = sysMenuDept2.Name, Path = sysMenuDept2.Path, Component = sysMenuDept2.Component, Meta = new MenuMeta { Title = sysMenuDept2.Title, Icon = sysMenuDept2.Icon } });
                    }
                }

                menus.Add(menu1);
            }

            return menus;
        }
        public CustomJsonResult GetInfo(string operater, string userId, RupOwnGetInfo rup)
        {
            var result = new CustomJsonResult();
            var ret = new RetOwnGetInfo();

            var sysUser = CurrentDb.SysUser.Where(m => m.Id == userId).FirstOrDefault();

            ret.UserName = sysUser.UserName;
            ret.FullName = sysUser.FullName;
            ret.Avatar = sysUser.Avatar;
            ret.Introduction = sysUser.Introduction;
            ret.Email = sysUser.Email;
            ret.PhoneNumber = sysUser.PhoneNumber;

            switch (rup.WebSite)
            {
                case "admin":
                    ret.Menus = GetMenus(Enumeration.BelongSite.Admin);
                    break;
                case "merch":
                    ret.Menus = GetMenus(Enumeration.BelongSite.Merch);
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

        public CustomJsonResult CheckPermission(string operater, string userId, string token, RupOwnCheckPermission rop)
        {
            var result = new CustomJsonResult();

            SSOUtil.Postpone(token);

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "检查成功");

            return result;
        }
    }
}
