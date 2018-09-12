using LNLamaScrape.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LNLamaScrape.Models
{
    public interface ISeries
    {
        IRepository GetParentRepository();
        string Title { get; }
        string Description { get; }

        string[] TitlesAlternative { get; }
        Uri SeriesPageUri { get; }
        Uri CoverImageUri { get; }
        string Author { get; }
        string Updated { get; }
        string[] Tags { get; }

        Task<byte[]> GetCoverAsync();
        Task<byte[]> GetCoverAsync(CancellationToken token);

        Task<IReadOnlyList<IChapter>> GetChaptersAsync();
        Task<IReadOnlyList<IChapter>> GetChaptersAsync(CancellationToken token);
        void UpdateSeriesDetails(string description = null, string[] titlesAlternative = null,
            Uri coverImageUri = null, string author = null, string updated = null, string[] tags = null, string[] genres = null);
    }
}
