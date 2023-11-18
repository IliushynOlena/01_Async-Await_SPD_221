using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _01_Async_Await_SPD_221
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //async - allow method to use await
            //await - wait task without freezing
            //int value = GenerateValue();//freeze
            //Task<int> task =  Task.Run(GenerateValue);
            //
            //listBox.Items.Add(task.Result);//freeze
            //task.Wait();//freeze

            ////
            ///
            //int value = await task;
            //int value = await Task.Run(GenerateValue);

            //int value = await GenerateValueAsync();
            ///
            //MessageBox.Show("Continue.........");
            //listBox.Items.Add(value);
            listBox.Items.Add(await GenerateValueAsync());
           // GenerateValueAsync();
        }

        int GenerateValue()
        {
            Thread.Sleep(random.Next(5000));
            //MessageBox.Show("Generated!!!");
            return random.Next(1000);
        }
        private Task<int> GenerateValueAsync()
        {
            return Task.Run(() =>
            {
                Thread.Sleep(random.Next(5000));
                //MessageBox.Show("Generated!!!");
                return random.Next(1000);
            });
           
        }
    }
}
