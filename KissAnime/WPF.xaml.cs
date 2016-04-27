using Kian.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
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

        //private List<animeEntry> _searchItems = new List<animeEntry>();
        private Visibility _display = Visibility.Collapsed;

        private string _searchItemsCountString = "Ready!";
        private bool _searching = false;
        private bool _enabled = false;

        public bool Searching
        {
            get
            {
                return _searching;
            }
            set
            {
                _searching = value;

                if (_searching)
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
                return _enabled;
            }
            set
            {
                _enabled = value;
                NotifyPropertyChanged("Enabled");
            }
        }

        public bool Authenticated
        {
            get
            {
                return _authenticated;
            }
            set
            {
                _authenticated = value;
                NotifyPropertyChanged("Authenticated");

                if (value == true)
                    Display = Visibility.Visible;
                else
                    Display = Visibility.Collapsed;
            }
        }

        /*
        public List<animeEntry> SearchItems
        {
            get
            {
                return _searchItems;
            }
            set
            {
                _searchItems = value;
                NotifyPropertyChanged("SearchItems");
            }
        }
        */

        public Visibility Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                NotifyPropertyChanged("Display");
            }
        }

        public string SearchItemsCountString
        {
            get
            {
                return _searchItemsCountString;
            }
            set
            {
                _searchItemsCountString = value;
                NotifyPropertyChanged("SearchItemsCountString");
            }
        }

        public void OnSearch(string searchString)
        {
            BackgroundWorker bw = new BackgroundWorker();

            /*
            bw.DoWork += delegate
            {
                Searching = true;

                SearchItems.Clear();

                anime searchResults = Search.GetResults(searchString, _login);

                if (searchResults != null && searchResults.Items != null)
                {
                    SearchItems = searchResults.Items.ToList();
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
            */

            bw.RunWorkerAsync();
        }

        private void Anime_Expanded(object sender, RoutedEventArgs e)
        {
            /*
            Expander expander = (Expander)sender;

            animeEntry entry = (animeEntry)expander.DataContext;

            if (entry == null)
            {
                anime searchResults = Search.GetResults(entry.title, _login);

                if (searchResults != null && searchResults.Items.Length > 0)
                {
                    entry = searchResults.Items[0];
                }
                else
                {
                    entry = null;
                }
            }
            */
        }

        private void UserControl_Loaded(object sender, EventArgs e)
        {
        }
    }
}