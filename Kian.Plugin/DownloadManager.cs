using Kian.Core.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kian.Core
{
    public static class DownloadManager
    {
        private static ObservableCollection<Download> download;

        public static ObservableCollection<Download> Download
        {
            get
            {
                return download;
            }

            set
            {
                download = value;
            }
        }
    }
}