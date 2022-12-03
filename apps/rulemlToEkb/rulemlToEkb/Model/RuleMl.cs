using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rulemlToEkb.Model
{
    internal class RuleMl
    {
        public string ClassName { get; set; }

        public List<(string, string)> Attribute { get; set; }


        public RuleMl(string className, List<(string, string)> attribute)
        {
            ClassName = className;
            Attribute = attribute;
        }
    }
}
