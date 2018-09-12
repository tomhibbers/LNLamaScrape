using LNLamaScrape.Tools;

namespace LNLamaScrape.Repository
{
    public static class Repositories
    {
        public static IRepository ReadLightNovel { get; }
        public static IRepository MangaHere { get; }
        public static IRepository[] AllRepositories { get; }
        static Repositories()
        {
            var client = new WebClient();
            ReadLightNovel = new ReadLightNovelRepository(client);
            MangaHere = new ReadLightNovelRepository(client);
            AllRepositories = new[] { ReadLightNovel, MangaHere };
        }
    }
}
