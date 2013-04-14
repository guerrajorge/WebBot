using System;
namespace LinqToWikipedia
{
    /// <summary>
    /// Interface for Query result set
    /// </summary>
    interface IWikipediaKeywordSearchResult
    {
        string Description { get; set; }
        string Keyword { get; set; }
        int RecordCount { get; set; }
        DateTime TimeStamp { get; set; }
        string Title { get; set; }
        Uri Url { get; set; }
        int WordCount { get; set; }
    }
}
