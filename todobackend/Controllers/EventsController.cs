using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace todobackend.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/events")]
    public class EventsController : ControllerBase
    {
        // GET: events
        [HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<string>> Get()
        {
            return new string[] { "events1", "events2" };
        }

        // GET: events/{id}
        [HttpGet("{id}", Name = "Get")]
        public async Task<string> Get(int id)
        {
            return "events1";
        }

        // POST: events
        [HttpPost]
        public async Task Post([FromBody] string value)
        {
        }

        // PUT: events/{id}
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
        }

        // DELETE: events/{id}
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
        }
    }
}
