﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Merch
{
    public class MerchServiceFactory
    {
        public static UserService User
        {
            get
            {
                return new UserService();
            }
        }

        public static MsUserService MsUser
        {
            get
            {
                return new MsUserService();
            }
        }
    }
}
