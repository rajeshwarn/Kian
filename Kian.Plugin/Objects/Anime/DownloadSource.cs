using Kian.Core.Objects.Anime;
using System.Collections.Generic;
using System.ComponentModel;

namespace Kian.Core.Objects.Anime
{
    public class DownloadSource : INotifyPropertyChanged
    {
        private string _name;
        private List<DownloadGroup> _downloadGroups = new List<DownloadGroup>();

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

        public List<DownloadGroup> DownloadGroups
        {
            get
            {
                return _downloadGroups;
            }
            set
            {
                if (_downloadGroups == value) return;
                _downloadGroups = value;
                NotifyPropertyChanged("DownloadGroups");
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