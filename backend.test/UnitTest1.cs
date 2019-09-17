using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace backend.test
{
    [TestClass]
    public class EventsControllerUnitTest
    {  
        private IQueryable<Event> GetTestEvents()
        {
            var culture = CultureInfo.CreateSpecificCulture("ru-RU");
            Thread.CurrentThread.CurrentCulture = culture;
            return new Event[] {
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
                }.AsQueryable();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new DataBaseContext(options))
            {
                var service = new EventsController(context);
                await service.Create(new EventView(GetTestEvents().First()), new ApiVersion(1, 0));
                context.SaveChanges();
            }

            using (var context = new DataBaseContext(options))
            {
                Assert.AreEqual(1, context.Events.Count());
                Assert.AreEqual(GetTestEvents().First().Id, context.Events.First().Id);
            }
        }
    }
}
