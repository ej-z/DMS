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

namespace DMS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LogLocation.Content = Logger.LogLocation;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".docx";
            dlg.Filter = "Word documents (.docx)|*.docx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                FileName.Text = filename;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                DocManp d = new DocManp();
                DocInputs inputs = d.ReadDoc(FileName.Text);
                InputWindow iw = new InputWindow(inputs);
                iw.ShowDialog();
            }
            catch(Exception ex)
            {
                Logger.Log(ex);
            }
        }

        DocInputs _inputs;

        public void SetInputs(DocInputs inputs)
        {
            _inputs = inputs;
            DownloadButton.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GenerateDocument();
        }
        public void GenerateDocument()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".docx";
            dlg.Filter = "Word documents (.docx)|*.docx";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dlg.FileName;
                    DocManp d = new DocManp();
                    d.CreateDoc(_inputs, FileName.Text, filename);
                    System.Diagnostics.Process.Start(filename);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            DownloadButton.Visibility = Visibility.Hidden;            
        }
    }
}
