using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace LinqToWikipedia
{
    /// <summary>
    /// Parse Http response and popluate WikipediaKeywordSearchResult class
    /// </summary>
    internal static class WikipediaKeywordSearchResponse
    {
        internal static IEnumerable<WikipediaKeywordSearchResult> Get(string xml)
        {
            List<WikipediaKeywordSearchResult> resultList = new List<WikipediaKeywordSearchResult>();

            int recordcount = 0;

            try
            {
                var descendants = from i in XDocument.Parse(xml).Descendants()
                                  select i;

                foreach (XElement element in descendants)
                {
                    if (element.Name.LocalName.ToString().Equals("searchinfo"))
                    {
                        recordcount = Convert.ToInt32(element.Attribute("totalhits").Value);
                    }

                    if (element.Name.LocalName.ToString().Equals("p"))
                    {
                        WikipediaKeywordSearchResult wsr = new WikipediaKeywordSearchResult();

                        var items = from x in element.Attributes()
                                    select x;

                        foreach (XAttribute item in items)
                        {
                            switch (item.Name.LocalName.ToString())
                            {
                                case "title":
                                    wsr.Title = item.Value;
                                    wsr.Url = new Uri("http://en.wikipedia.org/wiki/" + item.Value);
                                    break;
                                case "snippet":
                                    wsr.Description = item.Value;
                                    break;
                                case "timestamp":
                                    wsr.TimeStamp = Convert.ToDateTime(item.Value);
                                    break;
                                case "wordcount":
                                    wsr.WordCount = Convert.ToInt32(item.Value);
                                    break;
                            }
                        }

                        wsr.RecordCount = recordcount;

                        resultList.Add(wsr);
                    }
                }
            }
            catch (XmlException xmlerr)
            {
                throw new LinqToWikipediaException(xmlerr.Message, xmlerr);
            }
            catch (Exception e)
            {
                throw new LinqToWikipediaException(e.Message, e);
            }

            return resultList;
        }

    }
}
