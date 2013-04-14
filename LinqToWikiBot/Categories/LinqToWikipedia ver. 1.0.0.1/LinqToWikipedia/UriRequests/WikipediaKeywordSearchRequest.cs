using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Linq.Expressions;

namespace LinqToWikipedia
{
    /// <summary>
    /// Prepare MediaWiki Uri request for Open Search and send
    /// </summary>
    internal static class WikipediaKeywordSearchRequest
    {
        internal static string Send(Type elementType, Expression expression)
        {
            return Send(elementType, expression, null, null);
        }

        internal static string Send(Type elementType, Expression expression, WebProxy proxy, NetworkCredential credentials)
        {
            WikipediaKeywordSearchUriBuilder uriBuilder = new WikipediaKeywordSearchUriBuilder(elementType);

            Uri uri = uriBuilder.BuildUri(expression);

            return HttpRequest.Send(proxy, credentials, uri);
        }

       
    }
}
