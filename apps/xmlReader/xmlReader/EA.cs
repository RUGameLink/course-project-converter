using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlReader
{
    internal class EA
    {
        public string ClassName { get; set; }

        public List<(string, string, string, string)> Attribute { get; set; }

        public List<(string, string, string, string)> Operation { get; set; }

        public EA(string className, List<(string, string, string, string)> attribute, List<(string, string, string, string)> operation)
        {
            ClassName = className;
            Attribute = attribute;
            Operation = operation;
        }
    }
}
