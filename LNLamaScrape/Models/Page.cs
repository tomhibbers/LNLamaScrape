using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LNLamaScrape.Tests")]
namespace LNLamaScrape.Models
{
    public class Page : IPage
    {
        private readonly Chapter _parentChapterInternal;
        internal IChapter ParentChapter => _parentChapterInternal;
        public virtual string ParentRef { get; set; }
        public virtual string  PageRef { get; set; }
        public Uri PageUri { get; private set; }
        public int PageNo { get; private set; }
        public Uri ImageUri { get; internal set; }
        public string PageContent { get; internal set; }

        public IChapter GetParentChapter()
        {
            return ParentChapter;
        }

        public Page() { }
        public Page(Chapter parent, Uri pageUri, int pageNo)
        {
            _parentChapterInternal = parent;
            PageUri = pageUri;
            PageNo = pageNo;
        }
        internal Task<byte[]> GetPageImageAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetPageImageAsync(cts.Token);
            }
        }
        internal Task<byte[]> GetPageImageAsync(CancellationToken token)
        {
            return _parentChapterInternal.GetParentSeries().GetParentRepository().GetPageImageAsync(this, token);
        }
        internal Task<byte[]> GetPageTextAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetPageTextAsync(cts.Token);
            }
        }
        internal Task<byte[]> GetPageTextAsync(CancellationToken token)
        {
            return _parentChapterInternal.GetParentSeries().GetParentRepository().GetPageTextAsync(this, token);
        }

        public Task<byte[]> GetPageContentAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetPageContentAsync(cts.Token);
            }
        }
        public Task<byte[]> GetPageContentAsync(CancellationToken token)
        {
            return _parentChapterInternal.GetParentSeries().GetParentRepository().GetPageContentAsync(this, token);
        }
    }
}
