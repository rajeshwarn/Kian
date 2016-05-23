using Kian.Core;
using Kian.Core.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace KissAnime.Objects
{
    internal class AnimeDownload : Download, INotifyPropertyChanged, IDisposable
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

        public event PropertyChangedEventHandler PropertyChanged;

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
                DownloadStatusChanged(this, new DownloadStatusChangedEventData(oldStatus, value));
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

        private WebClient downloadingClient;

        private AnimeDownload GetDownload(Episode ep)
        {
            List<AnimeDownload> dlList = API.GetDownloads(ep.EpisodeUrl, ep.EpisodeName);

            AnimeDownload maxDl = new AnimeDownload();
            int maxResolution = 0;

            foreach (AnimeDownload dl in dlList)
            {
                int res = dl.Resolution;
                if (res > maxResolution)
                {
                    maxResolution = res;
                    maxDl = dl;
                }
            }

            return maxDl;
        }

        public bool Start()
        {
            AnimeDownload dl = GetDownload(Episode);

            DownloadLink = dl.DownloadLink;
            Resolution = dl.Resolution;

            // This is a really shitty solution, but so far it has worked as all files I have encountered so far has been mp4.
            // There is probably a way better solution to this, but I have time issues as it is :(
            // I tried to get the file extension by usual means, but sometimes the URL contained a '.' and no file extension,
            // which resulted in 25+ character long, invalid, file extensions.
            // TODO: Find the real extension through the first hex-bytes.
            string extension = ".mp4";
            

            FileName = DownloadManager.GenerateFilePath(this) + extension;

            downloadingClient = new WebClient();
            downloadingClient.Headers.Add("Referer", "http://www.animebam.net/");
            downloadingClient.DownloadProgressChanged += DownloadProgressChanged;
            downloadingClient.DownloadFileCompleted += DownloadFileCompleted;

            downloadingClient.DownloadFileAsync(new Uri(DownloadLink), Path.Combine(DownloadManager.DownloadFolder, FileName));

            Status = DownloadStatus.Downloading;
            Trace.TraceInformation("Started downloading {0} ({1})", FileName, Title);
            return true;
        }

        public bool Stop()
        {
            if (downloadingClient != null)
            {
                downloadingClient.CancelAsync();
                downloadingClient.Dispose();
            }

            Status = DownloadStatus.Cancelled;
            Trace.TraceInformation("Stopped downloading {0} ({1})", FileName, Title);
            return true;
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Status = DownloadStatus.Cancelled;
                File.Delete(FileName);
            }
            else
            {
                Status = DownloadStatus.Successful;
            }

            downloadProgress = 100;
            NotifyPropertyChanged("DownloadProgress");
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (downloadProgress != e.ProgressPercentage)
            {
                downloadProgress = e.ProgressPercentage;
                NotifyPropertyChanged("DownloadProgress");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Dispose()
        {
            if (downloadingClient != null)
            {
                downloadingClient.Dispose();
            }
        }
    }
}