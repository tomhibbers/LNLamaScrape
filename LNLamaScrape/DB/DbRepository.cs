using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LNLamaScrape.Models;
using LNLamaScrape.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LNLamaScrape.DB
{
    public class DbRepository
    {
        public async Task UpdateDbSeriesAsync(DbConfig dbConfig, IReadOnlyList<ISeries> series)
        {
            dbConfig.Init();
            var seriesDocuments = series.Select(d => new BsonDocument()
            {
                {"title", new BsonString(d.Title) },
                {"seriesPageUri", new BsonString(d.SeriesPageUri.AbsoluteUri) },
            });
            // create collection if doesn't exist
            if (!dbConfig.Database.ListCollectionNames().ToList().Any(x => x.Contains("Series")))
            {
                dbConfig.Database.CreateCollection("Series");
            }
            var collection = dbConfig.Database.GetCollection<BsonDocument>("Series");
            var bw = await collection.BulkWriteAsync(
                seriesDocuments.Select(
                    d => new UpdateOneModel<BsonDocument>(
                        new BsonDocument("title", d[0]),
                        new BsonDocument("$set", d))
                    { IsUpsert = true }
                )
            );
        }
        public async Task UpdateDbChaptersAsync(DbConfig dbConfig, IReadOnlyList<IChapter> chapters)
        {
            dbConfig.Init();
            var seriesDocuments = chapters.Select(d => new BsonDocument()
            {
                {"title", new BsonString(d.Title) },
                {"firstPageUri", new BsonString(d.FirstPageUri.AbsoluteUri) },
            });
            // create collection if doesn't exist
            if (!dbConfig.Database.ListCollectionNames().ToList().Any(x => x.Contains("Chapters")))
            {
                dbConfig.Database.CreateCollection("Chapters");
            }
            var collection = dbConfig.Database.GetCollection<BsonDocument>("Chapters");
            var bw = await collection.BulkWriteAsync(
                seriesDocuments.Select(
                    d => new UpdateOneModel<BsonDocument>(
                            new BsonDocument("title", d[0]),
                            new BsonDocument("$set", d))
                    { IsUpsert = true }
                )
            );
        }

        public async Task UpdateDbPagesAsync(DbConfig dbConfig, IReadOnlyList<IPage> pages)
        {
            dbConfig.Init();
            // generate model
            var pagesDocuments = pages.Select(d => new BsonDocument()
            {
                {"pageNo", new BsonString(d.PageNo.ToString()) },
                {"pageUri", new BsonString(d.PageUri.AbsoluteUri) },
            });
            // create collection if doesn't exist
            if (!dbConfig.Database.ListCollectionNames().ToList().Any(x => x.Contains("Pages")))
            {
                dbConfig.Database.CreateCollection("Pages");
            }
            var collection = dbConfig.Database.GetCollection<BsonDocument>("Pages");
            var bw = await collection.BulkWriteAsync(
                pagesDocuments.Select(
                    d => new UpdateOneModel<BsonDocument>(
                            new BsonDocument("pageNo", d[0]),
                            new BsonDocument("$set", d))
                    { IsUpsert = true }
                )
            );
        }
        public async Task UpdateDbPagesWithContentAsync(DbConfig dbConfig, IReadOnlyList<IPage> pages)
        {
            dbConfig.Init();
            // download content for pages
            var taskList = new List<Task>();
            foreach (var i in pages)
            {
                taskList.Add(DownloadPageContent((Page)i));
                taskList.Add(i.GetPageContentAsync());
            }
            Task.WaitAll(taskList.ToArray());
            // generate model
            var pagesDocuments = pages.Select(d => new BsonDocument()
            {
                {"pageNo", new BsonString(d.PageNo.ToString()) },
                {"pageUri", new BsonString(d.PageUri.AbsoluteUri) },
                {"pageContent", new BsonString(d.PageContent) },
            });
            // create collection if doesn't exist
            if (!dbConfig.Database.ListCollectionNames().ToList().Any(x => x.Contains("Pages")))
            {
                dbConfig.Database.CreateCollection("Pages");
            }
            var collection = dbConfig.Database.GetCollection<BsonDocument>("Pages");
            // bulk upsert
            var bw = await collection.BulkWriteAsync(
                pagesDocuments.Select(
                    d => new UpdateOneModel<BsonDocument>(
                            new BsonDocument("pageNo", d[0]),
                            new BsonDocument("$set", d))
                    { IsUpsert = true }
                )
            );
        }

        internal async Task DownloadPageContent(Page page)
        {
            var res = await page.GetPageContentAsync();
            page.PageContent = Encoding.UTF8.GetString(res);
        }
    }
}
