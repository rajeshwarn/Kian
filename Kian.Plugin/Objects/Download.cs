using System;
using System.ComponentModel;
using System.Net;

namespace Kian.Core.Objects
{
    public interface Download : INotifyPropertyChanged
    {
        string FileName { get; set; }
        string Title { get; set; }
        int Resolution { get; }
        string DownloadLink { get; set; }
        string FileSize { get; }
        DownloadStatus Status { get; }
        int DownloadProgress { get; }

        event DownloadStatusChangedEventHandler DownloadStatusChanged;

        bool Start();

        bool Stop();
    }
}