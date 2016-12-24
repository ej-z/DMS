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
                controls.Children.Add(ControlFactory.GenerateAttributeControl(attribute.Key,attribute.Value));
            }

            foreach(var repeater in inputs.Repeaters)
            {
                controls.Children.Add(ControlFactory.GenerateRepeaterControl(repeater.Key, repeater.Value));
            }

            foreach (var imageAttribute in inputs.ImageAttributes)
            {
                controls.Children.Add(ControlFactory.GenerateImageControl(imageAttribute.Key, imageAttribute.Value));
            }

            //this.Topmost = true;
        }        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().First();
            mainWindow.SetInputs((DocInputs)DataContext);
            this.Close();
        }
    }
}
