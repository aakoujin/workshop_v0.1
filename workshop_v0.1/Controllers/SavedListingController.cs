using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using workshop_v0._1.DAL;
using workshop_v0._1.Models;

namespace workshop_v0._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class SavedListingController : ControllerBase
    {
        SavedListingContext _context;
        UserContext _userContext;
        ListingContext _listingContext;

        public SavedListingController(SavedListingContext context, UserContext userContext, ListingContext listingContext)
        {
            _context = context;
            _userContext = userContext;
            _listingContext = listingContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SavedListing>> GetById(int id)
        {
            var savedListing = await _context.SavedListing.FindAsync(id);

            if (savedListing == null)
            {
                return NotFound();
            }

            return savedListing;
        }

        [HttpGet("checkSaved/{idListing}"), Authorize]
        public async Task<ActionResult<SavedListing>> CheckSaved(int idListing)
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<SavedListing> savedListings = await _context.SavedListing.Where(x => (x.id_listing == idListing && x.id_user == id)).ToListAsync();

            if (savedListings.Count == 0)
            {
                return Ok(null);
            }

            return Ok(idListing);
        }

        [HttpGet("savedlistings"), Authorize]
        public async Task<ActionResult<SavedListing>> GetByUserClaim()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<SavedListing> savedListings = await _context.SavedListing.Where(x => x.id_user == id).ToListAsync();
         

            if (savedListings.Count == 0)
                return NotFound("No listings yet");

            List<Listing> fetchedListings = new();

            foreach (SavedListing sl in savedListings)
            {
                fetchedListings.Add(await _listingContext.Listing.FindAsync(sl.id_listing));
            }
            await _listingContext.Listing.Include(x => x.contents).ToListAsync();

            return new ObjectResult(fetchedListings);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var savedListing = await _context.SavedListing.FindAsync(id);

            if (savedListing == null)
            {
                return NotFound();
            }

            _context.SavedListing.Remove(savedListing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("addToSaved"), Authorize]
        public async Task<ActionResult<SavedListing>> PostByUserClaim(SavedListing savedListing)
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (savedListing == null) { return BadRequest("Error"); }

            User tmpUser = await _userContext.User.FirstOrDefaultAsync(x => x.id_user == id);

            //add duplicate check
            List<SavedListing> listings = await _context.SavedListing.Where(x => (x.id_user == id && x.id_listing == savedListing.id_listing)).ToListAsync();

            if(listings.Count > 0) { return BadRequest("Already in Saved"); }

            savedListing.id_user = id;
            savedListing.User = tmpUser;
            tmpUser.savedListings = new HashSet<SavedListing>();
            tmpUser.savedListings.Add(savedListing);
            _userContext.User.Update(tmpUser);
            await _userContext.SaveChangesAsync();

            return Ok(savedListing.id_saved_listing);
            //return CreatedAtAction(nameof(GetById), new { id = savedListing.id_saved_listing }, savedListing);
        }

        [HttpDelete("removeFromSaved/{idListing}"), Authorize]
        public async Task<IActionResult> DeleteByClaim(int idListing)
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            SavedListing savedListing = await _context.SavedListing.Where(x => (x.id_listing == idListing && x.id_user == id)).FirstOrDefaultAsync();

            if(savedListing == null)
            {
                return NotFound("No such saved item");
            }

            _context.SavedListing.Remove(savedListing);
            await _context.SaveChangesAsync();

            return Ok(idListing);
        }

        [HttpPost]
        public async Task<ActionResult<SavedListing>> Post(SavedListing savedListing)
        {
            _context.SavedListing.Add(savedListing);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = savedListing.id_saved_listing }, savedListing);
        }

        [HttpPut]
        public async Task<ActionResult<SavedListing>> Put(SavedListing savedListing)
        {
            if (savedListing == null)
                return BadRequest();

            if (!_context.SavedListing.Any(x => x.id_saved_listing == savedListing.id_saved_listing))
                return NotFound();

            _context.Update(savedListing);
            await _context.SaveChangesAsync();
            return Ok(savedListing);
        }
    }
}
