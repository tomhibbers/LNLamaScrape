using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models;
using WebClient = LNLamaScrape.Tools.WebClient;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LNLamaScrape.Tests")]

namespace LNLamaScrape.Repository
{
    internal class MangaHereRepository : RepositoryBase
    {
        private static readonly Uri RepoIndexUri = new Uri("http://www.mangahere.co/mangalist/");

        public MangaHereRepository(WebClient webClient) : base(webClient, "Manga Here", "http://www.mangahere.co/",
            "MangaHere.png", false, repositoryType: RepositoryType.Manga)
        {
        }

        public override async Task<IReadOnlyList<ISeries>> GetSeriesAsync(CancellationToken token)
        {
            var html = await WebClient.GetStringAsync(RepoIndexUri, RootUri, token);
            if (html == null)
            {
                return null;
            }

            var parser = new HtmlParser();
            var document = parser.Parse(html);

            var nodes = document.QuerySelectorAll("a.manga_info");

            var output = nodes.Select(d => new Series(this, new Uri(RootUri, d.Attributes["href"].Value),
                WebUtility.HtmlDecode(d.Attributes["rel"].Value))).OrderBy(d => d.Title);
            return output.ToArray();
        }

        public override async Task<IReadOnlyList<IChapter>> GetChaptersAsync(ISeries input, CancellationToken token)
        {
            var html = await WebClient.GetStringAsync(input.SeriesPageUri, RepoIndexUri, token);
            if (html == null)
            {
                return null;
            }

            var parser = new HtmlParser();
            var document = parser.Parse(html);

            var node = document.QuerySelector("div.manga_detail");
            node = node.QuerySelector("div.detail_list ul");
            var nodes = node.QuerySelectorAll("a.color_0077");

            var Output = nodes.Select(d =>
            {
                string Title = d.TextContent;
                Title = Regex.Replace(Title, @"^[\r\n\s\t]+", string.Empty);
                Title = Regex.Replace(Title, @"[\r\n\s\t]+$", string.Empty);
                var Chapter = new Chapter((Series)input, new Uri(RootUri, d.Attributes["href"].Value), Title);
                return Chapter;
            }).Reverse().ToArray();

            return Output.ToArray();
        }

        public override async Task<IReadOnlyList<IPage>> GetPagesAsync(IChapter input, CancellationToken token)
        {
            var html = await WebClient.GetStringAsync(input.FirstPageUri, input.GetParentSeries().SeriesPageUri, token);
            if (html == null)
            {
                return null;
            }

            var parser = new HtmlParser();
            var document = parser.Parse(html);

            var node = document.QuerySelector("section.readpage_top");
            node = node.QuerySelector("span.right select");
            var nodes = node.QuerySelectorAll("option");

            var Output = nodes.Select((d, e) =>
                new Page((Chapter)input, new Uri(RootUri, d.Attributes["value"].Value), e + 1));
            return Output.ToArray();
        }

        public override async Task<byte[]> GetPageContentAsync(IPage input, CancellationToken token)
        {
            if (RepositoryType == RepositoryType.LightNovel)
                return await GetPageTextAsync(input, token);
            return await GetPageImageAsync(input, token);
        }

        public override async Task<byte[]> GetPageImageAsync(IPage input, CancellationToken token)
        {
            var html = await WebClient.GetStringAsync(input.PageUri, input.GetParentChapter().FirstPageUri, token);
            if (html == null)
            {
                return null;
            }

            var parser = new HtmlParser();
            var document = parser.Parse(html);

            var node = document.QuerySelector("img#image");
            var imageUri = new Uri(node.Attributes["src"].Value);

            ((Page)input).ImageUri = new Uri(RootUri, imageUri);
            var output = await WebClient.GetByteArrayAsync(input.ImageUri, input.PageUri, token);
            if (token.IsCancellationRequested)
            {
                return null;
            }

            return output;

        }

        public override Task<byte[]> GetPageTextAsync(IPage input, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}