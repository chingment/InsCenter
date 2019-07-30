using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.UI
{
    public class MenuChild
    {
        public MenuChild()
        {
            this.Meta = new MenuMeta();
        }
        public string Name { get; set; }
        public string Path { get; set; }
        public MenuMeta Meta { get; set; }
        public string Component { get; set; }
    }
}
