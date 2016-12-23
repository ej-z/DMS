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

namespace DMS
{
    /// <summary>
    /// Interaction logic for InputControl.xaml
    /// </summary>
    public partial class AttributeControl : UserControl
    {
        public AttributeControl(string label, Control control)
        {
            InitializeComponent();
            Label.Content = label;
            Content.Children.Add(control);
        }
    }
}
