using CsQuery;
using Kian.Core.Objects;
using System;
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
        /// The download episodes.
        /// </summary>
        private ObservableCollection<Episode> episodes = new ObservableCollection<Episode>();

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
        /// Gets or sets the download episodes.
        /// </summary>
        /// <value>
        /// The download episodes.
        /// </value>
        public ObservableCollection<Episode> Episodes
        {
            get { return episodes; }
            set
            {
                if (episodes == value) return;
                episodes = value;
                NotifyPropertyChanged("Episodes");
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
        public ObservableCollection<Episode> GetEpisodes()
        {
            // Collection of Episodes to separate downloads based on video resolution
            ObservableCollection<Episode> episodes = new ObservableCollection<Episode>();

            // Using a cloudflare-authorized webclient from API
            using (WebClient localClient = API.GetWebClient())
            {
                // For every episode found
                foreach (IDomObject episodeDomObject in EpisodesCq[".listing a"].ToList())
                {
                    // Get its name and strip "\n" and 39 spaces from the name of it. (For some reason, someone found it esier to add that at the beginning of each name instead of using CSS to format their webpage)
                    string episodeName = episodeDomObject.InnerText.Substring(41);

                    // Get its url to be able to load download info from it later
                    string episodeUrl = new Uri(API.BaseUri, episodeDomObject.Attributes["Href"]).AbsoluteUri;

                    episodes.Add(new Episode
                    {
                        EpisodeName = episodeName,
                        EpisodeUrl = episodeUrl
                    });
                }
            }

            // Return all episodes
            return episodes;
        }
    }
}