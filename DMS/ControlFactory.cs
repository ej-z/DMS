using DMS.Controls;
using DocumentManipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DMS
{
    class ControlFactory
    {
        public static Control GenerateAttributeControl(string name, DocumentManipulation.Attribute attribute)
        {
            switch (attribute.Type)
            {
                case "Text":
                    return new TextControl((TextAttribute)attribute);
                case "TextArea":
                    return new TextAreaControl((TextAttribute)attribute);
                case "Bit":
                    return new BitControl((BitAttribute)attribute);
                case "Label":
                    return new LabelControl((LabelAttribute)attribute);
                case "Enum":
                    return new EnumControl((EnumAttribute)attribute);
                case "Image":
                    return new ImageControl((ImageAttribute)attribute);
                case "File":
                    return new FileControl((FileAttribute)attribute);
            }

            return null; 
        }

        public static Control GenerateRepeaterControl(string name, DocumentManipulation.Repeater repeater)
        {
            return new RepeaterControl(name, repeater);
        }
    }
}
