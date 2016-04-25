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
        private WPF wpf;

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

        public void OnSearch(string searchString)
        {
            wpf.OnSearch(searchString);
        }

        public void OnStart()
        {
            PluginContent.Content = wpf = new WPF();
        }
    }
}