using LocalS.BLL;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Account
{
    public class LoginLogService: BaseDbContext
    {
        public CustomJsonResult GetList(string operater, string userId, RupLoginLogGetList rup)
        {
            var result = new CustomJsonResult();

            var query = (from u in CurrentDb.SysUserLoginHis
                          
                         select new { u.Id, u.LoginTime, u.Ip, u.City });


            int total = query.Count();

            int pageIndex = rup.Page - 1;
            int pageSize = rup.Limit;
            query = query.OrderByDescending(r => r.LoginTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {

                olist.Add(new
                {
                    Id = item.Id,
                    LoginTime = item.LoginTime.ToUnifiedFormatDateTime(),
                    Ip = item.Ip,
                    Address = item.City
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, Total = total, Items = olist };

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", pageEntity);

            return result;
        }
    }
}
