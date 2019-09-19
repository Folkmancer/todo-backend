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
            var eventTest = new NewEvent { Description = "Test1", DeadlineDate = 1560286800, IsComplete = false };

            using (var context = new DataBaseContext(options))
            {
                var service = new EventsController(context);
                await service.Create(eventTest, new ApiVersion(1, 0));
                context.SaveChanges();
            }

            using (var context = new DataBaseContext(options))
            {
                Assert.AreEqual(1, context.Events.Count());
            }
        }
    }
}
