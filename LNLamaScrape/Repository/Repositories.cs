using LNLamaScrape.DB;
using LNLamaScrape.Tools;

namespace LNLamaScrape.Repository
{
    public static class Repositories
    {
        public static IRepository ReadLightNovel { get; }
        public static IRepository MangaHere { get; }
        public static IRepository[] AllRepositories { get; }
        private static DbConfig DbConfig { get; set; }
        static Repositories()
        {
            var client = new WebClient();
            ReadLightNovel = new ReadLightNovelRepository(client);
            MangaHere = new ReadLightNovelRepository(client);
            AllRepositories = new[] { ReadLightNovel, MangaHere };
        }

        public static void SetDbConfig(string connectionString = null, string dbName = null, string repositoryName = null)
        {
            DbConfig = new DbConfig(connectionString, dbName, repositoryName);
        }
    }
}
