﻿using LocalS.Service.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Admin
{
    public class RetAdminUserInitAdd
    {
        public RetAdminUserInitAdd()
        {
            this.Orgs = new List<TreeNode>();
            this.Roles = new List<TreeNode>();
        }
        public List<TreeNode> Orgs { get; set; }

        public List<TreeNode> Roles { get; set; }
    }
}
