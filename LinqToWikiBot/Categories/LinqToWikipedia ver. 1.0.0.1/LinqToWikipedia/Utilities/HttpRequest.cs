using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace LinqToWikipedia
{
    /// <summary>
    /// Http helper class
    /// </summary>
    internal class HttpRequest
    {
        internal static string Send(WebProxy proxy, NetworkCredential credentials, Uri uri)
        {
            HttpWebResponse webResponse;

            string xml = string.Empty;

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                //webRequest.UserAgent = "Trivia Game";
                webRequest.Proxy = null;

                if (proxy != null)
                    webRequest.Proxy = proxy;

                if (credentials != null)
                    webRequest.Proxy.Credentials = credentials;

                webRequest.UserAgent = "Mozilla/5.0";

                webResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (HttpListenerException httperr)
            {
                throw new LinqToWikipediaException(httperr.Message, httperr);
            }
            catch (Exception e)
            {
                throw new LinqToWikipediaException(e.Message, e);
            }

            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                    xml = sr.ReadToEnd();
            }

            return xml;
        }
    }
}
