using System.Net;

namespace MyAnimeList.API
{
    internal class Authentication
    {
        public static bool VerifyCredentials(NetworkCredential loginCredentials)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = loginCredentials;
                    client.DownloadString("http://myanimelist.net/api/account/verifycredentials.xml");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}