using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LNLamaScrape.Models.Interfaces
{
    public interface IPage
    {
        IChapter ParentChapter { get; }
        Uri ImageUri { get; }
        int PageNo { get; }
        Uri PageUri { get; }

        Task<byte[]> GetPageContentAsync();
        Task<byte[]> GetPageContentAsync(CancellationToken token);
    }
}
