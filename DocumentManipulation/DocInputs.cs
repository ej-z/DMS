using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DocumentManipulation
{
    public class DocInputs
    {
        Dictionary<string, Attributes> attributes = new Dictionary<string, Attributes>();
        Dictionary<string, Repeaters> repeaters = new Dictionary<string, Repeaters>();

        public Dictionary<string, Attributes> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public Dictionary<string, Repeaters> Repeaters
        {
            get { return repeaters; }
            set { repeaters = value; }
        }

        public Attributes AddAttribute(string name, string type)
        {
            if(!attributes.ContainsKey(name))
                attributes.Add(name, new Attributes(type));
            return attributes[name];
        }

        public Repeaters AddRepeater(string name)
        {
            if (!repeaters.ContainsKey(name))
                repeaters.Add(name, new Repeaters());
            return repeaters[name];
        }
    }

    public class Attributes 
    {
        string _value;

        public string Type;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Attributes(string type)
        {
            Type = type;
        }
    }

    public class Repeaters
    {
        Dictionary<string, Attributes> attributes = new Dictionary<string, Attributes>();
        List<Dictionary<string, Attributes>> attributeList = new List<Dictionary<string, Attributes>>();

        public void AddAttribute(int position, string name, string type)
        {
            var current = attributeList[position];
            if (!current.ContainsKey(name))
                current.Add(name, new Attributes(type));
        }

        public Attributes GetAttribute(int position, string name)
        {
            var current = attributeList[position];
            if (!current.ContainsKey(name))
                return null;
            return current[name];
        }

        public int AddAttributeCollection()
        {
            attributeList.Add(new Dictionary<string, Attributes>());
            return attributeList.Count() - 1;
        }

        public int Count { get { return attributeList.Count(); } }
    }
}
