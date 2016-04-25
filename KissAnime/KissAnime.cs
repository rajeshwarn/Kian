using Kian.Core;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;

namespace KissAnime
{
    [Export(typeof(IPlugin))]
    public class KissAnime : IPlugin
    {
        private MetroWindow _mainWindow;
        private ContentControl _pluginContent;
        private string _name = "KissAnime";

        public MetroWindow MainWindow
        {
            get
            {
                return _mainWindow;
            }

            set
            {
                _mainWindow = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public ContentControl PluginContent
        {
            get
            {
                return _pluginContent;
            }

            set
            {
                _pluginContent = value;
            }
        }

        public void OnSearch(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public void OnStart()
        {
            PluginContent.Content = XamlReader.Load(new FileStream(Path.Combine("Plugins", _name + ".xaml"), FileMode.Open));
        }
    }
}