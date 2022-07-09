using System.Net;
using NUnit.Framework;

namespace Sample.Error.Tests
{
    [TestFixture]
    internal class ApiErrorTest
    {
        [Test]
        public void Default()
        {
            //Arrange
            //Act
            var obj = new ApiError();

            //Assert
            Assert.AreEqual(ApiErrorCode.Critical, obj.Code);
            Assert.AreEqual(HttpStatusCode.InternalServerError, obj.HttpStatusCode);
        }

        [Test]
        public void WithHttpStatusCodeTest()
        {
            //Arrange
            //Act
            var error = new ApiError(ApiErrorCode.Critical, HttpStatusCode.OK);

            //Assert
            Assert.AreEqual("Critical", error.Code.ToString());
            Assert.AreEqual("this is bad", error.Message);
            Assert.AreEqual(HttpStatusCode.OK, error.HttpStatusCode);
        }

        [Test]
        public void WithoutHttpStatusCodeTest()
        {
            //Arrange
            //Act
            var error = new ApiError(ApiErrorCode.Critical);

            //Assert
            Assert.AreEqual("Critical", error.Code.ToString());
            Assert.AreEqual("this is bad", error.Message);
            Assert.AreEqual(HttpStatusCode.InternalServerError, error.HttpStatusCode);
        }
    }
}
