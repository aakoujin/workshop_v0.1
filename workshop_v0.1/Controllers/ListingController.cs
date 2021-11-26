using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.DAL;
using workshop_v0._1.Models;

namespace workshop_v0._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        ListingContext _context;
        public ListingController(ListingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> Get()
        {
            return await _context.Listing.ToListAsync();
        }

        //api/offers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Listing>>> Get(int id)
        {
            Listing listing = await _context.Listing.FirstOrDefaultAsync(x => x.id_listing == id);
            if (listing == null)
                return NotFound("Listing doesn't exist");
            return new ObjectResult(listing);
        }

        [HttpPost]
        public async Task<ActionResult<Listing>> Post(Listing listing)
        {
            if (listing == null)
                return BadRequest("Can't add an empty offer");

            _context.Listing.Add(listing);
            await _context.SaveChangesAsync();
            return Ok(listing);

        }

        [HttpPut]
        public async Task<ActionResult<Listing>> Put(Listing listing)
        {
            if (listing == null)
                return BadRequest();

            if (!_context.Listing.Any(x => x.id_listing == listing.id_listing))
                return NotFound();

            _context.Update(listing);
            await _context.SaveChangesAsync();
            return Ok(listing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Listing>> Delete(int id)
        {
            Listing listing = await _context.Listing.FirstOrDefaultAsync(x => x.id_listing == id);
            if (listing == null)
                return NotFound();

            _context.Listing.Remove(listing);
            await _context.SaveChangesAsync();
            return Ok(listing);
        }

    }
}
