﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DMS
{
    class ControlFactory
    {
        public static Control GenerateAttributeControl(string name, DocumentManipulation.Attribute attribute)
        {
            return new AttributeControl(name, StringControl(name, attribute));
            switch (attribute.Type)
            {
                case "String":
                    return StringControl(name, attribute);
                case "Number":
                    return null;
                case "Date":
                    return null;
                case "TextArea":
                    return null;
            }

            return null;
        }

        public static Control GenerateControl(string name, DocumentManipulation.Attribute attribute)
        {
            return StringControl(name, attribute);
            switch (attribute.Type)
            {
                case "String":
                    return StringControl(name, attribute);
                case "Number":
                    return null;
                case "Date":
                    return null;
                case "TextArea":
                    return null;
            }

            return null;
        }

        private static TextBox StringControl(string name, DocumentManipulation.Attribute attribute)
        {
            var textBox = new TextBox() { Height = 20, Width = 200 };
            Binding binding = new Binding();
            binding.Path = new PropertyPath("Value");
            binding.Source = attribute;
            textBox.SetBinding(TextBox.TextProperty, binding);
            return textBox;
        }

        public static Control GenerateRepeaterControl(string name, DocumentManipulation.Repeater repeater)
        {
            return new RepeaterControl(name, repeater);
            //switch (attribute.Type)
            //{
            //    case "String":
            //        return StringControl(name, attribute);
            //    case "Number":
            //        return null;
            //    case "Date":
            //        return null;
            //    case "TextArea":
            //        return null;
            //}

            //return null;
        }

        public static Label GenerateRepeaterHeaderControl(string name)
        {
            var label = new Label();
            label.Content = name;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.FontSize = 12;
            label.FontWeight = FontWeights.Bold;
            return label;
        }
    }
}
