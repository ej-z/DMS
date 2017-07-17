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

    public partial class FileControl : UserControl
    {
        DocumentManipulation.FileAttribute _attribute;
        public FileControl(FileAttribute attribute)
        {
            InitializeComponent();
            _attribute = attribute;
            Label.Content = attribute.Label;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = _attribute.Filter.Trim().Replace("&pipe;","|");
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                _attribute.Address = TextBox.Text = filename;
            }
        }
    }
}
