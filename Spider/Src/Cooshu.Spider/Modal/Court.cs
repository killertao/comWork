using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooshu.Spider.Modal
{
    public class Court
    {
        public string Name { get; set; }

        public string Province { get; set; }
        
        public List<Court> Children { get; set; } = new List<Court>();
    }
}
