using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.InsApp
{
    public class RetSearchCarInfo
    {
        public RetSearchCarInfo()
        {
            this.CarInfo = new InsCarInfoModel();
            this.CarOwner = new InsCustomerModel();
        }

        public InsCarInfoModel CarInfo { get; set; }
        public InsCustomerModel CarOwner { get; set; }
    }
}
