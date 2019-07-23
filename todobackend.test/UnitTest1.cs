using todobackend.Models;
using todobackend.Controllers;
using todobackend;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace todobackend.test
{

    public class EventsControllerUnitTest
    {
        public void Seed(DataBaseContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Events.AddRange(GetTestEvents());
            context.SaveChanges();
        }

        private IEnumerable<Event> GetTestEvents()
        {
            var events = new List<Event>();
            var temp = new Event[] {
                    new Event { Id = 1, Description = "Test1", DeadlineDate = System.DateTimeOffset.Parse("12.06.2019"), IsComplete = false },
                    new Event { Id = 2, Description = "Test2", DeadlineDate = System.DateTimeOffset.Parse("13.02.2019"), IsComplete = false },
                    new Event { Id = 3, Description = "Test3", DeadlineDate = System.DateTimeOffset.Parse("14.06.2019"), IsComplete = false },
                    new Event { Id = 4, Description = "Test4", DeadlineDate = System.DateTimeOffset.Parse("3.01.2019"), IsComplete = false },
                    new Event { Id = 5, Description = "Test5", DeadlineDate = System.DateTimeOffset.Parse("19.06.2019"), IsComplete = false },
                    new Event { Id = 6, Description = "Test6", DeadlineDate = System.DateTimeOffset.Parse("1.06.2019"), IsComplete = false },
                    new Event { Id = 7, Description = "Test7", DeadlineDate = System.DateTimeOffset.Parse("2.06.2019"), IsComplete = false },
                    new Event { Id = 8, Description = "Test8", DeadlineDate = System.DateTimeOffset.Parse("6.09.2019"), IsComplete = false },
                    new Event { Id = 9, Description = "Test9", DeadlineDate = System.DateTimeOffset.Parse("7.11.2019"), IsComplete = false },
                    new Event { Id = 10, Description = "Test10", DeadlineDate = System.DateTimeOffset.Parse("1.11.2019"), IsComplete = false },
                    new Event { Id = 11, Description = "Test11", DeadlineDate = System.DateTimeOffset.Parse("11.06.2019"), IsComplete = false },
                    new Event { Id = 12, Description = "Test12", DeadlineDate = System.DateTimeOffset.Parse("18.12.2019"), IsComplete = false },
                    new Event { Id = 13, Description = "Test13", DeadlineDate = System.DateTimeOffset.Parse("16.06.2019"), IsComplete = false },
                    new Event { Id = 14, Description = "Test14", DeadlineDate = System.DateTimeOffset.Parse("15.06.2019"), IsComplete = false },
                    new Event { Id = 15, Description = "Test15", DeadlineDate = System.DateTimeOffset.Parse("4.12.2019"), IsComplete = false }
                };
            events.AddRange(temp);
            return events;
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var data = GetTestEvents().AsQueryable();
            var mockSet = new Mock<DbSet<Event>>();
            mockSet.As<IQueryable<Event>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Event>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Event>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Event>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockContext = new Mock<DataBaseContext>();
            mockContext.Setup(c => c.Events).Returns(mockSet.Object);
            var controller = new EventsController(mockContext.Object);

            // Act
            var result = controller.GetAllEvents();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            /*
            var mockSet = new Mock<DbSet<Event>>();

            var mockContext = new Mock<DataBaseContext>();
            mockContext.Setup(m => m.Events).Returns(mockSet.Object);

            var service = new BlogService(mockContext.Object);
            service.AddBlog("ADO.NET Blog", "http://blogs.msdn.com/adonet");

            mockSet.Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());*/
        }
    }
}
