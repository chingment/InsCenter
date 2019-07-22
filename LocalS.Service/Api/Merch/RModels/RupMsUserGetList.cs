using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Merch
{
    public class RupMsUserGetList: RupBaseGetList
    {
        public string MerchantId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
    }
}
