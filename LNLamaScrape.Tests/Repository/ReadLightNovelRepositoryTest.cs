using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Repository;
using LNLamaScrape.Tools;
using Xunit;


namespace LNLamaScrape.Tests.Repository
{
    public class ReadLightNovelRepositoryTest
    {
        private CancellationTokenSource _cts { get; }
        private readonly ReadLightNovelRepository _repo;

        public ReadLightNovelRepositoryTest()
        {
            _repo = new ReadLightNovelRepository(new Tools.WebClient());
            _cts = new CancellationTokenSource();
        }

        [Fact]
        public async Task TestGetSeriesAsync()
        {
            //Arrange

            //Act
            var series = await _repo.GetSeriesAsync(_cts.Token);

            //Assert
            Assert.NotNull(series);

            foreach (var s in series)
            {
                Assert.False(string.IsNullOrWhiteSpace(s.Title));
                Assert.NotNull(s.SeriesPageUri);
            }
        }
        [Fact]
        public async Task TestGetChaptersAsync()
        {
            //Arrange
            var series = await _repo.GetSeriesAsync(_cts.Token);
            var selectedSeries = series.FirstOrDefault();

            //Act
            var chapters = await _repo.GetChaptersAsync(selectedSeries, _cts.Token);

            //Assert
            Assert.NotNull(chapters);

            foreach (var i in chapters)
            {
                Assert.False(string.IsNullOrWhiteSpace(i.Title));
                Assert.NotNull(i.FirstPageUri);
            }

        }
        [Fact]
        public async Task TestGetPagesAsync()
        {
            var series = await _repo.GetSeriesAsync(_cts.Token);
            var selectedSeries = series.FirstOrDefault();
            Assert.NotNull(selectedSeries);
            var chapters = await selectedSeries.GetChaptersAsync();
            var selectedChapter = chapters.FirstOrDefault();
            Assert.NotNull(selectedChapter);
            var pages = await _repo.GetChaptersAsync(selectedSeries, _cts.Token);
            Assert.NotNull(pages);

            foreach (var i in pages)
            {
                Assert.False(string.IsNullOrWhiteSpace(i.Title));
                Assert.NotNull(i.FirstPageUri);
            }

        }
    }
}
