using Kian.Core;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MyAnimeList
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        private string name = "MyAnimeList";
        private ContentControl pluginContent;
        private WPF wpf;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public ContentControl PluginContent
        {
            get
            {
                return pluginContent;
            }

            set
            {
                pluginContent = value;
            }
        }

        public void OnSearch(string searchString)
        {
            if (wpf.Authenticated)
                wpf.OnSearch(searchString);
        }

        public void OnStart()
        {
            PluginContent.Content = wpf = new WPF();
        }
    }
}