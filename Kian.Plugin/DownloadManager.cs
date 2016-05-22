using Kian.Core;
using Kian.Core.Objects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Kian.Core
{
    public static class DownloadManager
    {
        public static int MaxParallelDownloads { get; set; } = 3;

        private static ObservableCollectionEx<Download> downloads = new ObservableCollectionEx<Download>();

        public static ObservableCollectionEx<Download> Downloads
        {
            get
            {
                return downloads;
            }
        }

        public static void AddDownload(Download dl)
        {
            dl.DownloadStatusChanged += Dl_DownloadStatusChanged;
            Downloads.Add(dl);

            if (Downloads.Where(x => x.Status == DownloadStatus.Downloading).Count() < MaxParallelDownloads)
            {
                dl.Start();
                Trace.TraceInformation("Starting download '{0}'.", dl.Title);
            }
            else
            {
                Trace.TraceInformation("Putting '{0}' into queue.", dl.Title);
            }
        }

        private static void Dl_DownloadStatusChanged(object sender, DownloadStatus oldStatus, DownloadStatus newStatus)
        {
            switch (oldStatus)
            {
                case DownloadStatus.Downloading:
                    break;
            }

            switch (newStatus)
            {
                case DownloadStatus.Downloading:
                    break;
            }

            if (GetDownloadsWithStatus(DownloadStatus.Downloading).Count < MaxParallelDownloads)
            {
                // Starting a download results in "Dl_DownloadStatusChanged" (this method) getting called, so we only need to do this once to reach MaxParallelDownloads.
                List<Download> queue = GetDownloadsWithStatus(DownloadStatus.Queue);
                queue.Remove((Download)sender);
                if (queue.Count > 0)
                    queue.First().Start();
            }

            // DO: Actually download things
        }

        public static void StopDownload(Download dl)
        {
            dl.Stop();
        }

        public static void RemoveDownload(Download dl)
        {
            dl.Stop();
            Downloads.Remove(dl);
        }

        public static List<Download> GetDownloadsWithStatus(DownloadStatus status)
        {
            return Downloads.Where(x => x.Status == status).ToList();
        }
    }
}