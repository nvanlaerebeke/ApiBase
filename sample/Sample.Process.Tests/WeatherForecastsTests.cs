using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using ApiBase.Filter.Pagination;

namespace Sample.Process.Tests
{
    [TestFixture]
    internal class WeatherForecastsTests
    {
        [Test]
        public async Task GetAllForecasts()
        {
            //Arrange
            //Act
            var obj = new WeatherForecasts();
            var r = await obj.GetAsync().ConfigureAwait(false);

            //Assert
            Assert.That(r.Data.Count().Equals(100));
            Assert.AreEqual(null, r.PaginationFilter);
            Assert.AreEqual(100, r.TotalRecords);
        }

        [Test]
        public async Task GetPage()
        {
            //Arrange
            //Act
            var obj = new WeatherForecasts();
            var all = await obj.GetAsync().ConfigureAwait(false);
            var r = await obj.GetAsync(new PaginationFilter() { PageNumber = 3, PageSize = 2 }).ConfigureAwait(false);

            //Assert
            Assert.That(r.Data.Count().Equals(2));
            Assert.AreEqual(3, r.PaginationFilter.PageNumber);
            Assert.AreEqual(2, r.PaginationFilter.PageSize);
            Assert.AreEqual(100, r.TotalRecords);
            Assert.AreEqual(all.Data.ElementAt(6).ID, r.Data.ElementAt(0).ID);
            Assert.AreEqual(all.Data.ElementAt(7).ID, r.Data.ElementAt(1).ID);
        }
    }
}
