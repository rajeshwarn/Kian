using System.Threading.Tasks;

namespace Kian.Core
{
    public interface IPlugin
    {
        string Name { get; }
        Task<Objects.Anime.DownloadSource> GetAnime(string searchString);
    }
}