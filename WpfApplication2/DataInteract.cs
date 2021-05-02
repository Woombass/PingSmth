using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfApplication2.Annotations;

namespace WpfApplication2
{
    public static  class DataInteract
    {
        private static string _path = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

        public static bool IsIp(string ip)
        {
            Regex regex = new Regex(@"\d*.\d*.\d*.\d*");
            Match match = regex.Match(ip);
            if (match.Success) return true;
            return false;
        }

        public static ObservableCollection<Address> GetCollection()
        {

            var jsonString = File.ReadAllText(_path);
            if (String.IsNullOrWhiteSpace(jsonString))
            {
                return new ObservableCollection<Address>();
            }
            return JsonSerializer.Deserialize<ObservableCollection<Address>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public  static void SaveCollection(ObservableCollection<Address> addresses)
        {
            File.WriteAllText(_path,JsonSerializer.Serialize(addresses)); 
           
        }
    }
}