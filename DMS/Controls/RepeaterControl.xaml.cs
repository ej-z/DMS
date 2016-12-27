using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

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
            this.SetValue(Grid.RowProperty, repeater.Row);
            this.SetValue(Grid.ColumnProperty, repeater.Column);
            this.SetValue(Grid.ColumnSpanProperty, repeater.ColumnSpan);
            HeaderGrid.RowDefinitions.Add(new RowDefinition());
            int i = 0;
            foreach (var key in repeater.GridHeaders())
            {
                HeaderGrid.ColumnDefinitions.Add(new ColumnDefinition());
                var header = ControlFactory.GenerateRepeaterHeaderControl(key);
                header.SetValue(Grid.ColumnProperty, i++);
                HeaderGrid.Children.Add(header);
            }
        }

        private void AddNewRow(IEnumerable<string> values)
        {
            Grid row = new Grid();
            row.RowDefinitions.Add(new RowDefinition());
            int i = 0;
            foreach (var value in values)
            {
                row.ColumnDefinitions.Add(new ColumnDefinition());
                var cell = ControlFactory.GenerateRepeaterHeaderControl(value);
                cell.SetValue(Grid.ColumnProperty, i++);
                row.Children.Add(cell);
            }

            var item = new System.Windows.Controls.ListViewItem();
            item.Content = row;
            RepeaterHolder.Items.Add(item);
        }

        private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var i = repeater.CloneInput();
            InputWindow iw = new InputWindow(repeater.RepeaterData[i]);
            iw.ShowDialog();
            AddNewRow(repeater.CurrentRowValues());
        }

        private void Edit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string s = "";
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string s = "";
        }
    }
}
