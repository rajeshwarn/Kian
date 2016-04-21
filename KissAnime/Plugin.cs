using Cloudflare_Evader;
using CsQuery;
using Kian.Core;
using Kian.Core.Objects.Anime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

                IDomObject searchDomObject = searchCq[".listing a"][0];
                string animeName = searchDomObject.InnerText;

                client.Headers = defaultHeaders;
                string episodes = client.DownloadString(new Uri(baseUri, searchDomObject.Attributes["Href"]).AbsoluteUri);

                if (!string.IsNullOrEmpty(episodes))
                {
                    CQ episodesQc = new CQ(episodes);

                    Dictionary<string, DownloadGroup> dlg = new Dictionary<string, DownloadGroup>();

                    foreach (IDomObject episodeDomObject in episodesQc[".listing a"].ToList())
                    {
                        string episodeName = episodeDomObject.InnerText.Substring(41);

                        foreach (Download dl in GetDownloads(client, new Uri(baseUri, episodeDomObject.Attributes["Href"]).AbsoluteUri, episodeName))
                        {
                            if (!dlg.ContainsKey(dl.Resolution))
                                dlg.Add(dl.Resolution, new DownloadGroup() { GroupName = dl.Resolution });

                            dlg[dl.Resolution].Downloads.Add(dl);
                        }
                    }

                    foreach (KeyValuePair<string, DownloadGroup> kvp in dlg)
                        downloadSource.DownloadGroups.Add(kvp.Value);
                    
                }
                if (downloadSource.DownloadGroups.Count > 0)
                    return downloadSource;
                else
                    return null;
            }
            return null;
        }

        private List<Download> GetDownloads(WebClient client, string url, string name)
        {
            List<Download> downloads = new List<Download>();

            client.Headers = defaultHeaders;
            string episode = client.DownloadString(url);

            if (!string.IsNullOrEmpty(episode))
            {
                CQ episodeQc = new CQ(episode);

                foreach (IDomObject quality in episodeQc["#selectQuality > option"].ToList())
                {
                    downloads.Add(new Download
                    {
                        EpisodeName = name,
                        DownloadLink = Encoding.UTF8.GetString(Convert.FromBase64String(quality.Attributes["value"])),
                        Resolution = quality.InnerText
                    });
                }
            }
            return downloads;
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