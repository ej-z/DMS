using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace DMS
{
    /// <summary>
    /// Interaction logic for RepeaterControl.xaml
    /// </summary>
    public partial class RepeaterControl : UserControl
    {
        int row = 1;
        DocumentManipulation.Repeater repeater;
        public RepeaterControl(string label, DocumentManipulation.Repeater repeater)
        {
            this.repeater = repeater;
            InitializeComponent();
            Label.Content = label;
            var initialRow = repeater.AttributeList[repeater.LastPosition];
            RepeaterGrid.RowDefinitions.Add(new RowDefinition());
            int i = 0;
            foreach(var key in initialRow.Keys)
            {
                RepeaterGrid.ColumnDefinitions.Add(new ColumnDefinition());
                var header = ControlFactory.GenerateRepeaterHeaderControl(key);
                header.SetValue(Grid.ColumnProperty, i++);
                RepeaterGrid.Children.Add(header);
            }
            AddNewRow(initialRow);
        }

        private void AddNewRow(Dictionary<string, DocumentManipulation.Attribute> attributeList)
        {
            RepeaterGrid.RowDefinitions.Add(new RowDefinition());
            int i = 0;
            foreach (var attribute in attributeList)
            {
                var control = ControlFactory.GenerateControl(attribute.Key, attribute.Value);
                control.SetValue(Grid.ColumnProperty, i++);
                control.SetValue(Grid.RowProperty, row);
                RepeaterGrid.Children.Add(control);                
            }
            row++;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            repeater.CloneLastAttributeCollection();
            var newRow = repeater.AttributeList[repeater.LastPosition];
            AddNewRow(newRow);
        }
    }
}
