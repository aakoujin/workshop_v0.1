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
    public class ListingController : ControllerBase
    {
        ListingContext _context;
        UserContext _userContext;

        public ListingController(ListingContext context, UserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> Get()
        {
            await _context.Listing.Include(x => x.contents).ToListAsync();
            return await _context.Listing.ToListAsync();
        }


        [HttpGet("userlistings"), Authorize]
        public async Task<ActionResult<IEnumerable<Listing>>> GetByUserClaim()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
      
            List<Listing> listings = await _context.Listing.Where(x => x.id_user == id).ToListAsync();
            await _context.Listing.Include(x => x.contents).ToListAsync();

            if (listings.Count == 0)
                return NotFound("No listings yet");
            return new ObjectResult(listings);
        }


        //api/offers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Listing>>> Get(int id)
        {
            Listing listing = await _context.Listing.FirstOrDefaultAsync(x => x.id_listing == id);
            await _context.Listing.Include(x => x.contents).ToListAsync();
            if (listing == null)
                return NotFound("Listing doesn't exist");
            return new ObjectResult(listing);
        }

        [HttpPost]
        public async Task<ActionResult<Listing>> Post(Listing listing)
        {
            if (listing == null)
                return BadRequest("Can't add an empty offer");

            if (listing.user == null && listing.state != 0)
            {
                User tmpUser = await _userContext.User.FirstOrDefaultAsync(x => x.id_user == listing.state);

                if (tmpUser == null) { return BadRequest("User does not exit"); }

                listing.user = tmpUser;
                listing.post_date = DateTime.Now;
                tmpUser.listings = new HashSet<Listing>();
                tmpUser.listings.Add(listing);
                _userContext.User.Update(tmpUser);
                await _userContext.SaveChangesAsync();
                return Ok(listing);
            }

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
