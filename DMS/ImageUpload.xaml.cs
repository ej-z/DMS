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
    /// Interaction logic for ImageUpload.xaml
    /// </summary>
    public partial class ImageUpload : UserControl
    {
        private DocumentManipulation.ImageAttribute _attribute;
        public ImageUpload(string name, DocumentManipulation.ImageAttribute attribute)
        {
            InitializeComponent();
            _attribute = attribute;
            OnLoad(attribute);
            Label.Content = name;
        }

        public BitmapImage Image { get; set; }


        public void OnLoad(DocumentManipulation.ImageAttribute attribute)
        {
            Binding binding = new Binding();
            binding.Path = new PropertyPath("Value");
            binding.Source = attribute;
            FileName.SetBinding(TextBox.TextProperty, binding);


            binding = new Binding();
            binding.Path = new PropertyPath("Description");
            binding.Source = attribute;
            ImageDescription.SetBinding(TextBox.TextProperty, binding);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.jpg";
            dlg.Filter = "Image Files|*.jpg";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                FileName.Text = filename;
                Image = new BitmapImage(new Uri(filename));
                _attribute.Image = Image;
            }
        }
    }
}
