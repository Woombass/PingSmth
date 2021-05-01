using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApplication2.Annotations;

namespace WpfApplication2
{
    public class Address : INotifyPropertyChanged
    {
        private string status;
        private long time;
        public string IpAdd { get; set; }
        public string Name { get; set; }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged();
                
            }
        }

        public long Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged();
            } 
        }

        public Address(string ip, string name)
        {
            Name = name;
            IpAdd = ip;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}