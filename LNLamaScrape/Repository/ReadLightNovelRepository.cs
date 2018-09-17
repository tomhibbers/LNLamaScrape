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
using WebClient = LNLamaScrape.Tools.WebClient;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LNLamaScrape.Tests")]

namespace LNLamaScrape.Repository
{
    internal class ReadLightNovelRepository : RepositoryBase
    {
        public static readonly RepositoryType RepositoryType = RepositoryType.LightNovel;
        private static readonly Uri RepoIndexUri = new Uri("http://www.readlightnovel.org/novel-list");

        public ReadLightNovelRepository(WebClient webClient) : base(webClient, "Read Light Novel", "http://www.readlightnovel.org/", "ReadLightNovel.png"
            , false, repositoryType: RepositoryType.LightNovel)
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
            var nodes = document.QuerySelectorAll("div.list-by-word-body>ul>li>a");

            var output = nodes.Select(d => new Series(this, new Uri(RootUri, d.Attributes["href"].Value), WebUtility.HtmlDecode(d.Text()))).OrderBy(d => d.Title);

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

            //update series
            var items = document.QuerySelectorAll("div.novel-detail-item");
            string description = string.Empty;
            string[] titlesAlternative = null;
            string[] genres = null;
            foreach (var item in items)
            {
                var header = item.QuerySelector("h6")?.Text();
                if (header == "Description")
                {
                    description = item.QuerySelector("div.novel-detail-body")?.Text();
                }
                else if (header == "Alternative Names")
                {
                    titlesAlternative = item.QuerySelectorAll("li")?.Select(d => d.Text())?.ToArray();
                }
                else if (header == "Genre")
                {
                    genres = item.QuerySelectorAll("li")?.Select(d => d.Text())?.ToArray();
                }
            }

            input.Description = description;
            input.Genres = genres;
            input.TitlesAlternative = titlesAlternative;

            //get cover
            var coverItem = document.QuerySelector("div.novel-cover>a>img");
            if (!string.IsNullOrWhiteSpace(coverItem.Attributes["src"].Value))
            {
                input.CoverImageUri = new Uri(coverItem.Attributes["src"].Value);
            }

            //get chapters
            var nodes = document.QuerySelectorAll("ul.chapter-chs>li>a");

            var output = nodes.Select(d => new Chapter((Series)input, new Uri(RootUri, d.Attributes["href"].Value), WebUtility.HtmlDecode(d.Text()))).OrderBy(d => d.Title);
            return output.ToArray();
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

            string description = string.Empty;
            var chapterDiv = document.QuerySelector("div.chapter-content3");
            description = chapterDiv.QuerySelector("h1")?.Text();
            var paras = chapterDiv.QuerySelectorAll("p");
            var content = new StringBuilder();
            foreach (var para in paras)
            {
                var text = para.Text();
                if (!text.StartsWith("Posted on"))
                    content.AppendLine(text);
            }
            return Encoding.UTF8.GetBytes(content.ToString());
        }

        public override Task<byte[]> GetPageImageAsync(IPage input, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
