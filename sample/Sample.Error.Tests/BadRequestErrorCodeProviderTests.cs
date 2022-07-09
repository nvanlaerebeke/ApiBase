using NUnit.Framework;

namespace Sample.Error.Tests
{
    [TestFixture]
    internal class BadRequestErrorCodeProviderTests
    {
        [Test]
        public void GetByCode()
        {
            //Arrange
            //Act
            var obj = new BadRequestErrorCodeProvider();
            var r = obj.GetCode("Summary");

            //Assert
            Assert.AreEqual("InvalidSummary", r);
        }

        [Test]
        public void GetByCodeUnknownProperty()
        {
            //Arrange
            //Act
            var obj = new BadRequestErrorCodeProvider();
            var r = obj.GetCode("Evil");

            //Assert
            Assert.AreEqual("UnknownError", r);
        }

        [Test]
        public void GetMessageForCode()
        {
            //Arrange
            //Act
            var obj = new BadRequestErrorCodeProvider();
            var r = obj.GetMessageForCode("InvalidSummary");

            //Assert
            Assert.AreEqual("Invalid Summary", r);
        }

        [Test]
        public void GetMessageForUnknownCode()
        {
            //Arrange
            //Act
            var obj = new BadRequestErrorCodeProvider();
            var r = obj.GetMessageForCode("EvilCode");

            //Assert
            Assert.AreEqual("Invalid value", r);
        }
    }
}
