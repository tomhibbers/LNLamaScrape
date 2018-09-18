using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using LNLamaScrape.Models;
using LNLamaScrape.Tools;
using static System.Int32;
using WebClient = LNLamaScrape.Tools.WebClient;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LNLamaScrape.Tests")]

namespace LNLamaScrape.Repository
{
    internal class WebNovelRepository : RepositoryBase
    {
        public static readonly RepositoryType RepositoryType = RepositoryType.LightNovel;
        private static readonly Uri RepoIndexUri = new Uri("https://www.webnovel.com/category/list?category=0&orderBy=4");

        public WebNovelRepository(WebClient webClient) : base(webClient, "WebNovel", "https://www.webnovel.com/", "WebNovel.png"
            , false, repositoryType: RepositoryType.LightNovel)
        {
        }
        public override async Task<IReadOnlyList<ISeries>> GetSeriesAsync(CancellationToken token)
        {
            // initial request
            var html = await WebClient.GetStringAsync(RepoIndexUri, RootUri, token);
            if (html == null)
            {
                return null;
            }

            var parser = new HtmlParser();
            var document = parser.Parse(html);
            //var lastPage = document.QuerySelectorAll("div.digg_pagination>a")?.Reverse()?.Skip(1)?.Take(1)?.FirstOrDefault()?.Text()?.Trim();
            //var result = new List<Series>();
            //if (string.IsNullOrWhiteSpace(lastPage)) return result;
            //if (!TryParse(lastPage, out var lastPageN)) return result;
            //for (var x = 1; x < 2; x++)
            ////TODO remove comment
            ////for (int x = 0; x < lastPageN; x++)
            //{
            //    html = await WebClient.GetStringAsync(new Uri(RepoIndexUri.AbsoluteUri + "?pg=" + x), RootUri, token);
            //    if (html == null)
            //    {
            //        return null;
            //    }
            //    parser = new HtmlParser();
            //    document = parser.Parse(html);
            //    var nodes = document.QuerySelectorAll("#myTable>tbody>tr");
            //    foreach (var n in nodes)
            //    {
            //        var title = WebUtility.HtmlDecode(n.QuerySelectorAll("a").LastOrDefault().Text().Trim());
            //        var href = n.QuerySelector("a").Attributes["href"].Value;
            //        var series = new Series(this, new Uri(RootUri, href),
            //                title)
            //        {
            //            CoverImageUri = new Uri(n.QuerySelector("img").Attributes["src"].Value),
            //            Genres = n.QuerySelectorAll("span.gennew")?.Select(d => d.Text())?.ToArray()
            //        };

            //        result.Add(series);
            //    }
            //}
            //return result;
            return null;
        }

        public override async Task<IReadOnlyList<IChapter>> GetChaptersAsync(ISeries input, CancellationToken token)
        {
            // initial request
            var html = await WebClient.GetStringAsync(input.SeriesPageUri, RepoIndexUri, token);
            if (html == null)
            {
                return null;
            }

            var parser = new HtmlParser();
            var document = parser.Parse(html);
            var lastPage = document.QuerySelectorAll("div.digg_pagination>a")?.Reverse()?.Skip(1)?.Take(1)?.FirstOrDefault()?.Text()?.Trim();
            var result = new List<Chapter>();
            if (string.IsNullOrWhiteSpace(lastPage)) return result;
            if (!TryParse(lastPage, out var lastPageN)) return result;
            for (var x = 0; x < lastPageN; x++)
            {
                html = await WebClient.GetStringAsync(new Uri(input.SeriesPageUri + "?pg=" + x), RootUri, token);
                if (html == null)
                {
                    return null;
                }
                parser = new HtmlParser();
                document = parser.Parse(html);
                var nodes = document.QuerySelectorAll("#myTable>tbody>tr");
                foreach (var n in nodes)
                {
                    //var title = WebUtility.HtmlDecode(n.QuerySelectorAll("a").LastOrDefault().Text().Trim());
                    var date = n.QuerySelector("td:nth-child(1)")?.Text();
                    var href = n.QuerySelectorAll("a").LastOrDefault()?.Attributes["href"].Value;
                    var chapter = new Chapter((Series)input, new Uri(RootUri, href),
                        null)
                    { Updated = date };
                    result.Add(chapter);
                }
            }
            return result;
        }

        public override async Task<IReadOnlyList<IPage>> GetPagesAsync(IChapter input, CancellationToken token)
        {
            //Only 1 page per chapter
            var page = new Page((Chapter)input, input.FirstPageUri, 1);
            return new Page[] { page };
        }

        public override async Task<byte[]> GetPageContentAsync(IPage input, CancellationToken token)
        {
            if (RepositoryType == RepositoryType.LightNovel)
                return await GetPageTextAsync(input, token);
            return await GetPageImageAsync(input, token);
        }

        public override async Task<byte[]> GetPageTextAsync(IPage input, CancellationToken token)
        {
            var html = await WebClient.GetStringAsync(input.PageUri, input.GetParentChapter().FirstPageUri, token);
            if (html == null)
            {
                return null;
            }

            var parser = new HtmlParser();
            var document = parser.Parse(html);

            var chapterDiv = document.QuerySelector("div.chapter-content3");
            var subtitle = chapterDiv.QuerySelector("h1");
            subtitle?.Remove();
            chapterDiv.RemoveAll("center");
            chapterDiv.RemoveAll("div");
            chapterDiv.RemoveAll("script");
            chapterDiv.RemoveAll("iframe");
            if (!string.IsNullOrWhiteSpace(subtitle?.Text()?.Trim()))
            {
                input.GetParentChapter().Subtitle = subtitle.Text().Trim();
            }


            var content = chapterDiv.Text().Trim();
            // update pageContent
            input.PageContent = content;
            return Encoding.UTF8.GetBytes(content);
        }

        public override Task<byte[]> GetPageImageAsync(IPage input, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
