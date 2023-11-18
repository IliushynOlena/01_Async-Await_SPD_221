using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace _02_flie_copy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Sourse { get; set; }
        public string Destination { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            sourseTBox.Text = Sourse = @"C:\Users\helen\Downloads\Wpf.Mvvm.Evolution.Final.zip";
            destTBox.Text = Destination = @"C:\Users\helen\Desktop\Test - Copy";

        }

        private async void Copy_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            string filename = Path.GetFileName(sourseTBox.Text);
            string destinationPath = Path.Combine(Destination, filename);
            await CopyFileAsync(Sourse, destinationPath);
            Console.WriteLine("Complited!!!");


        }//}//destStream.Dispose(); //}//srcStream.Dispose();  

        private Task CopyFileAsync(string src, string dest)
        {
            return Task.Run(() =>
            {
                ////1 - using File class
                //File.Copy(Sourse, destinationPath, true);
                //MessageBox.Show("Complited!!!");

                //2 - using FileStream
                using FileStream srcStream = new FileStream(Sourse, FileMode.Open, FileAccess.Read);
                using FileStream destStream = new FileStream(dest, FileMode.Create, FileAccess.Write);

                byte[] buffer = new byte[1024 * 8];//8Kb
                int bytes = 0;
                do
                {
                    bytes = srcStream.Read(buffer, 0, buffer.Length);
                    destStream.Write(buffer, 0, bytes);
                    float procent = destStream.Length / (srcStream.Length / 100);
                    progressBar.Value = procent;

                } while (bytes > 0);
            });

        }
        private void OpenSourceBtnClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if( dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Sourse = dialog.FileName;
                sourseTBox.Text = Sourse;
                //MessageBox.Show(Sourse);
            }
        }

        private void OpenDestBtn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Destination = dialog.FileName;
                destTBox.Text = Destination;
                //MessageBox.Show(Destination);
            }
        }


    }
}
