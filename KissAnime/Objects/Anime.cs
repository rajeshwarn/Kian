using CsQuery;
using Kian.Core.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace KissAnime.Objects
{
    /// <summary>
    /// Object containing data about a series.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class Anime : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The title.
        /// </summary>
        private string title;

        /// <summary>
        /// The download groups.
        /// </summary>
        private ObservableCollection<DownloadGroup> downloadGroups = new ObservableCollection<DownloadGroup>();

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
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

        /// <summary>
        /// Gets or sets the download groups.
        /// </summary>
        /// <value>
        /// The download groups.
        /// </value>
        public ObservableCollection<DownloadGroup> DownloadGroups
        {
            get { return downloadGroups; }
            set
            {
                if (downloadGroups == value) return;
                downloadGroups = value;
                NotifyPropertyChanged("DownloadGroups");
            }
        }

        /// <summary>
        /// Qc object containing the source of the episodes page.
        /// Used to keep the episodes page and not load it util needed to prevent looong search time.
        /// </summary>
        public CQ EpisodesCq;

        /// <summary>
        /// Gets the current series' episodes.
        /// </summary>
        /// <returns>All episodes found</returns>
        public ObservableCollection<DownloadGroup> GetEpisodes()
        {
            // Collection of downloadgroups to separate downloads based on video resolution
            ObservableCollection<DownloadGroup> downloadGroups = new ObservableCollection<DownloadGroup>();

            // Dictionary <resolution, group> to avoid creating multiple downloadGroups for each resolution
            Dictionary<int, DownloadGroup> dlg = new Dictionary<int, DownloadGroup>();

            // DownloadGroup containing the highest-res episodes
            // Create and add this here to keep it on top of the other groups
            DownloadGroup highest = new DownloadGroup { GroupName = "Highest" };
            downloadGroups.Add(highest);

            // DownloadGroup containing the lowest-res episodes
            // Create here to be able to add downloads to it, but don't add it to the list of groups as that would place it on top of the other groups
            DownloadGroup lowest = new DownloadGroup { GroupName = "Lowest" };

            // Using a cloudflare-authorized webclient from API
            using (WebClient localClient = API.GetWebClient())
            {
                // For every episode found
                foreach (IDomObject episodeDomObject in EpisodesCq[".listing a"].ToList())
                {
                    // Get its name and strip "\n" and 39 spaces from the name of it. (For some reason, someone found it esier to add that at the beginning of each name instead of using CSS to format their webpage)
                    string episodeName = episodeDomObject.InnerText.Substring(41);

                    // Keep the min-res and max-res download stored here to be able to add them to their respective collections later
                    Download maxResEpisode = null;
                    Download minResEpisode = null;

                    // For each download for an episode
                    foreach (Download dl in API.GetDownloads(localClient, new Uri(API.BaseUri, episodeDomObject.Attributes["Href"]).AbsoluteUri, episodeName))
                    {
                        // If it's resolution doesn't already have a group, create one for it
                        if (!dlg.ContainsKey(dl.Resolution))
                            dlg.Add(dl.Resolution, new DownloadGroup { GroupName = dl.ResolutionName });

                        // Add the download to the group containing episodes with the same resolution
                        dlg[dl.Resolution].Downloads.Add(dl);

                        // Find the highest/lowest resolution download and keep them
                        if (maxResEpisode == null || dl.Resolution > maxResEpisode.Resolution)
                            maxResEpisode = dl;
                        else if (minResEpisode == null || dl.Resolution < minResEpisode.Resolution)
                            minResEpisode = dl;
                    }

                    // Add the highest/lowest resolution download to their respective list
                    highest.Downloads.Add(maxResEpisode);
                    lowest.Downloads.Add(minResEpisode);
                }

                // Add all groups to the list of groups
                foreach (KeyValuePair<int, DownloadGroup> kvp in dlg)
                    downloadGroups.Add(kvp.Value);

                // Add this group here to keep it below the other groups
                downloadGroups.Add(lowest);
            }

            // Return all groups
            return downloadGroups;
        }
    }
}