using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Merch
{
    public class MerchServiceFactory
    {
        public static OwnService Own
        {
            get
            {
                return new OwnService();
            }
        }

        public static UserService User
        {
            get
            {
                return new UserService();
            }
        }
    }
}
