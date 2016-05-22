using Kian.Core;
using Kian.Core.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KissAnime.Objects
{
    internal class AnimeDownload : Download, INotifyPropertyChanged
    {
        private string downloadLink;
        private int downloadProgress;
        private Episode episode;
        private string fileName;
        private string fileSize;
        private int resolution;
        private string sizeRequestReferrer;
        private DownloadStatus status = DownloadStatus.Queue;
        private string title;

        public event DownloadStatusChangedEventHandler DownloadStatusChanged = delegate { };

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

        public int DownloadProgress
        {
            get { return downloadProgress; }
        }

        public Episode Episode
        {
            get
            {
                return episode;
            }
            set
            {
                if (episode == value) return;
                episode = value;
                NotifyPropertyChanged("Episode");
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
                return string.Format("{0}p", (resolution == 0) ? "???" : resolution.ToString());
            }
        }

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

        public DownloadStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status == value) return;
                DownloadStatus oldStatus = status;
                status = value;
                DownloadStatusChanged(this, oldStatus, value);
                NotifyPropertyChanged("Status");
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (title == value) return;
                title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public bool Start()
        {
            Trace.TraceInformation("Started downloading {0} ({1})", FileName, Title);
            Status = DownloadStatus.Downloading;
            ////throw new NotImplementedException();
            return true;
        }

        public bool Stop()
        {
            Trace.TraceInformation("Stopped downloading {0} ({1})", FileName, Title);
            Status = DownloadStatus.Cancelled;
            ////throw new NotImplementedException();
            return true;
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