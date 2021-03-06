﻿using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kian.Core.Objects
{
    public class Episode : INotifyPropertyChanged
    {
        private ObservableCollection<Download> downloads;
        private string episodeName;
        private string episodeUrl;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public string EpisodeName
        {
            get
            {
                return episodeName;
            }
            set
            {
                if (episodeName == value) return;
                episodeName = value;
                NotifyPropertyChanged("EpisodeName");
            }
        }

        public string EpisodeUrl
        {
            get
            {
                return episodeUrl;
            }

            set
            {
                if (episodeUrl == value) return;
                episodeUrl = value;
                NotifyPropertyChanged("EpisodeUrl");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}