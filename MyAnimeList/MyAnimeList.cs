using Kian.Core;
using Kian.Core.MyAnimeList.API.Anime;
using Kian.Core.MyAnimeList.Objects.API.Anime;
using Kian.Core.Objects.Anime;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyAnimeList
{
    class MyAnimeList
    {
        private NetworkCredential _malLogin;
        private List<Anime> _searchItems;

        public List<Anime> SearchItems
        {
            get
            {
                if (_searchItems == null)
                {
                    _searchItems = generateFakeSearch();
                }

                return _searchItems;
            }
        }
        private List<Anime> generateFakeSearch()
        {
            return new List<Anime>() {
                new Anime
                {
                Title = "Sabagebu!",
                DownloadSources = new List<DownloadSource> {
                    new DownloadSource
                    {
                        Name = "KissAnime.fake",
                        DownloadGroups = new List<DownloadGroup>
                        {
                            new DownloadGroup()
                            {
                                GroupName = "1080p",
                                Downloads = new List<Download> {
                                    new Download {
                                        FileName = "Noragami-1-1080p.mp4",
                                        DownloadLink = "http://cdn.oose.io/W4kU3x570Dj024S8llA5N677DgXHFr4b2x5E6wv4XX8grOaLltYJIa70Sui53xUj/video.mp4",
                                        Resolution = "Not Yet Implemented",
                                        EpisodeName = "Noragami - Episode 1"
                                    },

                                    new Download {
                                        FileName = "Noragami-1-1080p.mp4",
                                        DownloadLink = "http://cdn.oose.io/W4kU3x570Dj024S8llA5N677DgXHFr4b2x5E6wv4XX8grOaLltYJIa70Sui53xUj/video.mp4",
                                        Resolution = "Not Yet Implemented",
                                        EpisodeName = "Noragami - Episode 2"
                                    },

                                    new Download {
                                        FileName = "Noragami-1-1080p.mp4",
                                        DownloadLink = "http://cdn.oose.io/W4kU3x570Dj024S8llA5N677DgXHFr4b2x5E6wv4XX8grOaLltYJIa70Sui53xUj/video.mp4",
                                        Resolution = "Not Yet Implemented",
                                        EpisodeName = "Noragami - Episode 3"
                                    }
                                }
                            },

                            new DownloadGroup()
                            {
                                GroupName = "720p",
                                Downloads = new List<Download> {
                                    new Download {
                                        FileName = "Noragami-1-1080p.mp4",
                                        DownloadLink = "http://cdn.oose.io/W4kU3x570Dj024S8llA5N677DgXHFr4b2x5E6wv4XX8grOaLltYJIa70Sui53xUj/video.mp4",
                                        Resolution = "Not Yet Implemented",
                                        EpisodeName = "Noragami - Episode 1"
                                    },
                                    new Download {
                                        FileName = "Noragami-1-1080p.mp4",
                                        DownloadLink = "http://cdn.oose.io/W4kU3x570Dj024S8llA5N677DgXHFr4b2x5E6wv4XX8grOaLltYJIa70Sui53xUj/video.mp4",
                                        Resolution = "Not Yet Implemented",
                                        EpisodeName = "Noragami - Episode 2"
                                    },
                                    new Download {
                                        FileName = "Noragami-1-1080p.mp4",
                                        DownloadLink = "http://cdn.oose.io/W4kU3x570Dj024S8llA5N677DgXHFr4b2x5E6wv4XX8grOaLltYJIa70Sui53xUj/video.mp4",
                                        Resolution = "Not Yet Implemented",
                                        EpisodeName = "Noragami - Episode 3"
                                    }
                                }
                            }
                        }
                    }
                },
                    Tags = new List<string> { "360p", "1080p" }
                }
            };
        }

        private Plugin plugin;

        private ItemsControl gui_searchItems;

        public MyAnimeList(Plugin newPlugin)
        {
            plugin = newPlugin;

            gui_searchItems = (ItemsControl)plugin.PluginContent.FindName("searchItems");
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
            _searchItems.Clear();
            gui_searchItems.Items.Refresh();

            anime searchResults = Search.GetResults(searchString, _malLogin);
            if (searchResults != null && searchResults.Items != null)
            {
                foreach (animeEntry entry in searchResults.Items)
                {
                    Anime searchEntry = new Anime();

                    searchEntry.Title = entry.title;
                    searchEntry.MalData = entry;

                    foreach (IPlugin plugin in plugins)
                    {
                        DownloadSource source = plugin.GetAnime(entry.title);
                        if (source != null)
                        {
                            if (searchEntry.DownloadSources == null)
                                searchEntry.DownloadSources = new List<DownloadSource>();

                            searchEntry.DownloadSources.Add(source);
                        }
                    }

                    _searchItems.Add(searchEntry);
                }
            }

            searchItems.Items.Refresh();
        }
    }
}