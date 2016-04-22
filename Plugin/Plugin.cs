using Kian.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Plugin
{
    public class Plugin : IPlugin
    {
        private string _name = "Plugin";
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