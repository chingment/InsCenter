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

    }
}
