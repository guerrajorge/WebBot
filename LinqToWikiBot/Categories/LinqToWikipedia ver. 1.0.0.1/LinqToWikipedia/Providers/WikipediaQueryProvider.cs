using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Net;

namespace LinqToWikipedia
{
    /// <summary>
    /// Wikipedia Linq Provider
    /// </summary>
    internal sealed class WikipediaQueryProvider : IQueryProvider
    {
        private WebProxy _proxy;
        private NetworkCredential _credentials;

        public WikipediaQueryProvider() { }

        public WikipediaQueryProvider(WebProxy proxy, NetworkCredential credentials)
        {
            _proxy = proxy;
            _credentials = credentials;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (_proxy != null || _credentials != null)
            {
                return new WikipediaQueryable<TElement>(expression, _proxy, _credentials);
            }
            else
            {
                return new WikipediaQueryable<TElement>(expression);
            }
        }

        public IQueryable CreateQuery(Expression expression)
        {
            try
            {
                SystemType linqtype = new SystemType();

                return (IQueryable)Activator.CreateInstance(typeof(WikipediaQueryable<>).MakeGenericType(linqtype.GetLinqElementType(expression.Type)), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression, WebProxy proxy, NetworkCredential credentials)
        {
            return (TResult)Execute(expression, proxy, credentials);
        }

        public object Execute(Expression expression)
        {
            SystemType linqtype = new SystemType();

            if (linqtype.GetLinqElementType(expression.Type) == typeof(WikipediaKeywordSearchResult))
                return WikipediaKeywordSearchResponse.Get(WikipediaKeywordSearchRequest.Send(linqtype.GetLinqElementType(expression.Type), expression));

            if (linqtype.GetLinqElementType(expression.Type) == typeof(WikipediaOpenSearchResult))
                return WikipediaOpenSearchResponse.Get(WikipediaOpenSearchRequest.Send(linqtype.GetLinqElementType(expression.Type), expression));

            return null;
        }

        public object Execute(Expression expression, WebProxy proxy, NetworkCredential credentials)
        {
            SystemType linqtype = new SystemType();

            if (linqtype.GetLinqElementType(expression.Type) == typeof(WikipediaKeywordSearchResult))
                return WikipediaKeywordSearchResponse.Get(WikipediaKeywordSearchRequest.Send(linqtype.GetLinqElementType(expression.Type), expression, proxy, credentials));

            if (linqtype.GetLinqElementType(expression.Type) == typeof(WikipediaOpenSearchResult))
                return WikipediaOpenSearchResponse.Get(WikipediaOpenSearchRequest.Send(linqtype.GetLinqElementType(expression.Type), expression, proxy, credentials));

            return null;
        }
    }
}
