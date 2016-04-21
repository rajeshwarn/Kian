using Kian.Core.MyAnimeList.Objects.API.Anime;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Kian.Core.MyAnimeList.API.Anime
{
    public class Search
    {
        public static anime GetResults(string searchString, NetworkCredential loginCredenitals)
        {
            anime anime = new anime { };

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = loginCredenitals;
                    StringReader reader = new StringReader(client.DownloadString(string.Format("http://myanimelist.net/api/anime/search.xml?q={0}", searchString)));
                    anime = (anime)new XmlSerializer(typeof(anime)).Deserialize(reader);
                    reader.Close();
                }

                foreach (animeEntry entry in anime.Items)
                {
                    entry.synopsis = WebUtility.HtmlDecode(Regex.Replace(entry.synopsis, @"<[^>]+>|&nbsp;|\[i\]|\[/i\]", "").Trim()); // Remove HTML things.
                }

                System.Threading.Thread.Sleep(500);
                return anime;
            }
            catch
            {
                return null;
            }
        }
    }
}