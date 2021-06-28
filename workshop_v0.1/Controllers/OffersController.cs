using Microsoft.AspNetCore.Http;
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
    public class OffersController : ControllerBase
    {
        OfferContext _context;
        public OffersController(OfferContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offer>>> Get()
        {
            return await _context.Offer.ToListAsync();
        }

        //api/offers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Offer>>> Get(int id)
        {
            Offer offer = await _context.Offer.FirstOrDefaultAsync(x => x.Id_offer == id);
            if (offer == null)
               return NotFound("Offer doesn't exist");
            return new ObjectResult(offer);
        }

        [HttpPost]
        public async Task<ActionResult<Offer>> Post(Offer offer)
        {
            if (offer == null)
                return BadRequest("Can't add an empty offer");

            _context.Offer.Add(offer);
            await _context.SaveChangesAsync();
            return Ok(offer);
       
        }

        [HttpPut]
        public async Task<ActionResult<Offer>> Put(Offer offer)
        {
            if (offer == null)
                return BadRequest();

            if (!_context.Offer.Any(x => x.Id_offer == offer.Id_offer))
                return NotFound();

            _context.Update(offer);
            await _context.SaveChangesAsync();
            return Ok(offer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Offer>> Delete(int id)
        {
            Offer offer = await _context.Offer.FirstOrDefaultAsync(x => x.Id_offer == id);
            if (offer == null)
                return NotFound();

            _context.Offer.Remove(offer);
            await _context.SaveChangesAsync();
            return Ok(offer);
        }

        }
    }
