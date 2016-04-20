using System;
using System.ComponentModel;
using System.Net;

namespace Kian.Objects.Anime
{
    public class Download : INotifyPropertyChanged
    {
        private string _fileName;
        private string _episodeName;
        private string _resolution;
        private string _downloadLink;
        private string _fileSize;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value) return;
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public string FileSize
        {
            get {
                if (_fileSize == null)
                {
                    _fileSize = "?";
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(DownloadLink);
                    req.Referer = "http://www.animebam.net/";
                    req.Method = "HEAD";
                    using (WebResponse resp = req.GetResponse())
                    {
                        int ContentLength;
                        if (int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                        {
                            _fileSize = HelperFunctions.GetBytesReadable(ContentLength);
                        }
                    }
                }

                return _fileSize;
            }
        }

        public string EpisodeName
        {
            get { return _episodeName; }
            set
            {
                if (_episodeName == value) return;
                _episodeName = value;
                NotifyPropertyChanged("EpisodeName");
            }
        }

        public string Resolution
        {
            get { return _resolution; }
            set
            {
                if (_resolution == value) return;
                _resolution = value;
                NotifyPropertyChanged("Resolution");
            }
        }

        public string DownloadLink
        {
            get { return _downloadLink; }
            set
            {
                if (_downloadLink == value) return;
                _downloadLink = value;
                NotifyPropertyChanged("DownloadLink");
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