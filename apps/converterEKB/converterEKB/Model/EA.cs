using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace converterEKB.Model
{
    internal class EA
    {
        public string ClassName { get; set; }

        public List<( string, string, string)> Attribute { get; set; }


        public EA(string className, List<( string, string, string)> attribute)
        {
            ClassName = className;
            Attribute = attribute;
        }
    }
}
