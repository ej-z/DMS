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
            int i = 0;
            foreach (var key in repeater.GridHeaders())
            {
                HeaderGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                var header = ControlFactory.GenerateRepeaterHeaderControl(key);
                header.SetValue(Grid.ColumnProperty, i++);
                HeaderGrid.Children.Add(header);
            }
        }       

        private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var i = repeater.CloneInput();
            InputWindow iw = new InputWindow(repeater.RepeaterData[i]);
            iw.ShowDialog();
            AddRow(repeater.RowValues(i));
        }

        private void AddRow(IEnumerable<string> values)
        {
            Grid row = new Grid();
            int i = 0;
            foreach (var value in values)
            {
                row.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                var cell = ControlFactory.GenerateRepeaterRowControl(value);
                cell.SetValue(Grid.ColumnProperty, i++);
                row.Children.Add(cell);
            }

            var item = new System.Windows.Controls.ListViewItem();
            item.Content = row;
            RepeaterHolder.Items.Add(item);
        }

        private void Edit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (RepeaterHolder.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Please select a row.");
                return;
            }
            var i = RepeaterHolder.Items.IndexOf(RepeaterHolder.SelectedItems[0]);
            InputWindow iw = new InputWindow(repeater.RepeaterData[i]);
            iw.ShowDialog();
            EditRow(repeater.RowValues(i), (Grid)((System.Windows.Controls.ListViewItem)RepeaterHolder.SelectedItems[0]).Content);
        }

        private void EditRow(IEnumerable<string> values, Grid row)
        {
            int i = 0;
            row.Children.Clear();
            foreach (var value in values)
            {
                var cell = ControlFactory.GenerateRepeaterRowControl(value);
                cell.SetValue(Grid.ColumnProperty, i++);
                row.Children.Add(cell);
            }
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (RepeaterHolder.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Please select a row.");
                return;
            }
            var i = RepeaterHolder.Items.IndexOf(RepeaterHolder.SelectedItems[0]);
            RepeaterHolder.Items.RemoveAt(i);
            repeater.RemoveAt(i);
        }
    }
}
