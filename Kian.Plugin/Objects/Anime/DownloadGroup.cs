using System.Collections.Generic;
using System.ComponentModel;

namespace Kian.Core.Objects.Anime
{
    public class DownloadGroup : INotifyPropertyChanged
    {
        private string _groupName;
        private List<Download> _downloads = new List<Download>();

        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                if (_groupName == value) return;
                _groupName = value;
                NotifyPropertyChanged("GroupName");
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
