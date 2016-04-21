﻿using Kian;
using Kian.Core;
using Kian.MyAnimeList.Objects.API.Anime;
using Kian.Objects.Anime;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kian
{
    public partial class MainWindow : MetroWindow
    {
        private IEnumerable<IPlugin> plugins;
        private NetworkCredential _malLogin;
        private List<Anime> _searchItems;
        private PluginManager<IPlugin> loader = new PluginManager<IPlugin>("Plugins");

        public MainWindow()
        {
            InitializeComponent();
        }

        public List<Anime> SearchItems
        {
            get
            {
                if (_searchItems == null)
                {
                    _searchItems = new List<Anime>();

                    _searchItems.Add(new Anime
                    {
                        Title = "Sabagebu!",
                        DownloadSources = new List<DownloadSource> {
                            new DownloadSource
                            {
                                Name = "AnimeRam.co",
                                Downloads = new List<Objects.Anime.Download> {
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
                        },
                        Tags = new List<string> { "360p", "1080p" }
                    });
                }

                return _searchItems;
            }
        }

        private async void LoadMalData(object sender, RoutedEventArgs e)
        {
            Expander expander = (Expander)sender;
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

            await Task.Factory
                      .StartNew(() =>
                      {
                          if (entry.MalData == null)
                          {
                              anime searchResults = MyAnimeList.API.Anime.Search.GetResults(entry.Title, _malLogin);

                              if (searchResults != null && searchResults.Items.Length > 0)
                              {
                                  entry.MalData = searchResults.Items[0];
                              }
                              else
                              {
                                  entry.MalData = null;
                              }
                          }
                      });
        }

        private void LoadPlugins()
        {
            PluginManager<IPlugin> loader = new PluginManager<IPlugin>("Plugins");
            plugins = loader.Plugins;
        }

        private void SearchCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            _searchItems.Clear();
            searchItems.Items.Refresh();

            string searchString = searchStringBox.Text;

            BackgroundWorker malBackgroundWorker = new BackgroundWorker();
            malBackgroundWorker.DoWork += delegate
            {
                anime searchResults = MyAnimeList.API.Anime.Search.GetResults(searchString, _malLogin);
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
            };

            malBackgroundWorker.RunWorkerCompleted += delegate
            {
                searchItems.Items.Refresh();
            };

            malBackgroundWorker.RunWorkerAsync();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPlugins();
        }
    }
}