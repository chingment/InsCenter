using LocalS.Service.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.Merch
{
    public class RetOwnGetInfo
    {
        public RetOwnGetInfo()
        {
            this.Roles = new List<string>();
            this.Menus = new List<Menu>();
        }
        public string Introduction { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
