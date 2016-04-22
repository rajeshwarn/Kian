using Kian.Core;
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
            PluginContent.Content = XamlReader.Load(new FileStream(Path.Combine("Plugins", _name + ".xaml"), FileMode.Open));
        }
    }
}