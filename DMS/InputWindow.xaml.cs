﻿using DocumentManipulation;
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
            var rowCount = inputs.Attributes.Select(x => x.Value.Row).Max() + inputs.Repeaters.Count;
            var colCount = inputs.Attributes.Select(x => x.Value.Column).Max();

            for( int i =0; i <= rowCount; i++)
            {
                Controls.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i <= colCount; i++)
            {
                Controls.ColumnDefinitions.Add(new ColumnDefinition());
            }

            foreach (var attribute in inputs.Attributes.Where(x => !x.Value.GridOnly))
            {
                Controls.Children.Add(ControlFactory.GenerateAttributeControl(attribute.Key,attribute.Value));
            }

            foreach(var repeater in inputs.Repeaters)
            {
                Controls.Children.Add(ControlFactory.GenerateRepeaterControl(repeater.Key, repeater.Value));
            }
        }        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().First();
            mainWindow.SetInputs((DocInputs)DataContext);
            this.Close();
        }
    }
}
