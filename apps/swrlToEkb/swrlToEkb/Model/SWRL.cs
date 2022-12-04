using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swrlToEkb.Model
{
    internal class SWRL
    {
        public string ClassName { get; set; }

        public List<(string, string)> Attribute { get; set; }


        public SWRL(string className, List<(string, string)> attribute)
        {
            ClassName = className;
            Attribute = attribute;
        }
    }
}
