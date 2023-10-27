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
        TagContext _tagContext;
        AppDBContext _appDBContext;

        public ListingController(ListingContext context, UserContext userContext, TagContext tagContext, AppDBContext appDBContext)
        {
            _context = context;
            _userContext = userContext;
            _tagContext = tagContext;
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> Get()
        {
            await _appDBContext.Listing.Include(x => x.contents).ToListAsync();
            //await _appDBContext.Listing.Include(x => x.tags).ToListAsync();
            var listings = await _appDBContext.Listing.OrderByDescending(x => x.id_listing).ToListAsync();
            await _appDBContext.Listing.Include(x => x.locations).ToListAsync();

            return listings;
        }

        [HttpGet("userlistings"), Authorize]
        public async Task<ActionResult<IEnumerable<Listing>>> GetByUserClaim()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Listing> listings = await _context.Listing.Where(x => x.id_user == id).OrderByDescending(x => x.id_listing).ToListAsync();
            await _context.Listing.Include(x => x.contents).ToListAsync();
            await _appDBContext.Listing.Include(x => x.locations).ToListAsync();

            if (listings.Count == 0)
                return NotFound("No listings yet");
            return new ObjectResult(listings);
        }


        //api/offers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Listing>>> Get(int id)
        {
            Listing listing = await _appDBContext.Listing.FirstOrDefaultAsync(x => x.id_listing == id);

            listing.state++;

            await _appDBContext.Listing.Include(x => x.contents).ToListAsync();
            await _appDBContext.Listing.Include(x => x.tags).ToListAsync();
            await _appDBContext.Listing.Include(x => x.locations).ToListAsync();

            _appDBContext.Listing.Update(listing);
            await _appDBContext.SaveChangesAsync();

            if (listing == null)
                return NotFound("Listing doesn't exist");

            foreach (Tag tag in listing.tags)
            {
                tag.listings = null;
            }

            return new ObjectResult(listing);
            //return Ok(listing);
        }

        [HttpGet("similar/{id}")]
        public async Task<ActionResult<IEnumerable<Listing>>> GetRelatedListings(int id)
        {
            Listing tmp = await _appDBContext.Listing.FirstOrDefaultAsync(x => x.id_listing == id);
            await _appDBContext.Listing.Include(x=> x.tags).ToListAsync();
            
            HashSet<Tag> requestTags = new(tmp.tags);
            //update algorithm to fetch by set of tags

            return await _appDBContext.Listing.Take(10).Include(x => x.contents).OrderByDescending(x => x.id_listing).ToListAsync();
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<Listing>> Post(Listing listing)
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (listing == null) { return BadRequest("Can't add an empty offer"); }
               

            User tmpUser = await _userContext.User.FirstOrDefaultAsync(x => x.id_user == id);

            //fix tag duplication
            HashSet<Tag> tmpTags = new();
            
            foreach(Tag t in listing.tags)
            {
                Tag individualTag = await _appDBContext.Tag.FirstOrDefaultAsync(x => x.tag_name == t.tag_name);
                individualTag.listings = new HashSet<Listing>();
                individualTag.listings.Add(listing);
                tmpTags.Add(individualTag);
                _appDBContext.Tag.Update(individualTag);
            }
            listing.tags = null;

            listing.tags = new HashSet<Tag>(tmpTags);
            listing.user = tmpUser;
            listing.post_date = DateTime.Now;

            tmpUser.listings = new HashSet<Listing>();
            tmpUser.listings.Add(listing);


            //_userContext.User.Update(tmpUser);
            //await _userContext.SaveChangesAsync();   

            //_context.Listing.Add(listing);
            //await _context.SaveChangesAsync();         
            _appDBContext.Listing.Add(listing);
            _appDBContext.User.Update(tmpUser);
            
            await _appDBContext.SaveChangesAsync();

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
