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
        AppDBContext _appDBContext;

        public ListingController(ListingContext context, UserContext userContext, AppDBContext appDBContext)
        {
            _context = context;
            _userContext = userContext;
            _appDBContext = appDBContext;
        }

        [HttpGet("withPage/{page}")]
        public async Task<ActionResult<ListingResponse>> GetListings(int page)
        {
            if (_appDBContext.Listing == null)
                return NotFound();

            var pageResults = 12f;
            var pageCount = Math.Ceiling(_appDBContext.Listing.Count() / pageResults);

            await _appDBContext.Listing.Include(x => x.contents).ToListAsync();
            await _appDBContext.Listing.Include(x => x.locations).ToListAsync();

            var listings = await _appDBContext.Listing
                .OrderByDescending(x => x.id_listing)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var response = new ListingResponse
            {
                listings = listings,
                currentPage = page,
                pages = (int)pageCount
            };


            return response;
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
       
            _appDBContext.Listing.Add(listing);
            _appDBContext.User.Update(tmpUser);

            await _appDBContext.SaveChangesAsync();

            return Ok(listing);

        }

        [HttpPut, Authorize]
        public async Task<ActionResult<Listing>> Put(UpdatedListingDto listing)
        {
            int user_id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (listing == null)
                return BadRequest();

            Listing tmp = await _appDBContext.Listing.FirstOrDefaultAsync(x => x.id_listing == listing.id_listing);
            await _appDBContext.Listing.Include(x => x.tags).ToListAsync();
            await _appDBContext.Listing.Include(x => x.locations).ToListAsync();
            await _appDBContext.Tag.Include(x => x.listings).ToListAsync();


            if (tmp == null)
            {
                return NotFound();
            }

            if (user_id != tmp.id_user)
            {
                return BadRequest("Cannot delete other users listing");
            }
            
            tmp.post_name = listing.post_name;
            tmp.post_date = listing.post_date;
            tmp.post_desc = listing.post_desc;
            tmp.price = tmp.price;
            tmp.locations = listing.locations;
            tmp.post_date = DateTime.Now;
            

            foreach(Tag t in tmp.tags)
            {
                foreach (Tag n in listing.tags)
                {
                    if (t.tag_name.Equals(n.tag_name))
                    {
                        listing.tags.Remove(n);
                    }
                }
            }


            foreach (Tag t in listing.tags)
            {
                Tag individualTag = await _appDBContext.Tag.FirstOrDefaultAsync(x => x.tag_name == t.tag_name);
                //individualTag.listings = new HashSet<Listing>();
                individualTag.listings.Add(tmp);
                tmp.tags.Add(individualTag);
                _appDBContext.Tag.Update(individualTag); 
            }
            

            _appDBContext.Update(tmp);
            await _appDBContext.SaveChangesAsync();
            return Ok(tmp);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<Listing>> Delete(int id)
        {
            int user_id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Listing listing = await _appDBContext.Listing.FirstOrDefaultAsync(x => x.id_listing == id);
            if (user_id != listing.id_user)
            {
                return BadRequest("Cannot delete other users listing");
            }

            if (listing == null)
                return NotFound();


            _appDBContext.Listing.Remove(listing);
            await _appDBContext.SaveChangesAsync();
            return Ok(listing);
        }

    }
}
