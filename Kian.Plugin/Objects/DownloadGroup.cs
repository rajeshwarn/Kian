using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kian.Core.Objects
{
    public class DownloadGroup : INotifyPropertyChanged
    {
        private string groupName;
        private ObservableCollection<Download> downloads = new ObservableCollection<Download>();

        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                if (groupName == value) return;
                groupName = value;
                NotifyPropertyChanged("GroupName");
            }
        }

        public ObservableCollection<Download> Downloads
        {
            get
            {
                return downloads;
            }
            set
            {
                if (downloads == value) return;
                downloads = value;
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