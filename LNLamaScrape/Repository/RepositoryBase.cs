using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models;
using LNLamaScrape.Tools;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LNLamaScrape.Tests")]

namespace LNLamaScrape.Repository
{
    public abstract class RepositoryBase : IRepository
    {
        public RepositoryType RepositoryType { get; }
        internal abstract Task<IReadOnlyList<IChapter>> GetChaptersAsync(ISeries input, CancellationToken token);
        internal abstract Task<IReadOnlyList<IPage>> GetPagesAsync(IChapter input, CancellationToken token);
        internal abstract Task<byte[]> GetPageImageAsync(IPage input, CancellationToken token);
        internal abstract Task<byte[]> GetPageTextAsync(IPage input, CancellationToken token);
        internal abstract Task<byte[]> GetPageContentAsync(IPage input, CancellationToken token);
        public readonly IWebClient WebClient;
        public string Name { get; private set; }
        public Uri RootUri { get; private set; }

        public bool SupportsCover { get; }
        public bool SupportsAuthor { get; }
        public bool SupportsLastUpdateTime { get; }
        public bool SupportsTags { get; }
        public bool SupportsDescription { get; }


        public RepositoryBase(IWebClient webClient, string name, string uriString, string iconFileName, bool supportsAllMetadata, RepositoryType repositoryType) :
            this(webClient, name, uriString, iconFileName, supportsAllMetadata, supportsAllMetadata, supportsAllMetadata, supportsAllMetadata, supportsAllMetadata, repositoryType)
        {

        }
        public RepositoryBase(IWebClient webClient, string name, string uriString, string iconFileName, bool supportsCover, bool supportsAuthor, bool supportsLastUpdateTime, bool supportsTags, bool supportsDescription, RepositoryType repositoryType)
        {
            WebClient = webClient;
            Name = name;
            RootUri = new Uri(uriString, UriKind.Absolute);

            SupportsCover = supportsCover;
            SupportsAuthor = supportsAuthor;
            SupportsLastUpdateTime = supportsLastUpdateTime;
            SupportsTags = supportsTags;
            SupportsDescription = supportsDescription;
            RepositoryType = repositoryType;
        }
        public Task<IReadOnlyList<ISeries>> GetSeriesAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                return GetSeriesAsync(cts.Token);
            }
        }
        public virtual Task<IReadOnlyList<ISeries>> GetSeriesAsync(CancellationToken token)
        {
            return Task.FromResult<IReadOnlyList<ISeries>>(new ISeries[0]);
        }
        internal virtual async Task<byte[]> GetCoverImageAsync(ISeries input, CancellationToken token)
        {
            var output = default(byte[]);
            if (!SupportsCover)
            {
                return null;
            }

            if (input.CoverImageUri == null)
            {
                await input.GetChaptersAsync(token);
            }

            if (input.CoverImageUri != null && !token.IsCancellationRequested)
            {
                output = await WebClient.GetByteArrayAsync(input.CoverImageUri, input.SeriesPageUri, token);
            }

            return output;
        }
    }
}
