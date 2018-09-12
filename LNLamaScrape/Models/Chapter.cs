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
        readonly Series _parentSeriesInternal;
        internal ISeries ParentSeries => _parentSeriesInternal;
        public string ParentRef { get; set; }
        public Uri FirstPageUri { get; private set; }
        public string ChapterRef { get; set; }
        public string Title { get; private set; }
        public string Updated { get; private set; }

        public Chapter(Series parent, Uri firstPageUri, string title)
        {
            _parentSeriesInternal = parent;
            if (_parentSeriesInternal != null)
                ParentRef = ParentSeries?.Title;
            FirstPageUri = firstPageUri;
            ChapterRef = string.Join('-', ParentRef, FirstPageUri.Segments.Last());
            Title = title;
            Updated = string.Empty;
        }

        public ISeries GetParentSeries()
        {
            return this.ParentSeries;
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
            return _parentSeriesInternal.GetParentRepository().GetPagesAsync(this, token);
        }

    }

    //public class Chap2 : Chapter
    //{
    //    //public override Series ParentSeriesInternal { get; set; }
    //    //public Chap2(Series parent, Uri firstPageUri, string title) : base(parent, firstPageUri, title)
    //    //{

    //    //}
    //}
}
