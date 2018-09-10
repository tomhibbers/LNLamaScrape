using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models.Interfaces;

namespace LNLamaScrape.Models
{
    internal class Chapter : IChapter
    {
        internal Series ParentSeriesInternal { get; private set; }
        public ISeries ParentSeries => ParentSeriesInternal;

        public Uri FirstPageUri { get; private set; }
        public string Title { get; private set; }
        public string Updated { get; private set; }

        internal Chapter(Series parent, Uri firstPageUri, string title)
        {
            ParentSeriesInternal = parent;
            FirstPageUri = firstPageUri;
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
