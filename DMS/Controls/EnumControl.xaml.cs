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
    public partial class EnumControl : UserControl
    {
        DocumentManipulation.Attribute _attribute;
        public EnumControl(EnumAttribute attribute)
        {
            InitializeComponent();
            Label.Content = attribute.Label;
            this.SetValue(Grid.RowProperty, attribute.Row);
            this.SetValue(Grid.ColumnProperty, attribute.Column);
            this.SetValue(Grid.ColumnSpanProperty, attribute.ColumnSpan);

            foreach (var val in attribute.Values.Keys)
            {
                var radioButton = new RadioButton();
                radioButton.Content = val;
                radioButton.Click += new RoutedEventHandler(RadioButton_Click);
                if (val == attribute.Value)
                    radioButton.IsChecked = true;
                RadioGroup.Children.Add(radioButton);
            }
            _attribute = attribute;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            _attribute.Value = ((RadioButton)sender).Content.ToString();
        }
    }
}
