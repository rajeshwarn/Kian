using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace Kian.Core
{
    public interface IPlugin
    {
        string Name { get; }
        ContentControl PluginContent { get; set; }
        MetroWindow MainWindow { get; set; }

        void OnStart();
        void OnSearch(string searchTerm);
    }
}