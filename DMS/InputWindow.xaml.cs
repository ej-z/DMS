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
using System.Windows.Shapes;

namespace DMS
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow(DocInputs inputs)
        {
            InitializeComponent();

            this.DataContext = inputs;

            foreach(var attribute in inputs.Attributes)
            {
                controls.Children.Add(GenerateControl(attribute.Key,attribute.Value));
            }

            this.Topmost = true;
        }

        private Control GenerateControl(string name, Attributes attribute)
        {
            return new InputControl(name, StringControl(name, attribute));
            
                switch (attribute.Type)
            {
                case "String":
                    return StringControl(name, attribute);
                case "Number":
                    return null;
                case "Date":
                    return null;
                case "TextArea":
                    return null;
            }

            return null;
        }

        private TextBox StringControl(string name, Attributes attribute)
        {
            var textBox = new TextBox() { Height = 20, Width = 200 };
            Binding binding = new Binding();
            binding.Path = new PropertyPath("Value");
            binding.Source = attribute;
            textBox.SetBinding(TextBox.TextProperty, binding);
            return textBox;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().First();
            mainWindow.SetInputs((DocInputs)DataContext);
            this.Close();
        }
    }
}
