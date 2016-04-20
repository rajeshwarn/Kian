using Kian.Core;
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudFlareUtilities;
using System.Net.Http;
using Kian.Objects.Anime;
using CsQuery;

namespace KissAnime
{
    /*
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        HttpClient client = new HttpClient(new ClearanceHandler());

        private string pluginName = "KissAnime";
        private string baseUrl = "https://kissanime.to/";
        private string searchUrl = "https://kissanime.to/Search/Anime";

        public string Name { get { return pluginName; } }

        public async Task<DownloadSource> GetAnime(string searchString)
        {
            DownloadSource downloadSource = new DownloadSource { Name = pluginName };
            FormUrlEncodedContent postData = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("keyword", searchString) });
            HttpResponseMessage search = await client.PostAsync(searchUrl, postData);

            while (!search.IsSuccessStatusCode)
            {
                search = await client.PostAsync(searchUrl, postData);
            }
                CQ searchCq = new CQ(await search.Content.ReadAsStringAsync());

                foreach (IDomObject searchDomObject in searchCq[".listing a"].ToList())
                {
                    string animeName = searchDomObject.InnerText;
                    HttpResponseMessage episodes = await client.GetAsync(baseUrl + searchDomObject.Attributes["Href"]);
                    if (episodes.IsSuccessStatusCode)
                    {
                        CQ episodesQc = new CQ(await episodes.Content.ReadAsStringAsync());

                        foreach (IDomObject episodeDomObject in episodesQc[".listing a"].ToList())
                        {
                            string episodeName = episodeDomObject.InnerText;
                            await GetVideo(baseUrl + episodeDomObject.Attributes["Href"]);
                        }
                    }
                }

                return downloadSource;

            return null;
        }

        private async Task<string> GetVideo(string url)
        {
            HttpResponseMessage episode = await client.GetAsync(url);

            if (episode.IsSuccessStatusCode)
            {
                CQ episodeQc = new CQ(await episode.Content.ReadAsStringAsync());
                var target = episodeQc["#my_video_1_html5_api"][0].Attributes["Href"];
                return episodeQc["div:contains(\"CLICK HERE\")"].Attr("Href");
            }
            return "";
        }
    }
*/
}