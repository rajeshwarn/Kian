namespace Kian.Core.Objects
{
    public enum DownloadStatus
    {
        /// <summary>
        /// The download has been cancelled
        /// </summary>
        Cancelled,
        /// <summary>
        /// The download is in the queue
        /// </summary>
        Queue,
        /// <summary>
        /// The download being downloaded
        /// </summary>
        Downloading,
        /// <summary>
        /// The download was successful
        /// </summary>
        Successful,
        /// <summary>
        /// The download failed
        /// </summary>
        Failed
    }
}