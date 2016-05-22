using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kian.Core.Objects
{
    public delegate void DownloadStatusChangedEventHandler(object sender, DownloadStatus oldStatus, DownloadStatus newStatus);
}