using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Linq;

namespace LinqToWikipedia
{
    /// <summary>
    /// Parse Http response and popluate WikipediaOpenSearchResult class
    /// </summary>
    internal static class WikipediaOpenSearchResponse
    {
        internal static IEnumerable<WikipediaOpenSearchResult> Get(string xml)
        {
            List<WikipediaOpenSearchResult> resultList = new List<WikipediaOpenSearchResult>();

            try
            {
                var descendants = from i in XDocument.Parse(xml).Descendants()
                                  select i;

                foreach (XElement element in descendants)
                {
                    if (element.Name.LocalName.ToString().Equals("Item"))
                    {
                        WikipediaOpenSearchResult wsr = new WikipediaOpenSearchResult();

                        var items = from x in element.Nodes()
                                    select x;

                        foreach (XElement item in items)
                        {
                            switch (item.Name.LocalName.ToString())
                            {
                                case "Text":
                                    wsr.Text = item.Value;
                                    break;
                                case "Description":
                                    wsr.Description = item.Value;
                                    break;
                                case "Url":
                                    wsr.Url = new Uri(item.Value);
                                    break;
                                case "Image":
                                    wsr.ImageUrl = new Uri(item.Attribute("source").Value);
                                    break;
                            }
                        }

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
