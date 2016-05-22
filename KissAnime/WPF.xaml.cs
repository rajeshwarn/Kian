using Kian.Core;
using Kian.Core.Objects;
using KissAnime.Objects;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace KissAnime
{
    /// <summary>
    /// Interaction logic for WPF.xaml
    /// </summary>
    public partial class WPF : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public WPF()
        {
            InitializeComponent();
        }

        private List<Anime> searchItems = new List<Anime>();
        private Visibility display = Visibility.Visible;

        private string searchItemsCountString = "Ready!";
        private bool searching = false;
        private bool enabled = false;

        public bool Searching
        {
            get
            {
                return searching;
            }
            set
            {
                searching = value;

                if (searching)
                {
                    Enabled = false;
                    Display = Visibility.Visible;
                }
                else
                    Enabled = true;

                SearchItemsCountString = "Searching";

                NotifyPropertyChanged("Searching");
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                NotifyPropertyChanged("Enabled");
            }
        }

        public List<Anime> SearchItems
        {
            get
            {
                return searchItems;
            }
            set
            {
                searchItems = value;
                NotifyPropertyChanged("SearchItems");
            }
        }

        public Visibility Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
                NotifyPropertyChanged("Display");
            }
        }

        public string SearchItemsCountString
        {
            get
            {
                return searchItemsCountString;
            }
            set
            {
                searchItemsCountString = value;
                NotifyPropertyChanged("SearchItemsCountString");
            }
        }

        public void OnSearch(string searchString)
        {
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += delegate
            {
                Searching = true;

                SearchItems.Clear();

                List<Anime> searchResults = API.Search(searchString);

                if (searchResults != null && searchResults != null)
                {
                    SearchItems = searchResults;
                    Display = Visibility.Visible;
                }
                else
                    Display = Visibility.Collapsed;

                Searching = false;

                if (SearchItems.Count == 1)
                    SearchItemsCountString = SearchItems.Count + " Result";
                else
                    SearchItemsCountString = SearchItems.Count + " Results";
            };

            bw.RunWorkerAsync();
        }

        private void downloadEpisode_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ListView lv = ((ContextMenu)mnu.Parent).PlacementTarget as ListView;

            List<Episode> episodes = lv.SelectedItems.OfType<Episode>().ToList();
            episodes.Reverse();

            foreach (Episode episode in episodes)
            {
                AnimeDownload dl = new AnimeDownload();
                dl.Title = episode.EpisodeName;
                dl.Episode = episode;

                DownloadManager.AddDownload(dl);
            }
        }

        private void anime_Expanded(object sender, RoutedEventArgs e)
        {
            Expander exp = sender as Expander;
            Anime anime = exp.DataContext as Anime;

            if (anime.Episodes.Count == 0)
            {
                BackgroundWorker bw = new BackgroundWorker();

                bw.DoWork += delegate
                {
                    anime.Episodes = anime.GetEpisodes();
                };

                bw.RunWorkerAsync();
            }
        }
    }
}