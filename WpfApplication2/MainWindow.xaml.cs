using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ipAddresses = new ObservableCollection<Address>();
            ipAddresses.Add(new Address("192.168.9.1","Микрота"));
            ipAddresses.Add(new Address("8.8.8.8","Google"));
            ipAddresses.Add(new Address("192.168.9.236","Ноут"));

            lv_data.ItemsSource = ipAddresses;
        }

        private void Ping_button_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in ipAddresses)
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(item.IpAdd);
                item.Status = Convert.ToString(reply.Status);
                if (Convert.ToString(item.Status) == "Success")
                {
                    item.Time = Convert.ToInt64(reply.RoundtripTime);
                }
            }
        }
    }
}