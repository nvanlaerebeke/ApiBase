using System.Net;
using NUnit.Framework;

namespace Sample.Error.Tests
{
    [TestFixture]
    internal class SampleExceptionTest
    {
        [Test]
        public void Test()
        {
            //Arrange
            //Act
            var e = new SampleException(new ApiError(ApiErrorCode.Critical));

            //Assert
            Assert.NotNull(e.GetError());
            Assert.AreEqual("Critical", e.GetError().Code.ToString());
            Assert.AreEqual("this is bad", e.GetError().Message);
            Assert.AreEqual(HttpStatusCode.InternalServerError, e.GetError().HttpStatusCode);
        }
    }
}
