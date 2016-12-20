using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentManipulation
{
    static class Helpers
    {
        public static string ToAttributeString(this string name, string type)
        {
            return "{{" + name + "|" + type + "}}";
        }

        public static string ToRepeaterString(this string name, string repeaterName, string type)
        {
            return "{{" + repeaterName + "|" + name + "|" + type + "}}";
        }
    }
}
