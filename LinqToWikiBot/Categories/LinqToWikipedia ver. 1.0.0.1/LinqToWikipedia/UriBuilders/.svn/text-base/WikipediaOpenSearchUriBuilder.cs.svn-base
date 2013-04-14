using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Globalization;

namespace LinqToWikipedia
{
    /// <summary>
    /// Build the Open Search Uri to be sent to the MediaWiki API
    /// </summary>
    internal sealed class WikipediaOpenSearchUriBuilder : ExpressionVisitor
    {
        private StringBuilder _urlBuilder;
        private Dictionary<string, string> _queryString;
        private string query = string.Empty;

        public WikipediaOpenSearchUriBuilder(Type searchResultType)
        {
            _queryString = new Dictionary<string, string>();
            _queryString["format"] = "xml";
            _queryString["action"] = "opensearch";

            _urlBuilder = new StringBuilder();

            _urlBuilder.Append("http://en.wikipedia.org/w/api.php?");
        }

        public Uri BuildUri(Expression expression)
        {
            foreach (KeyValuePair<string, string> parameter in _queryString)
            {
                _urlBuilder.Append(parameter.Key);
                _urlBuilder.Append("=");
                _urlBuilder.Append(Uri.EscapeDataString(parameter.Value));
                _urlBuilder.Append("&");
            }

            // parse expression tree
            Visit((MethodCallExpression)expression);

            return new Uri(_urlBuilder.ToString());
        }

        // override ExpressionVisitor method
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if ((m.Method.DeclaringType == typeof(Queryable)) || (m.Method.DeclaringType == typeof(Enumerable)))
            {
                if (m.Method.Name.Equals("Where"))
                {
                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);

                    ParseQuery(lambda.Body);

                    _urlBuilder.Append("search=" + query);

                    return m;
                }
                if (m.Method.Name.Equals("Take"))
                {
                    Visit(m.Arguments[0]);

                    ConstantExpression countExpression = (ConstantExpression)StripQuotes(m.Arguments[1]);

                    _urlBuilder.Append("&limit=" + ((int)countExpression.Value).ToString(CultureInfo.InvariantCulture));

                    return m;
                }
            }

            return m;
        }

        internal void ParseQuery(Expression e)
        {
            if (e is BinaryExpression)
            {
                BinaryExpression c = e as BinaryExpression;

                switch (c.NodeType)
                {
                    case ExpressionType.Equal:
                        GetCondition(c);
                        break;
                    default:
                        throw new NotSupportedException("Only .Equal is supported for this query");
                }
            }
            else
                throw new NotSupportedException("This querytype is not supported.");
        }

        internal void GetCondition(BinaryExpression e)
        {
            string val = string.Empty, attrib = string.Empty;

            if (e.Left is MemberExpression)
            {
                attrib = ((MemberExpression)e.Left).Member.Name;
                val = Expression.Lambda(e.Right).Compile().DynamicInvoke().ToString();
            }
            else if (e.Right is MemberExpression)
            {
                attrib = ((MemberExpression)e.Right).Member.Name;
                val = Expression.Lambda(e.Left).Compile().DynamicInvoke().ToString();
            }

            if (attrib.Equals("Keyword"))
            {
                if (query.Length > 0)
                    throw new NotSupportedException("'WikipediaOpenSearchResult' Query expression can only contain one 'Keyword'");
                else
                    query = val;
            }
            else
            {
                throw new NotSupportedException("'WikipediaOpenSearchResult' Query expression can only contain a'Keyword' parameter");
            }
        }
    }
}
