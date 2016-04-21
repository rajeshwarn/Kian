using System.Threading.Tasks;

namespace Kian.Core
{
    public interface IPlugin
    {
        string Name { get; }

        Objects.Anime.DownloadSource GetAnime(string searchString);
    }
}