using System;
namespace LinqToWikipedia
{
    /// <summary>
    /// Interface for Open Search result set
    /// </summary>
    interface IWikipediaOpenSearchResult
    {
        string Description { get; set; }
        Uri ImageUrl { get; set; }
        string Keyword { get; set; }
        string Text { get; set; }
        Uri Url { get; set; }
    }
}
