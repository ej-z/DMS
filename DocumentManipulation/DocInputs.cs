using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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

        public event PropertyChangedEventHandler PropertyChanged;
        public Attribute(string type)
        {
            Type = type;
        }
    }

    public class Repeater
    {
        Dictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();
        List<Dictionary<string, Attribute>> attributeList = new List<Dictionary<string, Attribute>>();

        public void AddAttribute(int position, string name, string type)
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

        public int AddAttributeCollection()
        {
            attributeList.Add(new Dictionary<string, Attribute>());
            return attributeList.Count() - 1;
        }

        public int Count { get { return attributeList.Count(); } }
    }
}
