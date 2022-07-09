using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ApiBase.Controller.Response;
using ApiBase.Filter.Pagination;
using ApiBase.Object;

namespace ApiBase.Tests.Controller.Response
{
    public class PageResponseTests
    {
        [Test]
        public void TestCreate()
        {
            //Arrange
            var objects = new List<MyTestClass>() {
                new MyTestClass() { ID = "1" },
                new MyTestClass() { ID = "2" }
            };

            //Act
            var obj = new PageResponse<MyTestClass>(objects);

            //Assert
            Assert.That(obj.Data.ToList().Count.Equals(2));
            Assert.AreEqual(null, obj.Page);
            Assert.AreEqual(null, obj.Size);
            Assert.AreEqual(null, obj.TotalRecords);
            Assert.AreEqual(null, obj.NextPage);
            Assert.AreEqual(null, obj.PreviousPage);
            Assert.That(obj.Data.ElementAt(0).ID.Equals("1"));
            Assert.That(obj.Data.ElementAt(1).ID.Equals("2"));
            Assert.AreEqual(obj.Data.ElementAt(0).GetID(), "1");
            Assert.AreEqual(obj.Data.ElementAt(1).GetID(), "2");
        }

        [Test]
        public void TestCreateWithPagingNoData()
        {
            //Arrange
            var objects = new List<MyTestClass>();
            var paging = new PaginationFilter()
            {
                PageNumber = 1,
                PageSize = 100
            };

            //Act
            var obj = new PageResponse<MyTestClass>(objects, 101, paging);

            //Assert
            Assert.That(obj.Data.ToList().Count.Equals(0));
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(100, obj.Size);
            Assert.AreEqual(101, obj.TotalRecords);
            Assert.AreEqual(null, obj.NextPage);
            Assert.AreEqual(0, obj.PreviousPage);
        }

        [Test]
        public void TestCreateWithPagingLessItemsThenPageSize()
        {
            //Arrange
            var objects = new List<MyTestClass>() {
                new MyTestClass() { ID = "1" },
                new MyTestClass() { ID = "2" }
            };
            var paging = new PaginationFilter()
            {
                PageNumber = 5,
                PageSize = 100
            };

            //Act
            var obj = new PageResponse<MyTestClass>(objects, 2, paging);

            //Assert
            Assert.That(obj.Data.ToList().Count.Equals(2));
            Assert.AreEqual(5, obj.Page);
            Assert.AreEqual(100, obj.Size);
            Assert.AreEqual(2, obj.TotalRecords);
            Assert.AreEqual(null, obj.NextPage);
            Assert.AreEqual(4, obj.PreviousPage);
        }

        [Test]
        public void TestCreateWithPagingFirstPage()
        {
            //Arrange
            var objects = new List<MyTestClass>() {
                new MyTestClass() { ID = "1" },
                new MyTestClass() { ID = "2" }
            };
            var paging = new PaginationFilter()
            {
                PageNumber = 0,
                PageSize = 1
            };

            //Act
            var obj = new PageResponse<MyTestClass>(objects, objects.Count, paging);

            //Assert
            Assert.That(obj.Data.ToList().Count.Equals(2));
            Assert.AreEqual(0, obj.Page);
            Assert.AreEqual(1, obj.Size);
            Assert.AreEqual(2, obj.TotalRecords);
            Assert.AreEqual(1, obj.NextPage);
            Assert.AreEqual(0, obj.PreviousPage);
        }

        [Test]
        public void TestCreateWithPagingLastPage()
        {
            //Arrange
            var objects = new List<MyTestClass>() {
                new MyTestClass() { ID = "1" },
                new MyTestClass() { ID = "2" },
                new MyTestClass() { ID = "3" },
                new MyTestClass() { ID = "4" },
                new MyTestClass() { ID = "5" },
                new MyTestClass() { ID = "6" },
                new MyTestClass() { ID = "7" },
                new MyTestClass() { ID = "8" },
                new MyTestClass() { ID = "9" },
                new MyTestClass() { ID = "10" }
            };

            var paging = new PaginationFilter()
            {
                PageNumber = 1,
                PageSize = 6
            };

            //Act
            var obj = new PageResponse<MyTestClass>(objects.Skip(6), objects.Count, paging);

            //Assert
            Assert.That(obj.Data.ToList().Count.Equals(4));
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(6, obj.Size);
            Assert.AreEqual(10, obj.TotalRecords);
            Assert.AreEqual(null, obj.NextPage);
            Assert.AreEqual(0, obj.PreviousPage);
        }

        [Test]
        public void TestCreateWithPagingNoPage()
        {
            //Arrange
            var objects = new List<MyTestClass>() {
                new MyTestClass() { ID = "1" },
                new MyTestClass() { ID = "2" }
            };

            var paging = new PaginationFilter()
            {
                PageNumber = 0,
                PageSize = -1
            };

            //Act
            var obj = new PageResponse<MyTestClass>(objects, null, paging);

            //Assert
            Assert.That(obj.Data.ToList().Count.Equals(2));
            Assert.AreEqual(null, obj.Page);
            Assert.AreEqual(null, obj.Size);
            Assert.AreEqual(null, obj.TotalRecords);
            Assert.AreEqual(null, obj.NextPage);
            Assert.AreEqual(null, obj.PreviousPage);
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
