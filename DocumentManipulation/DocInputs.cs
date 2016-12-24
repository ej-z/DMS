using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace DocumentManipulation
{
    public class DocInputs
    {
        Dictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();
        Dictionary<string, Repeater> repeaters = new Dictionary<string, Repeater>();
        Dictionary<string, ImageAttribute> imageAttributes = new Dictionary<string, ImageAttribute>();

        public Dictionary<string, Attribute> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public Dictionary<string, ImageAttribute> ImageAttributes
        {
            get { return imageAttributes; }
            set { imageAttributes = value; }
        }

        public Dictionary<string, Repeater> Repeaters
        {
            get { return repeaters; }
            set { repeaters = value; }
        }

        public Attribute AddAttribute(string name, string type)
        {
            if(!attributes.ContainsKey(name))
                attributes.Add(name, new Attribute(type));
            return attributes[name];
        }

        public ImageAttribute AddImageAttribute(string name, string type)
        {
            if (!imageAttributes.ContainsKey(name))
                imageAttributes.Add(name, new ImageAttribute(type));
            return imageAttributes[name];
        }

        public Repeater AddRepeater(string name)
        {
            if (!repeaters.ContainsKey(name))
                repeaters.Add(name, new Repeater());
            return repeaters[name];
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
        public Attribute(string type)
        {
            Type = type;
        }

        internal Attribute Clone()
        {
            return new Attribute(Type);
        }
    }

    public class Repeater
    {
        Dictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();
        List<Dictionary<string, Attribute>> attributeList = new List<Dictionary<string, Attribute>>();

        public List<Dictionary<string, Attribute>> AttributeList { get { return attributeList; } }

        internal void AddAttribute(int position, string name, string type)
        {
            var current = attributeList[position];
            if (!current.ContainsKey(name))
                current.Add(name, new Attribute(type));
        }

        public Attribute GetAttribute(int position, string name)
        {
            var current = attributeList[position];
            if (!current.ContainsKey(name))
                return null;
            return current[name];
        }

        internal int AddAttributeCollection()
        {
            attributeList.Add(new Dictionary<string, Attribute>());
            return LastPosition;
        }

        public int CloneLastAttributeCollection()
        {
            var current = attributeList[LastPosition];
            var newCollection = new Dictionary<string, Attribute>();            
            foreach(var attribute in current)
            {
                newCollection.Add(attribute.Key, attribute.Value.Clone());
            }
            attributeList.Add(newCollection);
            return LastPosition;
        }

        public int Count { get { return attributeList.Count(); } }

        public int LastPosition { get { return Count - 1; } }
    }

    public class ImageAttribute : Attribute
    {
        public ImageAttribute(string type) : base(type)
        {

        }

        public BitmapImage Image { get; set; }

        public string Description { get; set; }
    }
    }
