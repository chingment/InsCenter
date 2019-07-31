using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.UI
{
    public class Menu
    {
        public Menu()
        {
            this.Meta = new MenuMeta();
            this.Children = new List<MenuChild>();
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public MenuMeta Meta { get; set; }
        public List<MenuChild> Children { get; set; }
        public bool Hidden { get; set; }
        public string Component { get; set; }
        public bool Navbar { get; set; }

        public string Redirect { get; set; }
    }
}
