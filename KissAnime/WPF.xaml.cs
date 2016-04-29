using Kian.Core.Objects;
using KissAnime.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            foreach (Download dl in lv.SelectedItems)
            {
                Console.WriteLine("Downloading {0}: {1}", dl.EpisodeName, dl.DownloadLink);
            }
        }

        private void anime_Expanded(object sender, RoutedEventArgs e)
        {
            Expander exp = sender as Expander;
            Anime anime = exp.DataContext as Anime;

            if (anime.DownloadGroups.Count == 0)
            {
                BackgroundWorker bw = new BackgroundWorker();

                bw.DoWork += delegate
                {
                    anime.DownloadGroups = anime.GetEpisodes();
                };

                bw.RunWorkerAsync();
            }
        }
    }
}