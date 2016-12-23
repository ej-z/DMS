using System.Linq;
using System.Windows.Controls;

namespace DMS
{
    /// <summary>
    /// Interaction logic for RepeaterControl.xaml
    /// </summary>
    public partial class RepeaterControl : UserControl
    {
        public RepeaterControl(string label, DocumentManipulation.Repeater repeater)
        {
            InitializeComponent();
            var initialRow = repeater.AttributeList[repeater.LastPosition];
            int i = 0;
            foreach(var key in initialRow.Keys)
            {
                RepeaterGrid.ColumnDefinitions.Add(new ColumnDefinition());
                var header = ControlFactory.GenerateLabelControl(key);
                header.SetValue(Grid.ColumnProperty, i++);
                RepeaterGrid.Children.Add(header);
            }
        }
    }
}
