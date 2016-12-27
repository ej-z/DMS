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
    public partial class TextAreaControl : UserControl
    {
        public TextAreaControl(TextAttribute attribute)
        {
            InitializeComponent();
            Label.Content = attribute.Label;
            Binding binding = new Binding();
            binding.Path = new PropertyPath("Value");
            binding.Source = attribute;
            TextBox.SetBinding(TextBox.TextProperty, binding);
            this.SetValue(Grid.RowProperty, attribute.Row);
            this.SetValue(Grid.ColumnProperty, attribute.Column);
            this.SetValue(Grid.ColumnSpanProperty, attribute.ColumnSpan);
        }
    }
}
