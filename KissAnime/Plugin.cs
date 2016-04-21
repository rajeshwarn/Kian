using Cloudflare_Evader;
using CsQuery;
using Kian.Core;
using Kian.Core.Objects.Anime;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.IO;

namespace KissAnime
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        private string _name = "KissAnime";
        private ContentControl _pluginContent;

        public string Name
        {
            get
            {
                throw new NotImplementedException();
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
            PluginContent.Content = XamlReader.Load(new FileStream(Path.Combine("Plugins", _name + ".xaml"), FileMode.Open)); ;
        }
    }
}