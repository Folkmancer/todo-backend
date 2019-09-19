using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace backend.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/events")]
    public class EventsController : ControllerBase
    {
        private readonly DataBaseContext dataBaseContext;

        public EventsController(DataBaseContext context)
        {
            dataBaseContext = context;
        }
              
        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>All events</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EventView>>> GetAllEvents(int? page = 1, int size = 5)
        {
            if (dataBaseContext.Events.Count() == 0)
            {
                return NotFound();
            }
            return await dataBaseContext.Events
                .OrderByDescending(x => x.DeadlineDate)
                .Skip(((int)page - 1) * (int)size).Take((int)size)
                .Select(x => new EventView(x))
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the event with the specified id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventView>> GetById(Guid id)
        {
            var eventItem = await dataBaseContext.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return new EventView(eventItem);
        }

        /// <summary>
        /// Creates a event.
        /// </summary>
        /// <param name="eventItem"></param>
        /// <param name="apiVersion"></param>
        /// <returns>A newly created event</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        [HttpPost]
        [ProducesResponseType(typeof(EventView), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventView>> Create(NewEvent eventItem, ApiVersion apiVersion)
        {
            if (eventItem == null)
            {
                return BadRequest();
            }
            var result = dataBaseContext.Events.Add(new Event(eventItem));
            await dataBaseContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = result.Entity.Id, version = apiVersion.ToString() }, eventItem);
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
        public async Task<IActionResult> UpdateById(Guid id, NewEvent eventItem)
        {
            if (dataBaseContext.Events.Count() == 0)
            {
                return NoContent();
            }
            if (!dataBaseContext.Events.Any(x => x.Id == id))
            {
                return BadRequest();
            }
            dataBaseContext.Entry(new Event(eventItem)).State = EntityState.Modified;
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
        public async Task<IActionResult> DeleteById(Guid id)
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
