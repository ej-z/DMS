using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DocumentManipulation;
using UserControl = System.Windows.Controls.UserControl;
using System.Windows.Controls;

namespace DMS
{
    /// <summary>
    /// Interaction logic for RepeaterControl.xaml
    /// </summary>
    public partial class RepeaterControl : UserControl
    {
        DocumentManipulation.Repeater repeater;

        private ObservableCollection<Record> _records;

        ObservableCollection<Record> Records { get { return _records; } set { _records = value; } }

        public RepeaterControl(string label, DocumentManipulation.Repeater repeater)
        {
            this.repeater = repeater;
            InitializeComponent();
            Label.Content = label;
            this.SetValue(Grid.RowProperty, repeater.Row);
            this.SetValue(Grid.ColumnProperty, repeater.Column);
            this.SetValue(Grid.ColumnSpanProperty, repeater.ColumnSpan);
            this.DataContext = this;
            _records = new ObservableCollection<Record>();
            dataGrid.ItemsSource = Records;

            foreach (var header in repeater.GridHeaders())
            {
                var binding = new Binding($"Attributes[{header.Index}]");
                dataGrid.Columns.Add(new CustomBoundColumn() { Header = header.Label, Binding = binding, TemplateName = "CustomTemplate" });
            }
            SetCountLabel();
        }       

        private void SetCountLabel()
        {
            CountLabel.Content = repeater.CountLabel + " " + repeater.Count;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var i = repeater.CloneInput();
            InputWindow iw = new InputWindow(repeater.RepeaterData[i]);
            iw.ShowDialog();
            if (!iw.wasSubmited)
            {
                repeater.RemoveAt(i);
                return;
            }
            _records.Add(new Record(repeater.RowValues(i)));
            SetCountLabel();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a row.");
                return;
            }
            var i = dataGrid.Items.IndexOf(dataGrid.SelectedItems[0]);
            InputWindow iw = new InputWindow(repeater.RepeaterData[i]);
            iw.ShowDialog();
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = Records;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a row.");
                return;
            }
            var i = dataGrid.Items.IndexOf(dataGrid.SelectedItems[0]);
            _records.RemoveAt(i);
            repeater.RemoveAt(i);
            SetCountLabel();
        }
    }

    public class CustomBoundColumn : DataGridBoundColumn
    {
        public string TemplateName { get; set; }

        public CustomBoundColumn()
        {
            Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var binding = new Binding(((Binding)Binding).Path.Path);
            binding.Source = dataItem;

            var content = new ContentControl();
            content.ContentTemplate = (DataTemplate)cell.FindResource(TemplateName);
            content.SetBinding(ContentControl.ContentProperty, binding);
            return content;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateElement(cell, dataItem);
        }
    }
    public class Record
    {
        private readonly ObservableCollection<Attribute> attributes = new ObservableCollection<Attribute>();

        public Record(IEnumerable<Attribute> attributes)
        {
            foreach (var attribute in attributes)
                Attributes.Add(attribute);
        }

        public void Edit(IEnumerable<Attribute> newAttributes)
        {
            var newAttr = newAttributes.ToArray();
            for (int i = 0; i < newAttr.Count(); i++)
                this.attributes[i] = newAttr[i];
        }

        public ObservableCollection<Attribute> Attributes
        {
            get { return attributes; }
        }
    }
}
