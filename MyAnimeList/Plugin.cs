using Kian.Core;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MyAnimeList
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        private string _name = "MyAnimeList";
        private ContentControl _pluginContent;
        private MetroWindow _mainWindow;
        private MyAnimeList myAnimeList;

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

        public void OnSearch(string searchString)
        {
            myAnimeList.OnSearch(searchString);
        }

        public void OnStart()
        {
            PluginContent.Content = XamlReader.Load(new FileStream(Path.Combine("Plugins", _name + ".xaml"), FileMode.Open));
            myAnimeList = new MyAnimeList(this);
        }
    }
}