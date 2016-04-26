using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
                    client.DownloadString("http://myanimelist.net/api/account/verify_credentials.xml");
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