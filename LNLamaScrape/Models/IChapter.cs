using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LNLamaScrape.Models
{
    public interface IChapter
    {
        ISeries GetParentSeries();
        string ParentRef { get; set; }

        string Title { get; set; }
        string Updated { get; set; }
        Uri FirstPageUri { get; set; }

        Task<IReadOnlyList<IPage>> GetPagesAsync();
        Task<IReadOnlyList<IPage>> GetPagesAsync(CancellationToken token);
    }
}
