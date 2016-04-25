using Kian.Core.MyAnimeList.API.Anime;
using Kian.Core.MyAnimeList.Objects.API.Anime;
using Kian.Core.Objects.Anime;
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

        private NetworkCredential _malLogin = new NetworkCredential("wildbook", "wille001");
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

        private void LoadData(string searchString)
        {
            /*
            Anime entry = (Anime)expander.DataContext;

            if (_malLogin == null)
            {
                LoginDialogData login = await this.ShowLoginAsync("You need to log in to MyAnimeList.net to continue.", "Enter your MAL login:");

                if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                {
                    expander.IsExpanded = false;
                    return;
                }

                _malLogin = new NetworkCredential(login.Username, login.Password);
            }

            if (entry.MalData == null)
            {
                anime searchResults = Search.GetResults(entry.Title, _malLogin);

                if (searchResults != null && searchResults.Items.Length > 0)
                {
                    entry.MalData = searchResults.Items[0];
                }
                else
                {
                    entry.MalData = null;
                }
            }
            */
        }

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
    }
}