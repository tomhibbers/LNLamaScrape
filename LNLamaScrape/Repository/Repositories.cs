using LNLamaScrape.Tools;

namespace LNLamaScrape.Repository
{
    public static class Repositories
    {
        public static IRepository ReadLightNovel { get; }
        public static IRepository OnlineNovelReader { get; }
        public static IRepository NovelUpdates { get; }
        public static IRepository WebNovel { get; }
        public static IRepository MangaHere { get; }
        public static IRepository[] AllRepositories { get; }
        static Repositories()
        {
            var client = new WebClient();
            ReadLightNovel = new ReadLightNovelRepository(client);
            OnlineNovelReader = new OnlineNovelReaderRepository(client);
            NovelUpdates = new NovelUpdatesRepository(client);
            WebNovel = new WebNovelRepository(client);
            MangaHere = new ReadLightNovelRepository(client);
            AllRepositories = new[] { ReadLightNovel, MangaHere };
        }
    }
}
