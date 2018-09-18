using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models;
using LNLamaScrape.Repository;

namespace LNLamaScrape.Models
{
    public class Series : ISeries
    {
        readonly RepositoryBase _parentRepositoryInternal;
        internal IRepository ParentRepository => _parentRepositoryInternal;
        public string Title { get; set; }

        public string Description { get; set; }
        public string[] TitlesAlternative { get; set; }
        public Uri SeriesPageUri { get; set; }
        public Uri CoverImageUri { get; set; }
        public string Author { get; set; }
        public string Updated { get; set; }
        public string[] Tags { get; set; }
        public string[] Genres { get; set; }
        public int Rating { get; set; }
        public SeriesStatus Status { get; set; }
        public virtual string SeriesRef { get; set; }

        public Series() { }
        public Series(RepositoryBase parent, Uri seriesPageUri, string title)
        {
            _parentRepositoryInternal = parent;
            SeriesPageUri = seriesPageUri;
            Title = title;
            Updated = string.Empty;
        }

        public IRepository GetParentRepository()
        {
            return ParentRepository;
        }
        public Task<byte[]> GetCoverAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetCoverAsync(cts.Token);
            }
        }
        public Task<byte[]> GetCoverAsync(CancellationToken token)
        {
            return _parentRepositoryInternal.GetCoverImageAsync(this, token);
        }

        public Task<IReadOnlyList<IChapter>> GetChaptersAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetChaptersAsync(cts.Token);
            }
        }

        public virtual Task<IReadOnlyList<IChapter>> GetChaptersAsync(CancellationToken token)
        {
            return _parentRepositoryInternal.GetChaptersAsync(this, token);
        }
    }
}
