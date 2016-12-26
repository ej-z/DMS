using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentManipulation
{
    class AttributeFactory
    {
        public static Attribute Create(string type, string properties, Dictionary<string, Attribute> attributes = null)
        {
            var propertiesMap = properties.Split('|')
                .ToDictionary(x => x.Split(':')[0], x => x.Split(':').Length > 1 ? x.Split(':')[1] : string.Empty);
            Attribute attribute;
            switch (type)
            {
                case "Text":
                case "TextArea":
                    attribute = new TextAttribute(type);
                    break;
                case "Enum":
                    attribute = new EnumAttribute(type);
                    break;
                case "Bit":
                    attribute = new BitAttribute(type);
                    break;
                case "Image":
                    attribute = new ImageAttribute(type);
                    break;
                case "Complex":
                    var attr = new ComplexAttribute(type);
                    attr.SetProperties(propertiesMap, attributes);
                    attribute = attr;
                    break;
                default:
                    attribute = new Attribute("");
                    break;
            }
            attribute.SetProperties(propertiesMap);
        return attribute;
        }
    }
}
