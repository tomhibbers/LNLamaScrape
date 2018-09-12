using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models;

namespace LNLamaScrape.Models
{
    public class Chapter : IChapter
    {
        public Series ParentSeriesInternal { get; private set; }
        public ISeries ParentSeries => ParentSeriesInternal;
        public string ParentRef { get; set; }
        public Uri FirstPageUri { get; private set; }
        public string ChapterRef { get; set; }
        public string Title { get; private set; }
        public string Updated { get; private set; }

        public Chapter(Series parent, Uri firstPageUri, string title)
        {
            ParentSeriesInternal = parent;
            if (ParentSeriesInternal != null)
                ParentRef = ParentSeries?.Title;
            FirstPageUri = firstPageUri;
            ChapterRef = string.Join('-', ParentRef, FirstPageUri.Segments.Last());
            Title = title;
            Updated = string.Empty;
        }

        public virtual Task<IReadOnlyList<IPage>> GetPagesAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetPagesAsync(cts.Token);
            }
        }
        public virtual Task<IReadOnlyList<IPage>> GetPagesAsync(CancellationToken token)
        {
            return ParentSeriesInternal.ParentRepositoryInternal.GetPagesAsync(this, token);
        }

    }
}
