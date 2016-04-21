using Cloudflare_Evader;
using CsQuery;
using Kian.Core;
using Kian.Objects.Anime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KissAnime
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        private string pluginName = "KissAnime";
        private Uri baseUri = new Uri("https://kissanime.to/");
        private string searchUrl = "https://kissanime.to/Search/Anime";

        private WebClient client = null;
        private WebHeaderCollection _defaultHeaders;

        private WebHeaderCollection defaultHeaders
        {
            get
            {
                return CloneHeaders(_defaultHeaders);
            }
        }

        public string Name { get { return pluginName; } }

        public DownloadSource GetAnime(string searchString)
        {
            while (client == null)
            {
                Console.WriteLine("Trying..");
                client = CloudflareEvader.CreateBypassedWebClient(baseUri.AbsoluteUri);
                _defaultHeaders = client.Headers;
            }

            DownloadSource downloadSource = new DownloadSource { Name = pluginName };

            client.Headers = defaultHeaders;
            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

            string search = client.UploadString(searchUrl, "keyword=" + searchString);

            if (!string.IsNullOrEmpty(search))
            {
                CQ searchCq = new CQ(search);

                foreach (IDomObject searchDomObject in searchCq[".listing a"].ToList())
                {
                    string animeName = searchDomObject.InnerText;

                    client.Headers = defaultHeaders;
                    string episodes = client.DownloadString("https://kissanime.to/Anime/Noragami-Dub"); //new Uri(baseUri, searchDomObject.Attributes["Href"]).AbsoluteUri);

                    if (!string.IsNullOrEmpty(episodes))
                    {
                        CQ episodesQc = new CQ(episodes);

                        foreach (IDomObject episodeDomObject in episodesQc[".listing a"].ToList())
                        {
                            string episodeName = episodeDomObject.InnerText;
                            downloadSource.Downloads.Add(new Download
                            {
                                DownloadLink = GetVideo(client, new Uri(baseUri, episodeDomObject.Attributes["Href"]).AbsoluteUri),
                                EpisodeName = animeName,
                                FileName = "filename.extension",
                                Resolution = "???p"
                            });
                        }
                    }
                }
                return downloadSource;
            }
            return null;
        }

        private string GetVideo(WebClient client, string url)
        {
            client.Headers = defaultHeaders;
            string episode = client.DownloadString(url);

            if (!string.IsNullOrEmpty(episode))
            {
                CQ episodeQc = new CQ(episode);

                var target = episodeQc["video"][0].Attributes.ToList();
                var t1 = episodeQc["a[href *= 'googlevideo']"].ToList();

                return null;
            }
            return "";
        }

        private WebHeaderCollection CloneHeaders(WebHeaderCollection headerCollection)
        {
            WebHeaderCollection newHeaderCollection = new WebHeaderCollection();

            foreach (string key in headerCollection.AllKeys)
                newHeaderCollection.Add(key, headerCollection[key]);

            return newHeaderCollection;
        }
    }
}