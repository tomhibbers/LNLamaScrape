using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LNLamaScrape.Models
{
    public interface IPage
    {
        IChapter GetParentChapter();
        Uri ImageUri { get; set; }
        int PageNo { get; set; }
        Uri PageUri { get; set; }
        string PageContent { get; set; }

        Task<byte[]> GetPageContentAsync();
        Task<byte[]> GetPageContentAsync(CancellationToken token);
    }
}
