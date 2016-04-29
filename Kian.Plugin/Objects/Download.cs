using System.ComponentModel;
using System.Net;

namespace Kian.Core.Objects
{
    public class Download : INotifyPropertyChanged
    {
        private string fileName;
        private string episodeName;
        private int resolution;
        private string downloadLink;
        private string fileSize;
        private string sizeRequestReferrer;

        public string SizeRequestReferrer
        {
            get { return sizeRequestReferrer; }
            set
            {
                if (sizeRequestReferrer == value) return;
                sizeRequestReferrer = value;
                NotifyPropertyChanged("SizeRequestReferrer");
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName == value) return;
                fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public string FileSize
        {
            get
            {
                if (fileSize == null)
                {
                    fileSize = "?";

                    BackgroundWorker bw = new BackgroundWorker();

                    bw.DoWork += delegate
                    {
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(DownloadLink);
                        req.Referer = SizeRequestReferrer;
                        req.Method = "HEAD";
                        using (WebResponse resp = req.GetResponse())
                        {
                            int ContentLength;
                            if (int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                            {
                                fileSize = HelperFunctions.GetBytesReadable(ContentLength);
                            }
                        }
                    };

                    bw.RunWorkerCompleted += delegate
                    {
                        NotifyPropertyChanged("FileSize");
                    };

                    bw.RunWorkerAsync();
                }

                return fileSize;
            }
        }

        public string EpisodeName
        {
            get { return episodeName; }
            set
            {
                if (episodeName == value) return;
                episodeName = value;
                NotifyPropertyChanged("EpisodeName");
            }
        }

        public int Resolution
        {
            get { return resolution; }
            set
            {
                if (resolution == value) return;
                resolution = value;
                NotifyPropertyChanged("Resolution");
                NotifyPropertyChanged("ResolutionName");
            }
        }

        public string ResolutionName
        {
            get
            {
                return string.Format("{0}p", resolution);
            }
        }

        public string DownloadLink
        {
            get { return downloadLink; }
            set
            {
                if (downloadLink == value) return;
                downloadLink = value;
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