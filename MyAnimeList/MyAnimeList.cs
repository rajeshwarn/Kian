using Kian.Core;
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
    public class MyAnimeList : IPlugin
    {
        private string _name = "MyAnimeList";
        private ContentControl _pluginContent;

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