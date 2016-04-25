using Kian.Core.MyAnimeList.API.Anime;
using Kian.Core.MyAnimeList.Objects.API.Anime;
using Kian.Core.Objects.Anime;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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
    public partial class WPF : UserControl
    {
        public WPF()
        {
            InitializeComponent();
        }

        private NetworkCredential _malLogin;// = new NetworkCredential("username", "password"); // For debugging only, remove real data when committing. (FILTER)
        private List<animeEntry> _searchItems;

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
            }
        }
        public string SearchItemsCountString { get { return SearchItems.Count + ((SearchItems.Count == 1) ? " Result" : "Results"); } }

        public void OnSearch(string searchString)
        {
            SearchItems.Clear();
            searchItems.Items.Refresh();

            anime searchResults = Search.GetResults(searchString, _malLogin);
            if (searchResults != null && searchResults.Items != null)
            {
                SearchItems = searchResults.Items.ToList();
            }

            searchItems.Items.Refresh();
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

        private async void UserControl_Initialized(object sender, EventArgs e)
        {
            if (_malLogin == null)
            {
                LoginDialogData login = await((MetroWindow)Window.GetWindow(this)).ShowLoginAsync("You need to log in to MyAnimeList.net to use it.", "Enter your MAL login:");
                _malLogin = new NetworkCredential(login.Username, login.Password);
            }
        }
}