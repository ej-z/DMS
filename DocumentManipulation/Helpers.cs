using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentManipulation
{
    static class Helpers
    {
        public static string ToAttributeString(this string name)
        {
            return "{{" + name + "}}";
        }

        public static string ToRepeaterString(this string name, string repeaterName)
        {
            return "{{" + repeaterName + "|" + name + "}}";
        }

        public static string ToGroupString(this string group)
        {
            return "[[" + group + "]]";
        }
    }
}
