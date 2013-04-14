using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Net;
using System.IO;

namespace LinqToWikipedia
{
    /// <summary>
    /// Prepare MediaWiki Uri request for Keyword Search and send
    /// </summary>
    internal static class WikipediaOpenSearchRequest
    {
        internal static string Send(Type elementType, Expression expression)
        {
            return Send(elementType, expression, null, null);
        }

        internal static string Send(Type elementType, Expression expression, WebProxy proxy, NetworkCredential credentials)
        {
            WikipediaOpenSearchUriBuilder uriBuilder = new WikipediaOpenSearchUriBuilder(elementType);

            Uri uri = uriBuilder.BuildUri(expression);

            return HttpRequest.Send(proxy, credentials, uri);
        }
    }
}
