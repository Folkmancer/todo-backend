using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
        public async Task GetAllEvents_Ok_Result()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new DataBaseContext(options);
            var service = new EventsController(context);
            var eventsTest = new[] {
                new Event {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Description = "Test1",
                    DeadlineDate = DateTimeOffset.FromUnixTimeSeconds(1560286800), IsComplete = false
                },
                new Event {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7"), Description = "Test2",
                    DeadlineDate = DateTimeOffset.FromUnixTimeSeconds(1560286800), IsComplete = false
                },
                new Event {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa8"), Description = "Test3",
                    DeadlineDate = DateTimeOffset.FromUnixTimeSeconds(1560286800), IsComplete = false
                }
            };
            context.Events.AddRange(eventsTest);
            context.SaveChanges();

            var result = await service.GetAllEvents();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, context.Events.Count());
        }

        [TestMethod]
        public async Task GetAllEvents_NotFound_Result()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new DataBaseContext(options);
            var service = new EventsController(context);

            var result = await service.GetAllEvents();
            var notFoundResult = result.Result as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task GetById_Ok_Result()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new DataBaseContext(options);
            var service = new EventsController(context);
            var id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var eventTest = new Event
            {
                Id = id,
                Description = "Test1",
                DeadlineDate = DateTimeOffset.FromUnixTimeSeconds(1560286800),
                IsComplete = false
            };
            
            context.Events.Add(eventTest);
            context.SaveChanges();

            var result = await service.GetById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Value.Id);
        }

        [TestMethod]
        public async Task GetById_NotFound_Result()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new DataBaseContext(options);
            var service = new EventsController(context);

            var result = await service.GetById(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
            var notFoundResult = result.Result as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task TestPostMethod()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var eventTest = new NewEvent { Description = "Test1", DeadlineDate = 1560286800, IsComplete = false };
            var context = new DataBaseContext(options);
            var service = new EventsController(context);

            var result = await service.Create(eventTest, new ApiVersion(1, 0));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, context.Events.Count());
        }

        [TestMethod]
        public async Task UpdateById_Ok_Result()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var eventTest = new Event { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Description = "Test1", DeadlineDate = DateTimeOffset.FromUnixTimeSeconds(1560286800), IsComplete = false };
            var updateEventTest = new UpdateEvent { Description = "UpdateTest1", DeadlineDate = 1560286800, IsComplete = false };
            var context = new DataBaseContext(options);
            var service = new EventsController(context);
            context.Events.Add(eventTest);

            var result = await service.UpdateById(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), updateEventTest);
            var okResult = result as OkResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateById_NotFound_Result()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options; ;
            var updateEventTest = new UpdateEvent { Description = "UpdateTest1", DeadlineDate = 1560286800, IsComplete = false };
            var context = new DataBaseContext(options);
            var service = new EventsController(context);

            var result = await service.UpdateById(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), updateEventTest);
            var notFoundResult = result as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteById_Ok_Result()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var eventTest = new Event { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Description = "Test1", DeadlineDate = DateTimeOffset.FromUnixTimeSeconds(1560286800), IsComplete = false };
            var context = new DataBaseContext(options);
            var service = new EventsController(context);
            context.Events.Add(eventTest);

            var result = await service.DeleteById(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
            var okResult = result as OkResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteById_NotFound_Result()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;;
            var context = new DataBaseContext(options);
            var service = new EventsController(context);

            var result = await service.DeleteById(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
            var notFoundResult = result as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}
