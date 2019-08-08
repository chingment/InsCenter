using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Admin
{
    public class RopAgentRoleEdit
    {
        public string RoleId { get; set; }
        public string Description { get; set; }

        public List<string> MenuIds { get; set; }
    }
}
