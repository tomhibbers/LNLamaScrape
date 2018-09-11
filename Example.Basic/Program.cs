using LNLamaScrape.DB;
using LNLamaScrape.Repository;
using System;

namespace Example.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            var series = Repositories.ReadLightNovel.GetSeriesAsync();
            series.Wait();
            var chapters = series.Result[0].GetChaptersAsync();
            chapters.Wait();
            var pages = chapters.Result[0].GetPagesAsync();
            pages.Wait();
            var pagesWithContent = pages.Result[0].GetPageContentAsync();
            var repodb = new DbRepository();
            var res=repodb.UpdateDbPagesWithContentAsync(
                new DbConfig(
                    connectionString: "mongodb://localhost:27017",
                    dbName: "LNLamaScrape"), pages.Result);
            res.Wait();
        }
    }
}
