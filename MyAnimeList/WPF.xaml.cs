﻿using Kian.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MyAnimeList.API.Anime;
using MyAnimeList.Objects.API.Anime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        private NetworkCredential _login;
        private bool _authenticated = false;
        private bool _saveLogin = false;
        private List<animeEntry> _searchItems = new List<animeEntry>();
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
                {
                    Display = Visibility.Visible;
                    if (_saveLogin == true)
                    {
                        Properties.Settings.Default.Username = _login.UserName;
                        Properties.Settings.Default.Password = Crypt.EncryptString(_login.SecurePassword);
                        Properties.Settings.Default.Save();
                    }
                }
                else
                {
                    Display = Visibility.Collapsed;

                    Properties.Settings.Default.Username = null;
                    Properties.Settings.Default.Password = null;
                    Properties.Settings.Default.Save();
                }
            }
        }

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

            bw.RunWorkerAsync();
        }

        private void Anime_Expanded(object sender, RoutedEventArgs e)
        {
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
        }

        private async void UserControl_Loaded(object sender, EventArgs e)
        {
            _login = new NetworkCredential(Properties.Settings.Default.Username, Crypt.DecryptString(Properties.Settings.Default.Password));
            if (API.Authentication.VerifyCredentials(_login))
                Authenticated = true;
            else
            {
                Authenticated = false;
                await Authenticate();
            }
        }

        private async Task<bool> Authenticate()
        {
            MetroDialogSettings approveLoginDialogSettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                FirstAuxiliaryButtonText = "Yes, but don't save my password",
                DefaultButtonFocus = MessageDialogResult.Affirmative,
                AnimateHide = true,
                AnimateShow = true,
            };

            MetroWindow mainWindow = ((MetroWindow)Window.GetWindow(this));
            var approveLogin = await mainWindow.ShowMessageAsync("Do you want to log in with your MyAnimeList account?", "If you don't log in, Kian will not be able to read info about shows from MAL", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, approveLoginDialogSettings);

            if (approveLogin == MessageDialogResult.Negative)
            {
                Authenticated = false;
                return false;
            }
            else if (approveLogin == MessageDialogResult.Affirmative)
                _saveLogin = true;
            else if (approveLogin == MessageDialogResult.FirstAuxiliary)
                _saveLogin = false;

            LoginDialogData login = await mainWindow.ShowLoginAsync("Enter your MAL login:", "By logging in, your password will be encrypted using DPAPI and saved until next time.");
            NetworkCredential loginCredentials = new NetworkCredential(login.Username, login.Password);

            if (!string.IsNullOrEmpty(loginCredentials.UserName) &&
                !string.IsNullOrEmpty(loginCredentials.Password) &&
                API.Authentication.VerifyCredentials(loginCredentials))
            {
                _login = loginCredentials;
                Authenticated = true;
                return true;
            }
            else
            {
                MetroDialogSettings retryDialogSettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    AnimateHide = true,
                    AnimateShow = true,
                    DefaultButtonFocus = MessageDialogResult.Affirmative,
                };

                MessageDialogResult retry = await mainWindow.ShowMessageAsync("Your MyAnimeList login could not be verified.", "Do you want to try re-entering it?", MessageDialogStyle.AffirmativeAndNegative, retryDialogSettings);
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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Authenticated = false;
        }
    }
}