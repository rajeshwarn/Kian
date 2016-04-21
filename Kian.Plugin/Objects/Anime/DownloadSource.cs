using System.Collections.Generic;
using System.ComponentModel;

namespace Kian.Objects.Anime
{
    public class DownloadSource : INotifyPropertyChanged
    {
        private string _name;
        private List<Download> _downloads = new List<Download>();

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public List<Download> Downloads
        {
            get
            {
                return _downloads;
            }
            set
            {
                if (_downloads == value) return;
                _downloads = value;
                NotifyPropertyChanged("Downloads");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}