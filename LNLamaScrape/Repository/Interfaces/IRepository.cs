using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models;
using LNLamaScrape.Models.Interfaces;

namespace LNLamaScrape.Repository.Interfaces
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
