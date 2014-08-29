using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WebDaD.Toolkit.Communications
{
    public static class Web
    {
        public static WebResponse GetPageContent(string url)
        {
            WebClient client = new WebClient();
            try
            {
                string res = client.DownloadString(url);
                return new WebResponse(HttpStatusCode.OK, res);
            }
            catch (WebException we)
            {
                HttpWebResponse r = (HttpWebResponse)we.Response;
                return new WebResponse(r.StatusCode, r.StatusDescription);
            }
        }
        public static WebResponse GetFile(string url, string target)
        {
            WebClient client = new WebClient();
            try
            {
                client.DownloadFile(url, target);
                return new WebResponse(HttpStatusCode.OK, target);
            }
            catch (WebException we)
            {
                HttpWebResponse r = (HttpWebResponse)we.Response;
                return new WebResponse(r.StatusCode, r.StatusDescription);
            }
        }
    }
}
