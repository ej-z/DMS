using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using DocumentFormat.OpenXml;

namespace DocumentManipulation
{
    public class DocInputs
    {
        Dictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();
        Dictionary<string, Repeater> repeaters = new Dictionary<string, Repeater>();

        public Dictionary<string, Attribute> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public void AddInput(string name, string type, string properties)
        {
            if (type == "Repeater")
            {
                AddRepeater(name, properties);
            }
            else if (name.Contains("."))
            {
                var repeaterName = name.Split('.')[0];
                var attributeName = name.Split('.')[1];
                repeaters[repeaterName].AddInput(attributeName,type, properties);
            }
            else
            {
                attributes.Add(name, AttributeFactory.Create(type, properties, attributes));
            }
        }

        public Dictionary<string, Repeater> Repeaters
        {
            get { return repeaters; }
            set { repeaters = value; }
        }

        private void AddRepeater(string name, string properties)
        {
            if (!repeaters.ContainsKey(name))
            {
                repeaters.Add(name, new Repeater());
                repeaters[name].SetProperties(properties.Split('|')
                .ToDictionary(x => x.Split(':')[0], x => x.Split(':').Length > 1 ? x.Split(':')[1] : string.Empty));
            }
        }
    }

    public class Attribute
    {
        string _value;

        public string Type;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public virtual string FinalValue { get { return _value; } }

        public string Label { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }
        public bool GridOnly { get; set; }
        public bool WindowOnly { get; set; }

        
        public Attribute(string type)
        {
            Type = type;
        }

        public virtual void SetProperties(Dictionary<string,string> properties)
        {
            Label = properties.ContainsKey(nameof(Label)) ? properties[nameof(Label)]:null;
            Row = properties.ContainsKey(nameof(Row)) ? Convert.ToInt32(properties[nameof(Row)]) : -1;
            Column = properties.ContainsKey(nameof(Column)) ? Convert.ToInt32(properties[nameof(Column)]) : -1;
            ColumnSpan = properties.ContainsKey(nameof(ColumnSpan)) ? Convert.ToInt32(properties[nameof(ColumnSpan)]) : -1;
            GridOnly = properties.ContainsKey(nameof(GridOnly)) && Convert.ToBoolean(properties[nameof(GridOnly)]);
            WindowOnly = properties.ContainsKey(nameof(WindowOnly)) && Convert.ToBoolean(properties[nameof(WindowOnly)]);
        }

        internal virtual Attribute Clone()
        {
            return new Attribute(Type);
        }
    }

    public class TextAttribute : Attribute
    {
        public TextAttribute(string type) : base(type)
        {

        }

        public string Prefix { get; set; }
        public string Suffix { get; set; }

        public override string FinalValue
        {
            get { return Prefix + base.Value + Suffix; }            
        }

        public override void SetProperties(Dictionary<string, string> properties)
        {
            base.SetProperties(properties);
            Prefix = properties[nameof(Prefix)];
            Suffix = properties[nameof(Suffix)];
        }
    }

    public class EnumAttribute : Attribute
    {
        public Dictionary<string, string> Values;

        public EnumAttribute(string type) : base(type)
        {

        }

        public override void SetProperties(Dictionary<string, string> properties)
        {
            base.SetProperties(properties);
            Values = properties[nameof(Values)].Split(',').ToDictionary(x => x.Split('~')[0], x => x.Split('~')[1]);
        }

        public override string FinalValue
        {
            get { return  string.IsNullOrEmpty(base.Value) ? null : Values[base.Value]; }
        }
    }

    public class BitAttribute : Attribute
    {
        public BitAttribute(string type) : base(type)
        {

        }

        public string Group { get; set; }

        public override void SetProperties(Dictionary<string, string> properties)
        {
            base.SetProperties(properties);
            Group = properties[nameof(Group)];
        }
    }

    public class LabelAttribute : Attribute
    {
        public LabelAttribute(string type) : base(type)
        {
            
        }
    }

    public class ImageAttribute : Attribute
    {
        public ImageAttribute(string type) : base(type)
        {

        }

        public BitmapImage Image { get; set; }

        public string Description { get; set; }
    }

    public class ComplexAttribute : Attribute
    {
        List<Attribute> Attributes = new List<Attribute>();
        public ComplexAttribute(string type) : base(type)
        {

        }

        public void AddAttributes(Attribute attribute)
        {
            Attributes.Add(attribute);
        }

        public override string FinalValue
        {
            get { return string.Join(", ", Attributes.Select(x => x.FinalValue).ToArray()); }
        }

        public void SetProperties(Dictionary<string, string> properties, Dictionary<string, Attribute> attributes)
        {
            var attributeList = properties[nameof(Attributes)].Split(',');
            foreach (var attributeName in attributeList)
            {
                Attributes.Add(attributes[attributeName]);
            }
        }

    }
}
