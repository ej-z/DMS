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
            }

            return new TextBox();
        }

        public static Control GenerateRepeaterControl(string name, DocumentManipulation.Repeater repeater)
        {
            return new RepeaterControl(name, repeater);
        }

        public static Label GenerateRepeaterHeaderControl(string name)
        {
            var label = GenerateRepeaterRowControl(name);
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.FontWeight = FontWeights.Bold;
            return label;
        }

        public static Label GenerateRepeaterRowControl(string name)
        {
            var label = new Label();
            label.Content = name;
            label.BorderBrush = Brushes.Black;
            label.BorderThickness = new Thickness(0.5);
            return label;
        }
    }
}
