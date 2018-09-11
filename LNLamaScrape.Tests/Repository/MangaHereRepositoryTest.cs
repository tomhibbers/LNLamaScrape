using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LNLamaScrape.Models;
using LNLamaScrape.Repository;
using Xunit;

namespace LNLamaScrape.Tests.Repository
{
    public class MangaHereRepositoryTest
    {
        private CancellationTokenSource _cts { get; }
        private readonly MangaHereRepository _repo;

        public MangaHereRepositoryTest()
        {
            _repo = new MangaHereRepository(new Tools.WebClient());
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
            var pages = await _repo.GetPagesAsync(selectedChapter, _cts.Token);
            Assert.NotNull(pages);

            foreach (var i in pages)
            {
                Assert.NotNull(i.PageUri);
            }

        }
        [Fact]
        public async Task TestGetPageContentAsync()
        {
            var series = await _repo.GetSeriesAsync(_cts.Token);
            var selectedSeries = series.FirstOrDefault();
            Assert.NotNull(selectedSeries);
            var chapters = await selectedSeries.GetChaptersAsync();
            var selectedChapter = chapters.FirstOrDefault();
            Assert.NotNull(selectedChapter);
            var pages = await _repo.GetPagesAsync(selectedChapter, _cts.Token);
            Assert.NotNull(pages);
            foreach (var i in pages)
            {
                Assert.NotNull(i.PageUri);
                var content = await i.GetPageContentAsync();
                Assert.NotNull(content);
                Assert.IsType<byte[]>(content);
            }

        }
        [Fact]
        public async Task TestGetPageImageAsync()
        {
            var series = await _repo.GetSeriesAsync(_cts.Token);
            var selectedSeries = series.FirstOrDefault();
            Assert.NotNull(selectedSeries);
            var chapters = await selectedSeries.GetChaptersAsync();
            var selectedChapter = chapters.FirstOrDefault();
            Assert.NotNull(selectedChapter);
            var pages = await _repo.GetPagesAsync(selectedChapter, _cts.Token);
            Assert.NotNull(pages);
            foreach (var i in pages.Select(d => (Page)d))
            {
                Assert.NotNull(i.PageUri);
                var content = await i.GetPageImageAsync();
                Assert.NotNull(content);
                Assert.IsType<byte[]>(content);
                var contentString = Encoding.UTF8.GetString(content);
                Assert.False(string.IsNullOrWhiteSpace(contentString));
            }

        }
    }
}
