using IOExtensions;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ViewModel model; 
        public MainWindow()
        {
            InitializeComponent();
            model = new ViewModel()
            {
                Sourse = @"C:\Users\helen\Downloads\1GB.bin",
                Destination = @"C:\Users\helen\Desktop\Testtt",
                Progress = 0
            };
            this.DataContext = model;
        }

        private async void Copy_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            string filename = Path.GetFileName(model.Sourse);
            string destinationPath = Path.Combine(model.Destination, filename);
            //add item to list
            CopyFileInfo copyFileInfo = new CopyFileInfo()
            {
                FileName = filename,
                Percentage = 0,
                BytesPerSecond = 0
            };

            model.AddProcess(copyFileInfo);
            await CopyFileAsync(model.Sourse, destinationPath, copyFileInfo);
            copyFileInfo.Percentage = 100;

           MessageBox.Show("Complited!!!");


        }//}//destStream.Dispose(); //}//srcStream.Dispose();  
        private Task CopyFileAsync(string src, string dest, CopyFileInfo copyFileInfo)
        {
            return Task.Run(() =>
            {
                ////1 - using File class
                //File.Copy(Sourse, destinationPath, true);
                //MessageBox.Show("Complited!!!");

                /*
                //2 - using FileStream
                using FileStream srcStream = new FileStream(model.Sourse, FileMode.Open, FileAccess.Read);
                using FileStream destStream = new FileStream(dest, FileMode.Create, FileAccess.Write);

                byte[] buffer = new byte[1024 * 8];//8Kb
                int bytes = 0;
                do
                {
                    bytes = srcStream.Read(buffer, 0, buffer.Length);
                    destStream.Write(buffer, 0, bytes);
                    float procent = destStream.Length / (srcStream.Length / 100);
                    model.Progress = procent;
                    //MessageBox.Show(model.Progress.ToString());

                } while (bytes > 0);
                */
                return FileTransferManager.CopyWithProgressAsync(src, dest, (progress) =>
                {
                    //model.Progress = progress.Percentage;
                    copyFileInfo.Percentage = progress.Percentage;
                    copyFileInfo.BytesPerSecond = progress.BytesPerSecond;//Mb/s
                }, false);
            });

        }
        private void OpenSourceBtnClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if( dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                model.Sourse = dialog.FileName;
            }
        }

        private void OpenDestBtn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                model.Destination = dialog.FileName;
            }
        }


    }
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        public string Sourse { get; set; }
        public string Destination { get; set; }
        public double Progress { get; set; }
        public bool IsWaiting => Progress == 0;
        private ObservableCollection<CopyFileInfo> files;
        public IEnumerable<CopyFileInfo> Files => files;//read only
        public ViewModel()
        {
            files = new ObservableCollection<CopyFileInfo>();
        }
        public void AddProcess(CopyFileInfo info)
        {
            files.Add(info);
        }

    }
    [AddINotifyPropertyChangedInterface]
    class CopyFileInfo
    {
        public string FileName { get; set; }
        public double Percentage { get; set; }
        public int PercentageInt => (int)Percentage;
        public double BytesPerSecond { get; set; }
        public double MegaBytesPerSecond => Math.Round(BytesPerSecond / 1024 / 1024, 1);

    }
}
