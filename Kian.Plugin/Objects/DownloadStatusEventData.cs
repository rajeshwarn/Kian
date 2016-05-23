using System;

namespace Kian.Core.Objects
{
    public class DownloadStatusChangedEventData : EventArgs
    {
        public DownloadStatus oldStatus;
        public DownloadStatus newStatus;

        public DownloadStatusChangedEventData(DownloadStatus oldStatus, DownloadStatus newStatus)
        {
            this.oldStatus = oldStatus;
            this.newStatus = newStatus;
        }
    }
}