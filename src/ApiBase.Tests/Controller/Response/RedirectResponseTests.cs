using System;
using Moq;
using NUnit.Framework;
using ApiBase.Controller;
using ApiBase.Controller.Response;
using ApiBase.Object;

namespace ApiBase.Tests.Controller.Response
{
    public class RedirectResponseTests
    {
        [Test]
        public void TestCreate()
        {
            //Arrange
            var data = new MyTestClass() { ID = "1" };
            var controller = new Mock<IAPIController>();

            _ = controller.Setup(x => x.GetBaseUri()).Returns(new Uri("https://example.com"));

            //Act
            var obj = new RedirectResponse<MyTestClass>(controller.Object, data);

            //Assert
            Assert.AreEqual("1", obj.GetID());
            //Moq creates a class named IAPIControllerProxy, so the target endpoints in this case would be IAPIProxy
            Assert.AreEqual("https://example.com/IAPIProxy/1", obj.Uri.ToString());
        }

        private class MyTestClass : IAPIObject
        {
            public string ID { get; set; }

            public string GetID()
            {
                return ID;
            }
        }
    }
}
