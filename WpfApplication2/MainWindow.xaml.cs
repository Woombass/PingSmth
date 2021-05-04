using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Timers;
using Timer = System.Timers.Timer;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Timer tm = new Timer(2000);
        public ObservableCollection<Address> IpAddresses { get; set; }
        public ObservableCollection<Address> ProblemAddresses { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists($@"{Directory.GetCurrentDirectory()}\config.json"))
            {
                File.Create($@"{Directory.GetCurrentDirectory()}\config.json").Close();
            }

            tm.Enabled = false;
            tm.Elapsed += (o, args) => Pinging();
        

            IpAddresses = DataInteract.GetCollection();
            if (IpAddresses == null) IpAddresses = new ObservableCollection<Address>();
            dg_data.ItemsSource = IpAddresses;
        }

        private async void Pinging()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Проверьте подключение сети!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProblemAddresses = new ObservableCollection<Address>();
            Dispatcher.Invoke(() => { dg_ProblemIp.ItemsSource = ProblemAddresses; });
            IEnumerable<Task<PingReply>> tasks;
            tasks = IpAddresses.Select(selector: item => { return new Ping().SendPingAsync(item.IpAdd, 300); });
            var results = await Task.WhenAll(tasks);
            for (int i = 0; i < results.Length; i++)
            {
                IpAddresses[i].Status = results[i].Status.ToString();
                IpAddresses[i].Time = results[i].RoundtripTime;
                if (IpAddresses[i].Status != "Success")
                {
                    Dispatcher.Invoke(() => ProblemAddresses.Add(IpAddresses[i]));
                }
            }
        }

        private void Ping_button_OnClick(object sender, RoutedEventArgs e)
        {

            Pinging();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Ip_TextBox.Text) || string.IsNullOrWhiteSpace(Name_TextBox.Text))
            {
                MessageBox.Show("IP-адрес и наименование должны быть заполнены!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            var parse = DataInteract.IsIp(Ip_TextBox.Text);
            if (!parse)
            {
                MessageBox.Show("Проверьте правильность ввода IP адреса!");
                return;
            }

            IpAddresses.Add(new Address(Ip_TextBox.Text, Name_TextBox.Text));
            Ip_TextBox.Clear();
            Name_TextBox.Clear();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            foreach (var item in IpAddresses)
            {
                item.Status = String.Empty;
                item.Time = 0;
            }

            DataInteract.SaveCollection(IpAddresses);
        }

        private void Dg_data_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var grid = sender as DataGrid;
                if (grid.SelectedItem != null)
                {
                    IpAddresses.RemoveAt(grid.SelectedIndex);
                }
            }
        }

        private void StartTimer_Button_OnClick(object sender, RoutedEventArgs e)
        {
            tm.Enabled = true;
        }

        private void FillCollection_Button_OnClick(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < 10; i++)
            {
                IpAddresses.Add(new Address("8.8.8.8","Google"));
            }
        }

        private void StopTimer_Button_OnClick(object sender, RoutedEventArgs e)
        {
            tm.Enabled = false;
        }
    }
}