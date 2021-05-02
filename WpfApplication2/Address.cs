using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApplication2.Annotations;

namespace WpfApplication2
{
    public class Address : INotifyPropertyChanged
    {
        private string _status;
        private long _time;
        public string IpAdd { get; set; }
        public string Name { get; set; }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged();
                
            }
        }

        public long Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            } 
        }

        public Address(string ipAdd, string name)
        {
            Name = name;
            IpAdd = ipAdd;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}