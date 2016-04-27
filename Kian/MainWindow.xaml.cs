using Kian.Core;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Kian
{
    public partial class MainWindow : MetroWindow
    {
        private IEnumerable<IPlugin> plugins;
        private PluginManager<IPlugin> loader = new PluginManager<IPlugin>("Plugins");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadPlugins()
        {
            PluginManager<IPlugin> loader = new PluginManager<IPlugin>("Plugins");
            plugins = loader.Plugins;
        }

        private void SearchCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            string searchString = ((TextBox)sender).Text;
            foreach (IPlugin plugin in plugins)
                plugin.OnSearch(searchString);
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
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
    }
}