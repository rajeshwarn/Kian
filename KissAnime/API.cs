using Cloudflare_Evader;
using CsQuery;
using Kian.Core.Objects;
using KissAnime.Objects;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KissAnime
{
    internal class API
    {
        private static WebClient client = null;

        public static Uri BaseUri { get; set; } = new Uri("https://kissanime.to/");
        public static string SearchUrl { get; set; } = "https://kissanime.to/AdvanceSearch";
        public static string PostDataTemplate { get; set; } = @"animeName={0}&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&status=";

        private static WebClient Client
        {
            get
            {
                while (client == null)
                {
                    Console.WriteLine("Trying to bypass Cloudflare...");
                    WebClient localClient = CloudflareEvader.CreateBypassedWebClient(BaseUri.AbsoluteUri);

                    if (localClient != null)
                        defaultHeaders = localClient.Headers;

                    client = localClient;
                }

                return client;
            }
        }

        private static WebHeaderCollection defaultHeaders;

        public static WebClient GetWebClient()
        {
            WebClientEx localClient = new WebClientEx(((WebClientEx)Client).CookieContainer);
            localClient.Headers = CloneHeaders(defaultHeaders);
            return localClient;
        }

        public static List<Anime> Search(string searchString)
        {
            using (WebClient localClient = GetWebClient())
            {
                List<Anime> searchResults = new List<Anime>();

                localClient.Headers = CloneHeaders(defaultHeaders);
                localClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                string search = localClient.UploadString(SearchUrl, string.Format(PostDataTemplate, searchString));

                if (!string.IsNullOrEmpty(search))
                {
                    CQ searchCq = new CQ(search);

                    foreach (IDomObject searchDomObject in searchCq[".listing a"])
                    {
                        if (!searchDomObject.Attributes["Href"].Contains("?id="))
                        {
                            localClient.Headers = CloneHeaders(defaultHeaders);
                            Uri episodesUri = new Uri(BaseUri, searchDomObject.Attributes["Href"]);
                            string episodes = localClient.DownloadString(episodesUri);

                            Anime anime = new Anime();
                            anime.Title = searchDomObject.InnerText.Substring(17);

                            if (!string.IsNullOrEmpty(episodes))
                                anime.EpisodesCq = new CQ(episodes);

                            searchResults.Add(anime);
                        }
                    }
                }

                if (searchResults.Count > 0)
                    return searchResults;
                else
                    return null;
            }
        }

        public static List<Download> GetDownloads(WebClient client, string url, string name)
        {
            List<Download> downloads = new List<Download>();
            string episode;

            using (WebClient localClient = GetWebClient())
            {
                client.Headers = CloneHeaders(defaultHeaders);
                episode = client.DownloadString(url);

                // Check if we were detected and attacked by an evil captcha. If we are, we can't continue :(
                if (episode.Contains("AreYouHuman"))
                    return null;
            }

            if (!string.IsNullOrEmpty(episode))
            {
                CQ episodeQc = new CQ(episode);

                foreach (IDomObject quality in episodeQc["#selectQuality > option"].ToList())
                {
                    downloads.Add(new Download
                    {
                        EpisodeName = name,
                        DownloadLink = Encoding.UTF8.GetString(Convert.FromBase64String(quality.Attributes["value"])),
                        Resolution = int.Parse(new Regex(@"[0-9]+").Match(quality.InnerText).Value),
                        SizeRequestReferrer = "http://www.animebam.net/",
                    });
                }
            }
            return downloads;
        }

        private static WebHeaderCollection CloneHeaders(WebHeaderCollection headerCollection)
        {
            WebHeaderCollection newHeaderCollection = new WebHeaderCollection();

            foreach (string key in headerCollection.AllKeys)
                newHeaderCollection.Add(key, headerCollection[key]);

            return newHeaderCollection;
        }
    }
}