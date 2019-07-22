using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Merch
{
    public class RetUserGetInfo
    {
        public RetUserGetInfo()
        {
            this.Roles = new List<string>();
        }
        public List<string> Roles { get; set; }
        public string Introduction { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
    }
}
