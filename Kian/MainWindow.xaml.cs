using Kian.Core;
using Kian.Core.Objects;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Kian
{
    /// <summary>
    /// The main window.
    /// </summary>
    /// <seealso cref="MahApps.Metro.Controls.MetroWindow" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : MetroWindow
    {
        private PluginManager<IPlugin> loader = new PluginManager<IPlugin>("Plugins");
        private IEnumerable<IPlugin> plugins;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void cancelDownload_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ListView lv = ((ContextMenu)mnu.Parent).PlacementTarget as ListView;

            List<Download> downloads = lv.SelectedItems.OfType<Download>().ToList();

            foreach (Download dl in downloads)
            {
                if (dl.Status == DownloadStatus.Downloading ||
                    dl.Status == DownloadStatus.Queue)
                {
                    dl.Stop();
                }
            }
        }

        private void LoadPlugins()
        {
            PluginManager<IPlugin> loader = new PluginManager<IPlugin>("Plugins");
            plugins = loader.Plugins;
        }

        private void removeDownload_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ListView lv = ((ContextMenu)mnu.Parent).PlacementTarget as ListView;

            List<Download> downloads = lv.SelectedItems.OfType<Download>().ToList();

            foreach (Download dl in downloads)
            {
                if (dl.Status == DownloadStatus.Downloading ||
                    dl.Status == DownloadStatus.Queue)
                {
                    dl.Stop();
                }
                DownloadManager.RemoveDownload(dl);
            }
        }

        private void SearchCommandBindingExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            string searchString = ((TextBox)sender).Text;
            foreach (IPlugin plugin in plugins)
                try
                {
                    plugin.OnSearch(searchString);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Plugin '{0}' crashed on search. (Error: {1})", plugin.Name, ex.Message);
                }
        }

        private void windowClosed(object sender, EventArgs e)
        {
            foreach (Download dl in DownloadManager.Downloads)
            {
                dl.Stop();
            }
        }

        private void windowLoaded(object sender, RoutedEventArgs e)
        {
            LoadPlugins();

            foreach (IPlugin plugin in plugins)
            {
                ContentControl pluginContent = new ContentControl();
                plugin.PluginContent = pluginContent;

                pluginExpanderStackPanel.Children.Add(pluginContent);

                plugin.OnStart();
            }
        }

        public class EnumToObjectConverter : IValueConverter
        {
            public ResourceDictionary Items { get; set; }

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                string key = Enum.GetName(value.GetType(), value);
                return Items[key];
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException("This converter only works for one way binding");
            }
        }

        private void openDownload_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            ListView lv = ((ContextMenu)mnu.Parent).PlacementTarget as ListView;

            List<Download> downloads = lv.SelectedItems.OfType<Download>().ToList();

            foreach (Download dl in downloads)
            {
                if (!string.IsNullOrEmpty(dl.FileName))
                    Process.Start(dl.FileName);
            }
        }

        private void downloads_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            while (obj != null)
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    ListViewItem item = (ListViewItem)obj;
                    Download dl = (item.Content as Download);

                    if (!string.IsNullOrEmpty(dl.FileName))
                    {
                        object path = Path.Combine(DownloadManager.DownloadFolder, dl.FileName);

                        Process.Start(Path.Combine(DownloadManager.DownloadFolder, dl.FileName));
                    }

                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }
    }
}