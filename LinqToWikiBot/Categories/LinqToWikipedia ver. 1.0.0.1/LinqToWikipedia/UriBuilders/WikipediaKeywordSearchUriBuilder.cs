using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqToWikipedia
{
    /// <summary>
    /// Build the Query Uri to be sent to the MediaWiki API
    /// </summary>
    internal sealed class WikipediaKeywordSearchUriBuilder : ExpressionVisitor
    {
        private StringBuilder _urlBuilder;
        private Dictionary<string, string> _queryString;
        private string query = string.Empty;

        public WikipediaKeywordSearchUriBuilder(Type searchResultType)
        {
            _queryString = new Dictionary<string, string>();
            _queryString["format"] = "xml";
            _queryString["action"] = "query";
            _queryString["list"] = "search";

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

                    _urlBuilder.Append("srsearch=" + query.Substring(0, query.Length -1));

                    return m;
                }
                if (m.Method.Name.Equals("Skip"))
                {
                    Visit(m.Arguments[0]);

                    ConstantExpression countExpression = (ConstantExpression)StripQuotes(m.Arguments[1]);

                    _urlBuilder.Append("&sroffset=" + ((int)countExpression.Value).ToString(CultureInfo.InvariantCulture));

                    return m;
                }
                if (m.Method.Name.Equals("Take"))
                {
                    Visit(m.Arguments[0]);

                    ConstantExpression countExpression = (ConstantExpression)StripQuotes(m.Arguments[1]);

                    _urlBuilder.Append("&srlimit=" + ((int)countExpression.Value).ToString(CultureInfo.InvariantCulture));

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
                    case ExpressionType.AndAlso:
                        ParseQuery(c.Left);
                        ParseQuery(c.Right);
                        break;
                    default:
                        throw new NotSupportedException("Only .Equal and .AndAlso are supported for this query");
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
                query += val + "|";
            }
            else
            {
                throw new NotSupportedException("'WikipediaQueryResult' Query expression can only contain multiple 'Keyword's");
            }
        }
    }
}
