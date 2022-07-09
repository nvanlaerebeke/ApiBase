using NUnit.Framework;
using ApiBase.Controller.Response;
using ApiBase.Object;

namespace ApiBase.Tests.Controller.Response
{
    public class ResponseTests
    {
        [Test]
        public void TestCreate()
        {
            //Arrange
            var data = new MyTestClass() { ID = "1" };

            //Act
            var obj = new Response<MyTestClass>(data);

            //Assert
            Assert.AreEqual(data, obj.Data);
            Assert.AreEqual("1", obj.Data.ID);
            Assert.AreEqual(obj.Data.GetID(), "1");
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
