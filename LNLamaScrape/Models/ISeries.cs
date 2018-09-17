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
        string Description { get; set; }

        string[] TitlesAlternative { get; set; }
        Uri SeriesPageUri { get; set; }
        Uri CoverImageUri { get; set; }
        string Author { get; set; }
        string Updated { get; set; }
        string[] Tags { get; set; }
        string[] Genres { get; set; }

        Task<byte[]> GetCoverAsync();
        Task<byte[]> GetCoverAsync(CancellationToken token);

        Task<IReadOnlyList<IChapter>> GetChaptersAsync();
        Task<IReadOnlyList<IChapter>> GetChaptersAsync(CancellationToken token);
        //void UpdateSeriesDetails(string description = null, string[] titlesAlternative = null,
        //    Uri coverImageUri = null, string author = null, string updated = null, string[] tags = null, string[] genres = null);
    }
}
