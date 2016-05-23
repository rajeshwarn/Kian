using Kian.Core.Objects;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Kian.Core
{
    public static class DownloadManager
    {
        private static ObservableCollectionEx<Download> downloads = new ObservableCollectionEx<Download>();

        private static string filePathTemplate = "{Title}";

        /*
            --- Valid arguments so far: ---
                {Title}
        */

        /// <summary>
        /// Gets or sets the download folder.
        /// </summary>
        /// <value>
        /// The download folder.
        /// </value>
        public static string DownloadFolder
        {
            get
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.DownloadFolder))
                    return Directory.GetCurrentDirectory();
                return Properties.Settings.Default.DownloadFolder;
            }

            set
            {
                if (Properties.Settings.Default.DownloadFolder == value) return;
                Properties.Settings.Default.DownloadFolder = value;
                Properties.Settings.Default.Save();
            }
        }
        /// <summary>
        /// Gets a list of downloads.
        /// </summary>
        /// <value>
        /// List of downloads.
        /// </value>
        public static ObservableCollectionEx<Download> Downloads
        {
            get
            {
                return downloads;
            }
        }

        public static int MaxParallelDownloads { get; set; } = 3;

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

        public static string GenerateFilePath(Download dl)
        {
            return HelperFunctions.GetValidFilePath(filePathTemplate.Replace("{Title}", dl.Title));
        }

        public static List<Download> GetDownloadsWithStatus(DownloadStatus status)
        {
            return Downloads.Where(x => x.Status == status).ToList();
        }

        public static void RemoveDownload(Download dl)
        {
            dl.Stop();
            Downloads.Remove(dl);
        }

        public static void StopDownload(Download dl)
        {
            dl.Stop();
        }

        private static void Dl_DownloadStatusChanged(object sender, DownloadStatusChangedEventData data)
        {
            DownloadStatus oldStatus = data.oldStatus;
            DownloadStatus newStatus = data.newStatus;

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
    }
}