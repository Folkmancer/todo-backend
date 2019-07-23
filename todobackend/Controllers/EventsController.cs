using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todobackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace todobackend.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/events")]
    public class EventsController : ControllerBase
    {
        private readonly DataBaseContext dataBaseContext;
#if DEBUG
        public EventsController(DataBaseContext context)
        {
            dataBaseContext = context;
            if (dataBaseContext.Events.Count() == 0)
            {
                DateTimeFormatInfo ru = new CultureInfo("ru-ru").DateTimeFormat;
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                var temp = new Event[] {
                    new Event { Id = 1, Description = "Test1", DeadlineDate = System.DateTimeOffset.Parse("22.06.2019", ru), IsComplete = false },
                    new Event { Id = 2, Description = "Test2", DeadlineDate = System.DateTimeOffset.Parse("13.02.2019", ru), IsComplete = false },
                    new Event { Id = 3, Description = "Test3", DeadlineDate = System.DateTimeOffset.Parse("14.06.2019", ru), IsComplete = false },
                    new Event { Id = 4, Description = "Test4", DeadlineDate = System.DateTimeOffset.Parse("03.01.2019", ru), IsComplete = false },
                    new Event { Id = 5, Description = "Test5", DeadlineDate = System.DateTimeOffset.Parse("19.06.2019", ru), IsComplete = false },
                    new Event { Id = 6, Description = "Test6", DeadlineDate = System.DateTimeOffset.Parse("01.06.2019", ru), IsComplete = false },
                    new Event { Id = 7, Description = "Test7", DeadlineDate = System.DateTimeOffset.Parse("02.06.2019", ru), IsComplete = false },
                    new Event { Id = 8, Description = "Test8", DeadlineDate = System.DateTimeOffset.Parse("06.09.2019", ru), IsComplete = false },
                    new Event { Id = 9, Description = "Test9", DeadlineDate = System.DateTimeOffset.Parse("07.11.2019", ru), IsComplete = false },
                    new Event { Id = 10, Description = "Test10", DeadlineDate = System.DateTimeOffset.Parse("01.11.2019", ru), IsComplete = false },
                    new Event { Id = 11, Description = "Test11", DeadlineDate = System.DateTimeOffset.Parse("11.06.2019", ru), IsComplete = false },
                    new Event { Id = 12, Description = "Test12", DeadlineDate = System.DateTimeOffset.Parse("18.12.2019", ru), IsComplete = false },
                    new Event { Id = 13, Description = "Test13", DeadlineDate = System.DateTimeOffset.Parse("16.06.2019", ru), IsComplete = false },
                    new Event { Id = 14, Description = "Test14", DeadlineDate = System.DateTimeOffset.Parse("15.06.2019", ru), IsComplete = false },
                    new Event { Id = 15, Description = "Test15", DeadlineDate = System.DateTimeOffset.Parse("04.12.2019", ru), IsComplete = false }
                };
                dataBaseContext.Events.AddRange(temp);
                dataBaseContext.SaveChanges();
            }
        }
#endif      
        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>All events</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents(int? page = null, int? size = 5)
        {
            if (dataBaseContext.Events.Count() == 0)
            {
                return NotFound();
            }
            if (!page.HasValue)
            {
                return await dataBaseContext.Events.ToListAsync();
            }
            return await dataBaseContext.Events.Skip(((int)page - 1) * (int)size).Take((int)size).ToListAsync();
        }

        /// <summary>
        /// Gets a specific event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the event with the specified id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Event>> GetById(long id)
        {
            var eventItem = await dataBaseContext.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return eventItem;
        }

        /// <summary>
        /// Creates a event.
        /// </summary>
        /// <param name="eventItem"></param>
        /// <returns>A newly created event</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        [HttpPost]
        [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> Create(Event eventItem)
        {
            if (await dataBaseContext.Events.AnyAsync(x => x.Id == eventItem.Id))
            {
                return BadRequest();
            }
            //await dataBaseContext.Events.AddAsync(eventItem);
            dataBaseContext.Events.Add(eventItem);
            await dataBaseContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = eventItem.Id }, eventItem);
        }

        /// <summary>
        /// Updates a specific event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eventItem"></param>
        /// <returns>Status code</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateById(long id, Event eventItem)
        {
            if (dataBaseContext.Events.Count() == 0)
            {
                return NoContent();
            }
            if (id != eventItem.Id)
            {
                return BadRequest();
            }
            dataBaseContext.Entry(eventItem).State = EntityState.Modified;
            await dataBaseContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Deletes a specific event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(long id)
        {
            if (dataBaseContext.Events.Count() == 0)
            {
                return NoContent();
            }
            var eventItem = await dataBaseContext.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            dataBaseContext.Events.Remove(eventItem);
            await dataBaseContext.SaveChangesAsync();
            return Ok();
        }
    }
}
