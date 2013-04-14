using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace LinqToWikipedia
{
    /// <summary>
    /// 
    /// </summary>
    public class WikipediaContext
    {
        private WebProxy _proxy = null;
        private NetworkCredential _credentials = null;

        public WikipediaContext()
        {

        }

        public WikipediaContext(WebProxy proxy, NetworkCredential credentials)
        {
            _proxy = proxy;
            _credentials = credentials;
        }

        public IWikipediaQueryable<WikipediaKeywordSearchResult> KeywordSearch
        {
            get
            {
                if (_proxy != null || _credentials != null)
                    return new WikipediaQueryable<WikipediaKeywordSearchResult>(_proxy, _credentials);

                return new WikipediaQueryable<WikipediaKeywordSearchResult>();
            }
        }

        public IWikipediaQueryable<WikipediaOpenSearchResult> OpenSearch
        {
            get
            {
                if (_proxy != null || _credentials != null)
                    return new WikipediaQueryable<WikipediaOpenSearchResult>(_proxy, _credentials);

                return new WikipediaQueryable<WikipediaOpenSearchResult>();
            }
        }
    }
}
