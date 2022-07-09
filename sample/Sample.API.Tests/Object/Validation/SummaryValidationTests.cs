using NUnit.Framework;
using Sample.API.Object.Validation;

namespace Sample.API.Tests.Object.Validation
{
    [TestFixture]
    internal class SummaryValidationTests
    {
        [Test]
        public void Valid()
        {
            //Arrange
            //Act
            var obj = new SummaryValidation();
            var r = obj.IsValid("Warm");

            //Assert
            Assert.IsTrue(r);
        }

        [Test]
        public void InValidString()
        {
            //Arrange
            //Act
            var obj = new SummaryValidation();
            var r = obj.IsValid("Invalid");

            //Assert
            Assert.IsFalse(r);
        }

        [Test]
        public void FailedCheck()
        {
            //Arrange
            //Act
            var obj = new SummaryValidation();
            var r = obj.IsValid("Freezing");

            //Assert
            Assert.IsFalse(r);
        }

        [Test]
        public void InvalidNotAString()
        {
            //Arrange
            //Act
            var obj = new SummaryValidation();
            var r = obj.IsValid(new object());

            //Assert
            Assert.IsFalse(r);
        }

        [Test]
        public void GetMessage()
        {
            //Arrange
            //Act
            var obj = new SummaryValidation();
            _ = obj.IsValid("Freezing");
            var r = obj.FormatErrorMessage("Summary");

            //Assert
            Assert.AreEqual("I will not allow Freezing for Summary!", r);
        }
    }
}
