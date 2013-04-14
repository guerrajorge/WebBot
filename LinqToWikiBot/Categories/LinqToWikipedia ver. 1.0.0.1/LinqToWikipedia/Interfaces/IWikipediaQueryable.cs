using System.Linq;

namespace LinqToWikipedia
{
    /// <summary>
    /// Interface to extend the IQuerable interface
    /// </summary>
    /// <typeparam name="T">Generic query result class</typeparam>
    public interface IWikipediaQueryable<T> : IQueryable<T> { }
}
