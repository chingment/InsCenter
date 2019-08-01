﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Admin
{
    public class AdminServiceFactory
    {
        public static AdminUserService AdminUser
        {
            get
            {
                return new AdminUserService();
            }
        }

        public static AdminRoleService AdminRole
        {
            get
            {
                return new AdminRoleService();
            }
        }
    }
}