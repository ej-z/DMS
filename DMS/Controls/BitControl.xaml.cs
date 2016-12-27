using DocumentManipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DMS.Controls
{
    /// <summary>
    /// Interaction logic for TextControl.xaml
    /// </summary>   

    public partial class BitControl : UserControl
    {
        DocumentManipulation.BitAttribute _attribute;
        public BitControl(BitAttribute attribute)
        {
            InitializeComponent();
            Label.Content = attribute.Label;
            this.SetValue(Grid.RowProperty, attribute.Row);
            this.SetValue(Grid.ColumnProperty, attribute.Column);
            this.SetValue(Grid.ColumnSpanProperty, attribute.ColumnSpan);
            _attribute = attribute;
            if (attribute.Value == "True")
                CheckBox.IsChecked = true;
        }

        private void CheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _attribute.Value = "True";
        }

        private void CheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _attribute.Value = "False";
        }
    }
}
