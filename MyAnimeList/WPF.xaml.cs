using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MyAnimeList.API.Anime;
using MyAnimeList.Objects.API.Anime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyAnimeList
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

        private NetworkCredential _malLogin;// = new NetworkCredential("username", "password"); // For debugging only, remove real data when committing. (FILTER)
        private List<animeEntry> _searchItems;
        private Visibility _display = Visibility.Collapsed;
        private string _searchItemsCountString = "Ready!";
        private bool _searching = false;
        private bool _enabled = true;
        private bool _authenticated = false;

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
            }
        }

        public List<animeEntry> SearchItems
        {
            get
            {
                if (_searchItems == null)
                {
                    _searchItems = new List<animeEntry>() {
                        new animeEntry
                        {
                            title       = "Sabagebu!",
                            synopsis    = "Synopsis",
                            score       = "Score"
                        }
                    };
                }

                return _searchItems;
            }
            set
            {
                _searchItems = value;
                NotifyPropertyChanged("SearchItems");
            }
        }

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

            bw.DoWork += delegate
            {
                Searching = true;

                SearchItems.Clear();

                anime searchResults = Search.GetResults(searchString, _malLogin);

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

            bw.RunWorkerAsync();
        }

        private void Anime_Expanded(object sender, RoutedEventArgs e)
        {
            Expander expander = (Expander)sender;

            animeEntry entry = (animeEntry)expander.DataContext;

            if (entry == null)
            {
                anime searchResults = Search.GetResults(entry.title, _malLogin);

                if (searchResults != null && searchResults.Items.Length > 0)
                {
                    entry = searchResults.Items[0];
                }
                else
                {
                    entry = null;
                }
            }
        }

        private async void UserControl_Loaded(object sender, EventArgs e)
        {
            await Authenticate();
        }

        private async Task<bool> Authenticate()
        {
            MetroWindow mainWindow = ((MetroWindow)Window.GetWindow(this));
            LoginDialogData login = await mainWindow.ShowLoginAsync("You need to log in to MyAnimeList.net to use it.", "Enter your MAL login:");
            NetworkCredential loginCredentials = new NetworkCredential(login.Username, login.Password);

            if (!string.IsNullOrEmpty(loginCredentials.UserName) &&
                !string.IsNullOrEmpty(loginCredentials.Password) &&
                API.Authentication.VerifyCredentials(loginCredentials))
            {
                Authenticated = true;
                _malLogin = loginCredentials;
                Display = Visibility.Visible;
                return true;
            }
            else
            {
                MetroDialogSettings metroDialogSettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    AnimateHide = true,
                    AnimateShow = true,
                    DefaultButtonFocus = MessageDialogResult.Affirmative,
                };

                MessageDialogResult retry = await mainWindow.ShowMessageAsync("Your MyAnimeList login could not be verified.", "Do you want to try re-entering it?", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
                if (retry == MessageDialogResult.Affirmative)
                {
                    return await Authenticate();
                }
                else
                {
                    Authenticated = false;
                    return false;
                }
            }
        }
    }
}