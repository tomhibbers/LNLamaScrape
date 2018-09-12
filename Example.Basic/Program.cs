using LNLamaScrape.DB;
using LNLamaScrape.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;
using LNLamaScrape.Models;
using Microsoft.Extensions.Options;

namespace Example.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            //var series = Repositories.ReadLightNovel.GetSeriesAsync();
            //series.Wait();
            //var chapters = series.Result[0].GetChaptersAsync();
            //chapters.Wait();
            //var pages = chapters.Result[0].GetPagesAsync();
            //pages.Wait();
            //var pagesWithContent = pages.Result[0].GetPageContentAsync();
            //var repodb = new MongoRepository();
            //var res=repodb.UpdateDbPagesWithContentAsync(
            //    new MongoContext(
            //        connectionString: "mongodb://localhost:27017",
            //        dbName: "LNLamaScrape"), pages.Result);
            //res.Wait();
            
            var repodb = new MongoRepository(new Settings()
            {
                Database = "LNLamaScrape",
                ConnectionString = "mongodb://localhost:27017"
            });
            var series = Repositories.ReadLightNovel.GetSeriesAsync();
            series.Wait();
            var seriesList = series.Result.Take(10).ToList();
            var seriesDbRes = repodb.UpdateDbSeriesAsync(
                seriesList);
            seriesDbRes.Wait();
            foreach (var s in seriesList)
            {
                var chapters = s.GetChaptersAsync();
                chapters.Wait();
                var chaptersList = chapters.Result.Take(10).ToList();
                if(chaptersList.Count==0)
                    continue;
                var chaptersDbRes = repodb.UpdateDbChaptersAsync(
                    chaptersList);
                chaptersDbRes.Wait();
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
                    var pagesWithContentDbRes = repodb.UpdateDbPagesWithContentAsync(
                        pagesList);
                    pagesWithContentDbRes.Wait();
                }
            }
            //var chapters = series.Result[0].GetChaptersAsync();
            //chapters.Wait();
            //var pages = chapters.Result[0].GetPagesAsync();
            //pages.Wait();
            //var pagesWithContent = pages.Result[0].GetPageContentAsync();
            //var repodb = new MongoRepository();
            //var res = repodb.UpdateDbPagesWithContentAsync(
            //    new MongoContext(
            //        connectionString: "mongodb://localhost:27017",
            //        dbName: "LNLamaScrape"), pages.Result);
            //res.Wait();
        }
    }
}
