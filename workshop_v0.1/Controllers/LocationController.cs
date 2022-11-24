using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.Models;

namespace workshop_v0._1.DAL
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class LocationController : ControllerBase
    {
        LocationContext _context;
        public LocationController(LocationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> Get()
        {
            return await _context.Location.ToListAsync();
        }

        //api/Locations/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Location>>> Get(int id)
        {
            Location location = await _context.Location.FirstOrDefaultAsync(x => x.id_location == id);
            if (location == null)
                return NotFound("Location doesn't exist");
            return new ObjectResult(location);
        }

        [HttpPost]
        public async Task<ActionResult<Location>> Post(Location Location)
        {
            if (Location == null)
                return BadRequest("Can't add an empty Location");

            _context.Location.Add(Location);
            await _context.SaveChangesAsync();
            return Ok(Location);

        }

        [HttpPut]
        public async Task<ActionResult<Location>> Put(Location location)
        {
            if (location == null)
                return BadRequest();

            if (!_context.Location.Any(x => x.id_location == location.id_location))
                return NotFound();

            _context.Update(location);
            await _context.SaveChangesAsync();
            return Ok(location);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Location>> Delete(int id)
        {
            Location location = await _context.Location.FirstOrDefaultAsync(x => x.id_location == id);
            if (location == null)
                return NotFound();

            _context.Location.Remove(location);
            await _context.SaveChangesAsync();
            return Ok(location);
        }

    }
}
