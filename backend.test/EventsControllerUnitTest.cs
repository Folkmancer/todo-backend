using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace backend.test
{
    [TestClass]
    public class EventsControllerUnitTest
    {  
        [TestMethod]
        public async Task TestPostMethod()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            Guid id = Guid.NewGuid();
            var eventTest = new Event { Id = id, Description = "Test1", DeadlineDate = System.DateTimeOffset.Parse("12.06.2019"), IsComplete = false };

            using (var context = new DataBaseContext(options))
            {
                var service = new EventsController(context);
                await service.Create(new EventView(eventTest), new ApiVersion(1, 0));
                context.SaveChanges();
            }
            using (var context = new DataBaseContext(options))
            {
                Assert.AreEqual(1, context.Events.Count());
            }
        }
    }
}
