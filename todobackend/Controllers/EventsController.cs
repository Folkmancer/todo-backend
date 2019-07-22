using System.Linq;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todobackend.Models;
using Microsoft.EntityFrameworkCore;

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
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                var temp = new Event { Id = 1, Description = "Test", DeadlineDate = null, IsComplete = false };
                dataBaseContext.Events.Add(temp);
                temp = new Event { Id = 2, Description = "Test", DeadlineDate = null, IsComplete = false };
                dataBaseContext.Events.Add(temp);
                dataBaseContext.SaveChanges();
            }
        }
#endif      
        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            if (dataBaseContext.Events.Count() == 0)
            {
                return NotFound();
            }
            return await dataBaseContext.Events.ToListAsync();
        }

        /// <summary>
        /// Gets a specific event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Event>> Create(Event eventItem)
        {
            if (await dataBaseContext.Events.AnyAsync(x => x.Id == eventItem.Id))
            {
                return BadRequest();
            }
            await dataBaseContext.Events.AddAsync(eventItem);
            return CreatedAtAction(nameof(GetById), new { id = eventItem.Id }, eventItem);
        }

        /// <summary>
        /// Updates a specific event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eventItem"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateById(long id, Event eventItem)
        {
            if (id != eventItem.Id)
            {
                return BadRequest();
            }
            dataBaseContext.Entry(eventItem).State = EntityState.Modified;
            await dataBaseContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
