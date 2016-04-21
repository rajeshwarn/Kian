using Kian.Core.MyAnimeList.Objects.API.Anime;
using System.Collections.Generic;
using System.ComponentModel;

namespace Kian.Core.Objects.Anime
{
    public class Anime : INotifyPropertyChanged
    {
        private string _title;
        private List<string> _tags;
        private animeEntry _malData;
        private List<Objects.Anime.DownloadSource> _downloadData;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public List<string> Tags
        {
            get { return _tags; }
            set
            {
                if (_tags == value) return;
                _tags = value;
                NotifyPropertyChanged("Tags");
            }
        }

        public animeEntry MalData
        {
            get { return _malData; }
            set
            {
                if (_malData == value) return;
                _malData = value;
                NotifyPropertyChanged("MalData");
            }
        }

        public List<Objects.Anime.DownloadSource> DownloadSources
        {
            get { return _downloadData; }
            set
            {
                if (_downloadData == value) return;
                _downloadData = value;
                NotifyPropertyChanged("DownloadSources");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayMember"));
            }
        }
    }
}