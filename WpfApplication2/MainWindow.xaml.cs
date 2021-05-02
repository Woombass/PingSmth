using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //private ObservableCollection<Address> ipAddresses = new ObservableCollection<Address>();
        public ObservableCollection<Address> ipAddresses { get; set; }
        public ObservableCollection<Address> problemAddresses { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ipAddresses = new ObservableCollection<Address>();
            problemAddresses = new ObservableCollection<Address>();
            ipAddresses.Add(new Address("192.168.9.1","Микрота"));
            ipAddresses.Add(new Address("8.8.8.8","Google"));
            ipAddresses.Add(new Address("192.168.9.236","Ноут"));
            for (int i = 0; i < 20; i++)
            {
                ipAddresses.Add(new Address("8.8.8.8","Тест " + i));
            }
            dg_data.ItemsSource = ipAddresses;

        }

        async Task<PingReply> ping(string address)
        {
            PingReply pr = null;
            try
            {
                pr = await new Ping().SendPingAsync(address);
            }
            catch (Exception e)
            {
                return null;
            }

            return pr;
        }
        
        // private async Task<List<PingReply>> PingAsync()
        // {
        //     Ping pingSender = new Ping();
        //     var tasks = ipAddresses.Select((Machine => pingSender.SendPingAsync(Machine.IpAdd)));
        //     var results = await Task.WhenAll(tasks);
        //
        //     return results.ToList();
        // }
        private async void Ping_button_OnClick(object sender, RoutedEventArgs e)
        {
            dg_ProblemIp.ItemsSource = problemAddresses;
            foreach (var item in ipAddresses)
            {
                item.Status = String.Empty;
                item.Time = 0;
                var el = await ping(item.IpAdd);
                if (el == null)
                {
                    MessageBox.Show("Проверьте подключение к сети!", "Ошибка!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                if (el != null)
                {
                    item.Status = el.Status.ToString();
                    if (item.Status == "Success") item.Time = Convert.ToInt32(el.RoundtripTime);
                    else
                    {
                        problemAddresses.Add(item);
                    }
                }

            }
        }
    }
}