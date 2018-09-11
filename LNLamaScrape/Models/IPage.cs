using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LNLamaScrape.Models
{
    public interface IPage
    {
        IChapter ParentChapter { get; }
        Uri ImageUri { get; }
        int PageNo { get; }
        Uri PageUri { get; }
        string PageContent { get; }

        Task<byte[]> GetPageContentAsync();
        Task<byte[]> GetPageContentAsync(CancellationToken token);
    }
}
