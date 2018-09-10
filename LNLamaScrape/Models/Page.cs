using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;
using LNLamaScrape.Models.Interfaces;

namespace LNLamaScrape.Models
{
    internal class Page : IPage
    {
        public Chapter ParentChapterInternal { get; private set; }
        public IChapter ParentChapter => ParentChapterInternal;

        public Uri PageUri { get; private set; }
        public int PageNo { get; private set; }
        public Uri ImageUri { get; internal set; }

        internal Page(Chapter parent, Uri pageUri, int pageNo)
        {
            ParentChapterInternal = parent;
            PageUri = pageUri;
            PageNo = pageNo;
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
            return ParentChapterInternal.ParentSeriesInternal.ParentRepositoryInternal.GetPageContentAsync(this, token);
        }
    }
}
