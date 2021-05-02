using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
            ipAddresses = DataInteract.GetCollection();
            if (ipAddresses == null) ipAddresses = new ObservableCollection<Address>();
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
            problemAddresses = new ObservableCollection<Address>();
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
                    if (item.Status == "Success")
                    {
                        item.Time = Convert.ToInt32(el.RoundtripTime);

                    }
                    else
                    {
                        problemAddresses.Add(item);
                    }
                }

            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Ip_TextBox.Text) || string.IsNullOrWhiteSpace(Name_TextBox.Text))
            {
                MessageBox.Show("IP-адрес и наименование должны быть заполнены!", "Ошибка!", MessageBoxButton.OK);
                return;
            }

            IPAddress ip;
            //var parse = IPAddress.TryParse(Ip_TextBox.Text, out ip);
            var parse = DataInteract.IsIp(Ip_TextBox.Text);
            if (!parse)
            {
                MessageBox.Show("Проверьте правильность ввода IP адреса!");
                return;
            }
            ipAddresses.Add(new Address(Ip_TextBox.Text,Name_TextBox.Text));
            Ip_TextBox.Clear();
            Name_TextBox.Clear();
    
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            DataInteract.SaveCollection(ipAddresses);
        }

        private void Dg_data_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var grid = sender as DataGrid;
                if (grid.SelectedItem != null)
                {
                    ipAddresses.RemoveAt(grid.SelectedIndex);
                }
            }
            
        }
    }
}