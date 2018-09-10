using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LNLamaScrape.Models.Interfaces
{
    public interface IChapter
    {
        ISeries ParentSeries { get; }

        string Title { get; }
        string Updated { get; }
        Uri FirstPageUri { get; }

        Task<IReadOnlyList<IPage>> GetPagesAsync();
        Task<IReadOnlyList<IPage>> GetPagesAsync(CancellationToken token);
    }
}
