using Cloudflare_Evader;
using CsQuery;
using KissAnime.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace KissAnime
{
    internal class API
    {
        private static WebClient client = null;

        private static WebHeaderCollection defaultHeaders;
        public static Uri BaseUri { get; set; } = new Uri("https://kissanime.to/");
        public static string PostDataTemplate { get; set; } = @"animeName={0}&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&genres=0&status=";
        public static string SearchUrl { get; set; } = "https://kissanime.to/AdvanceSearch";

        private static WebClient Client
        {
            get
            {
                while (client == null)
                {
                    Console.WriteLine("Trying to bypass Cloudflare...");
                    WebClient localClient = null;

                    try
                    {
                        localClient = CloudflareEvader.CreateBypassedWebClient(BaseUri.AbsoluteUri);

                        if (localClient != null)
                            defaultHeaders = localClient.Headers;

                        client = localClient;
                    }
                    catch (WebException ex)
                    {
                        Trace.TraceError("Could not get BypassedWebClient from CloudflareEvader. (Error: {0})", ex.Message);

                        return null;
                    }
                }

                return client;
            }
        }

        public static WebHeaderCollection CloneHeaders(WebHeaderCollection headerCollection)
        {
            WebHeaderCollection newHeaderCollection = new WebHeaderCollection();

            foreach (string key in headerCollection.AllKeys)
                newHeaderCollection.Add(key, headerCollection[key]);

            return newHeaderCollection;
        }

        public static List<AnimeDownload> GetDownloads(string url, string name, int maxRetries = 3, int currentTry = 0)
        {
            List<AnimeDownload> downloads = new List<AnimeDownload>();
            string episode = null;

            try
            {
                using (WebClient localClient = GetWebClient())
                {
                    localClient.Headers = CloneHeaders(defaultHeaders);
                    episode = localClient.DownloadString(url);

                    // Check if we were detected and attacked by an evil captcha. If we are, we can't continue :(
                    // TODO: Notify user if KissAnime throws a captcha at us. (Low priority, it hasn't done that since implementing anti-bot-finding things like "it's a bad idea to request 347 different URL's at the same time.")
                    if (episode.Contains("AreYouHuman"))
                        return null;
                }
            }
            catch (WebException)
            {
                if (currentTry < maxRetries)
                {
                    GetDownloads(url, name, maxRetries, currentTry);
                }
                else
                {
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(episode))
            {
                CQ episodeQc = new CQ(episode);

                foreach (IDomObject quality in episodeQc["#selectQuality > option"].ToList())
                {
                    downloads.Add(new AnimeDownload
                    {
                        Title = name,
                        DownloadLink = Encoding.UTF8.GetString(Convert.FromBase64String(quality.Attributes["value"])),
                        Resolution = int.Parse(new Regex(@"[0-9]+").Match(quality.InnerText).Value),
                        SizeRequestReferrer = "http://www.animebam.net/",
                    });
                }
            }
            return downloads;
        }

        public static WebClient GetWebClient()
        {
            WebClientEx localClient = new WebClientEx(((WebClientEx)Client).CookieContainer);
            return localClient;
        }

        public static List<Anime> Search(string searchString)
        {
            using (WebClient localClient = GetWebClient())
            {
                List<Anime> searchResults = new List<Anime>();
                string search;

                //try
                //{
                localClient.Headers = CloneHeaders(defaultHeaders);
                localClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                search = localClient.UploadString(SearchUrl, string.Format(PostDataTemplate, searchString));
                //}catch()

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
    }
}