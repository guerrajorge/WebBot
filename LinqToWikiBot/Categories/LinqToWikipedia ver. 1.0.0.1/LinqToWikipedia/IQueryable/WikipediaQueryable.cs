using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Linq.Expressions;
using System.Net;

namespace LinqToWikipedia
{
    /// <summary>
    /// Concrete Wikipedia Query implementation of the IQueryable interface
    /// </summary>
    /// <typeparam name="T">Wikipedia IQueryable provider</typeparam>
    internal sealed class WikipediaQueryable<T> : IWikipediaQueryable<T>
    {
        private Expression _expression;
        private WebProxy _proxy;
        private NetworkCredential _credentials;

        public WikipediaQueryable()
        {
            _expression = Expression.Constant(this);
        }

        public WikipediaQueryable(WebProxy proxy, NetworkCredential credentials)
        {
            _expression = Expression.Constant(this);

            _proxy = proxy;
            _credentials = credentials;
        }

        public WikipediaQueryable(Expression expression)
        {
            _expression = expression;
        }

        public WikipediaQueryable(Expression expression, WebProxy proxy, NetworkCredential credentials)
        {
            _expression = expression;

            _proxy = proxy;
            _credentials = credentials;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (_proxy != null || _credentials != null)
            {
                WikipediaQueryProvider provider = new WikipediaQueryProvider(_proxy, _credentials);

                IEnumerable<T> sequence = provider.Execute<IEnumerable<T>>(_expression, _proxy, _credentials);

                return sequence.GetEnumerator();
            }
            else
            {
                WikipediaQueryProvider provider = new WikipediaQueryProvider();

                IEnumerable<T> sequence = provider.Execute<IEnumerable<T>>(_expression);

                return sequence.GetEnumerator();
            }
        }

        Type IQueryable.ElementType { get { return typeof(T); } }

        Expression IQueryable.Expression { get { return _expression; } }

        IQueryProvider IQueryable.Provider 
        {
            get
            {
                if (_proxy != null || _credentials != null)
                {
                    WikipediaQueryProvider provider = new WikipediaQueryProvider(_proxy, _credentials);
                    return provider;
                }
                else
                {
                    WikipediaQueryProvider provider = new WikipediaQueryProvider();
                    return provider;
                }
            }
        }

    }
}
