using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LNLamaScrape.Tools
{
    internal interface IWebClient
    {
        Task<byte[]> GetByteArrayAsync(Uri uri, Uri referrer, CancellationToken token);
        Task<string> GetStringAsync(Uri uri, Uri referrer, CancellationToken token);
        Task<HttpResponseMessage> PostAsync(HttpContent content, Uri uri, Uri referrer, CancellationToken token);
    }
}
