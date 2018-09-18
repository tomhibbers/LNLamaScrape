using System;
using System.IO;
using System.Linq;
using LNLamaScrape.Repository;
using Microsoft.Extensions.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Examples.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            var series = Repositories.WebNovel.GetSeriesAsync();
            series.Wait();
            var seriesList = series.Result.Take(3).ToList();
            foreach (var s in seriesList)
            {
                var chapters = s.GetChaptersAsync();
                chapters.Wait();
                var chaptersList = chapters.Result.Take(3).ToList();
                if (chaptersList.Count == 0)
                    continue;

                foreach (var c in chaptersList)
                {
                    var pages = c.GetPagesAsync();
                    pages.Wait();
                    var pagesList = pages.Result.ToList();
                    if (pagesList.Count == 0)
                        continue;
                    foreach (var p in pagesList)
                    {
                        var pagesWithContent = p.GetPageContentAsync();
                        pagesWithContent.Wait();
                    }
                }
            }
        }
    }
}
