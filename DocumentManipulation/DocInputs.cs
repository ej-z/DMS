using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

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
                AddRepeater(name);
            }
            else if (name.Contains("."))
            {
                var repeaterName = name.Split('.')[0];
                var attributeName = name.Split('.')[1];
                repeaters[repeaterName].AddAttribute(attributeName,type, properties);
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

        private void AddRepeater(string name)
        {
            if (!repeaters.ContainsKey(name))
                repeaters.Add(name, new Repeater());
        }
    }

    public class Attribute
    {
        string _value;

        public string Type;

        public virtual string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Label { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }

        public Attribute(string type)
        {
            Type = type;
        }

        public virtual void SetProperties(Dictionary<string,string> properties)
        {
            Label = properties[nameof(Label)];
            Row = Convert.ToInt32(properties[nameof(Row)]);
            Column = Convert.ToInt32(properties[nameof(Column)]);
            ColumnSpan = Convert.ToInt32(properties[nameof(ColumnSpan)]);
        }

        internal Attribute Clone()
        {
            return new Attribute(Type);
        }
    }

    public class Repeater
    {
        readonly List<Dictionary<string, Attribute>> attributeList = new List<Dictionary<string, Attribute>>();

        public List<Dictionary<string, Attribute>> AttributeList => attributeList;

        public Repeater()
        {
            attributeList.Add(new Dictionary<string, Attribute>());
        }

        internal void AddAttribute(string name, string type, string properties)
        {
            var current = attributeList[LastPosition];
            if (!current.ContainsKey(name))
                current.Add(name, AttributeFactory.Create(type, properties, current));
        }

        public Attribute GetAttribute(int position, string name)
        {
            var current = attributeList[position];
            if (!current.ContainsKey(name))
                return null;
            return current[name];
        }

        public int CloneLastAttributeCollection()
        {
            var current = attributeList[LastPosition];
            var newCollection = new Dictionary<string, Attribute>();
            foreach (var attribute in current)
            {
                newCollection.Add(attribute.Key, attribute.Value.Clone());
            }
            attributeList.Add(newCollection);
            return LastPosition;
        }

        public int Count
        {
            get { return attributeList.Count(); }
        }

        public int LastPosition
        {
            get { return Count - 1; }
        }
    }

    public class TextAttribute : Attribute
    {
        public TextAttribute(string type) : base(type)
        {

        }

        public string Prefix { get; set; }
        public string Suffix { get; set; }

        public override string Value
        {
            get { return Prefix + base.Value + Suffix; }
            set { base.Value = value; }
        }

        public override void SetProperties(Dictionary<string, string> properties)
        {
            base.SetProperties(properties);
            Prefix = properties[nameof(Prefix)];
            Suffix = properties[nameof(Prefix)];
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

        //public override string Value
        //{
        //    get { returnbase.Value + Suffix; }
        //    set { base.Value = value; }
        //}
    }

    public class BitAttribute : Attribute
    {
        public BitAttribute(string type) : base(type)
        {

        }

        //public override string Value
        //{
        //    get { returnbase.Value + Suffix; }
        //    set { base.Value = value; }
        //}
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

        public override string Value
        {
            get { return string.Join(", ", Attributes.Select(x => x.Value).ToArray()); }
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
