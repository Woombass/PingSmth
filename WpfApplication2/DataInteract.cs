using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using Newtonsoft.Json;
namespace WpfApplication2
{
    public static  class DataInteract
    {
        private static string path = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

        public static bool IsIp(string ip)
        {
            Regex regex = new Regex(@"\d*.\d*.\d*.\d*");
            Match match = regex.Match(ip);
            if (match.Success) return true;
            return false;
        }

        public static ObservableCollection<Address> GetCollection()
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<ObservableCollection<Address>>(File.ReadAllText(path));
            }

            return new ObservableCollection<Address>();
        }

        public static void SaveCollection(ObservableCollection<Address> addresses)
        {
;
        
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            var output = JsonConvert.SerializeObject(addresses);
            File.WriteAllText(path,output);
        }
    }
}