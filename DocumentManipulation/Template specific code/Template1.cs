using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentManipulation
{
    class Template1
    {
        public string CielingResults1(DocInputs inputs)
        {
            var None = inputs.Attributes["None"];
            var Loosefill = inputs.Attributes["Loosefill"];
            var InsulationBatts = inputs.Attributes["InsulationBatts"];

            if(None.FinalValue=="True")
            {
                return "absence of insulation batts/loose fill insulation";
            }
            else if(Loosefill.FinalValue == "True" &&InsulationBatts.FinalValue == "True")
            {
                return "presence of loose fill insulation, insulation batts";
            }
            else if (Loosefill.FinalValue == "True")
            {
                return "presence of loose fill insulation";
            }
            else if (InsulationBatts.FinalValue == "True")
            {
                return "presence of insulation batts";
            }

            return string.Empty;
        }

        public string CielingResults2(DocInputs inputs)
        {
            var LFAI = inputs.Attributes["LFAIfound"];
            var Loosefill = inputs.Attributes["Loosefill"];
            var InsulationBatts = inputs.Attributes["InsulationBatts"];

            if (LFAI.Value == "Yes")
            {
                return "Yes,";
            }
            else
            {
                return "No";
            }
        }

        public string CielingResults3(DocInputs inputs)
        {
            var None = inputs.Attributes["None"];
            var Loosefill = inputs.Attributes["Loosefill"];
            var InsulationBatts = inputs.Attributes["InsulationBatts"];

            if (None.FinalValue == "True")
            {
                return "No asbestos insulation was detected in the ceiling space.";
            }
            else if (Loosefill.FinalValue == "True" && InsulationBatts.FinalValue == "True")
            {
                return "Insulation batts, Loose fill insulation are distributed throughout the ceiling space.";
            }
            else if (Loosefill.FinalValue == "True")
            {
                return "Loose fill insulation is distributed throughout the ceiling space.";
            }
            else if (InsulationBatts.FinalValue == "True")
            {
                return "Insulation batts is distributed throughout the ceiling space.";
            }

            return string.Empty;
        }

        public string SampleResults1(DocInputs inputs)
        {
            var repeater = inputs.Repeaters["Sample"];
            var c = repeater.Count > 1 ? "were" : "was";
            var d = new Dictionary<string, int>();
            int x = 0;
            for(var i = 0; i < repeater.Count;i++)
            {
                var desc = repeater.GetAttribute(i, "SampleDescription").FinalValue;
                if (desc != null)
                {
                    if (d.ContainsKey(desc))
                        d[desc]++;
                    else
                    {
                        d.Add(desc, 1);
                        x++;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            bool first = true;
            foreach(var v in d)
            {
                sb.Append(NumberToWords(v.Value, first) + " ("+ v.Value +") " + v.Key);
                if (x == 2)
                    sb.Append(" and ");
                if (x > 2)
                    sb.Append(", ");

                first = false;
            }
            sb.Append(" "+c);

            return sb.ToString();
        }

        public string SampleResults2(DocInputs inputs)
        {
            var repeater = inputs.Repeaters["Sample"];
            for (var i = 0; i < repeater.Count; i++)
            {
                var res = repeater.GetAttribute(i, "Result").FinalValue;
                if (string.IsNullOrEmpty(res) || res.ToLower() != "no asbestos detected")
                    return string.Empty;
            }

            return "No asbestos was detected within any of the samples collected";
        }

        public static string NumberToWords(int number, bool Capitalize= false)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return Capitalize? FirstLetterToUpper(words) : words;
        }

        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

    }
}
