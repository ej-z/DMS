using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DocumentManipulation
{
    public class Repeater
    {
        private struct InputInfo
        {
            public string Name;
            public string Type;
            public string Properties;
        }

        private List<InputInfo> InputData = new List<InputInfo>();

        private DocInputs originalInput = new DocInputs();

        public List<DocInputs> RepeaterData = new List<DocInputs>();

        public string Label { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }

        public string CountLabel { get; set; }

        internal void AddInput(string name, string type, string properties)
        {
            InputData.Add(new InputInfo() {Name = name, Type = type, Properties = properties});
            originalInput.AddInput(name, type, properties);
        }

        public Attribute GetAttribute(int position, string name)
        {
            var current = RepeaterData[position].Attributes;
            if (!current.ContainsKey(name))
                return null;
            return current[name];
        }

        public IEnumerable<Header> GridHeaders()
        {
            return originalInput.Attributes.Values.Where(x => !x.WindowOnly).Select((x, i) => new Header(){ Label = x.Label, Index = i });
        }

        public IEnumerable<Attribute> RowValues(int position)
        {
            var current = RepeaterData[position].Attributes;
            return current.Values.Where(x => !x.WindowOnly);
        }

        public void RemoveAt(int position)
        {
            RepeaterData.RemoveAt(position);
        }

        public int CloneInput()
        {
            var input = new DocInputs();
            foreach (var inputInfo in InputData)
            {
                input.AddInput(inputInfo.Name,inputInfo.Type,inputInfo.Properties);
            }
            RepeaterData.Add(input);
            return LastPosition;
        }

        public int Count
        {
            get { return RepeaterData.Count(); }
        }

        public int LastPosition
        {
            get { return Count - 1; }
        }

        public virtual void SetProperties(Dictionary<string, string> properties)
        {
            Label = properties.ContainsKey(nameof(Label)) ? properties[nameof(Label)] : null;
            Row = properties.ContainsKey(nameof(Row)) ? Convert.ToInt32(properties[nameof(Row)]) : -1;
            Column = properties.ContainsKey(nameof(Column)) ? Convert.ToInt32(properties[nameof(Column)]) : -1;
            ColumnSpan = properties.ContainsKey(nameof(ColumnSpan)) ? Convert.ToInt32(properties[nameof(ColumnSpan)]) : -1;
            CountLabel = properties.ContainsKey(nameof(CountLabel)) ? properties[nameof(CountLabel)] : null;
        }
    }

    public class Header
    {
        public string Label;
        public int Index;
    }
}
