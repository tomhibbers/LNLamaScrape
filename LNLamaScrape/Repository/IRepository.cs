using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models;

namespace LNLamaScrape.Repository
{
    public interface IRepository
    {
        string Name { get; }
        Uri RootUri { get; }
        bool SupportsCover { get; }
        bool SupportsAuthor { get; }
        bool SupportsLastUpdateTime { get; }
        bool SupportsTags { get; }
        bool SupportsDescription { get; }
        Task<IReadOnlyList<ISeries>> GetSeriesAsync();
        Task<IReadOnlyList<ISeries>> GetSeriesAsync(CancellationToken token);
    }
}
