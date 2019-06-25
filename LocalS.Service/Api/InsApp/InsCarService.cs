using LocalS.BLL;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.InsApp
{
    public class InsCarService : BaseDbContext
    {
        public CustomJsonResult GetIndexPageData()
        {
            var result = new CustomJsonResult();

            var ret = new RetGetIndexPageData();

            var carPlateNoSearchHiss = CurrentDb.InsCarPlateNoSearchHis.OrderByDescending(m => m.CreateTime).ToList();

            foreach (var item in carPlateNoSearchHiss)
            {
                ret.SearchPlateNoRecords.Add(new InsCarSearchPlateNoRecordModel { PlateNo = item.CarPlateNo });
            }

            var carCompanyRules = CurrentDb.InsCarCompanyRule.OrderByDescending(m => m.Priority).ToList();

            foreach (var item in carCompanyRules)
            {
                ret.CompanyRules.Add(new InsCarCompanyRuleModel { CompanyId = item.CompanyId, CompanyName = item.CompanyName, CompanyImgUrl = item.CompanyImgUrl, CommissionRate = item.CommissionRate });
            }


            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);

            return result;
        }

        public CustomJsonResult SearchCarInfo(RupSearchCarInfo rup)
        {
            var result = new CustomJsonResult();

            var ret = new RetSearchCarInfo();


            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "", ret);

            return result;
        }
    }
}
