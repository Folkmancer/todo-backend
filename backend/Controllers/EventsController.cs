using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<EventProxy>>> GetAllEvents(int? page = null, int? size = 5)
        {
            if (dataBaseContext.Events.Count() == 0)
            {
                return NotFound();
            }
            if (!page.HasValue)
            {
                return await dataBaseContext.Events.Select(x => new EventProxy(x)).ToListAsync();
            }
            return await dataBaseContext.Events.Skip(((int)page - 1) * (int)size).Take((int)size).Select(x => new EventProxy(x)).ToListAsync();
        }

        /// <summary>
        /// Gets a specific event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the event with the specified id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventProxy), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventProxy>> GetById(long id)
        {
            var eventItem = await dataBaseContext.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return new EventProxy(eventItem);
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
        [ProducesResponseType(typeof(EventProxy), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventProxy>> Create([FromBody] EventProxy eventItem, ApiVersion apiVersion)
        {
            if (await dataBaseContext.Events.AnyAsync(x => x.Id == eventItem.Id))
            {
                return BadRequest();
            }
            //await dataBaseContext.Events.AddAsync(eventItem);
            dataBaseContext.Events.Add(new Event(eventItem));
            await dataBaseContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = eventItem.Id, version = apiVersion.ToString() }, eventItem);
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
        public async Task<IActionResult> UpdateById(long id, EventProxy eventItem)
        {
            if (dataBaseContext.Events.Count() == 0)
            {
                return NoContent();
            }
            if (id != eventItem.Id)
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
