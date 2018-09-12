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
        Task<IReadOnlyList<IChapter>> GetChaptersAsync(ISeries input, CancellationToken token);
        Task<IReadOnlyList<IPage>> GetPagesAsync(IChapter input, CancellationToken token);
        Task<byte[]> GetPageImageAsync(IPage input, CancellationToken token);
        Task<byte[]> GetPageTextAsync(IPage input, CancellationToken token);
        Task<byte[]> GetPageContentAsync(IPage input, CancellationToken token);
    }
}
