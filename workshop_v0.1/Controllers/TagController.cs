using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.Models;
using workshop_v0._1.DAL;

namespace workshop_v0._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class TagController : ControllerBase
    {
        AppDBContext _context;
        //TagContext _context;

        public TagController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> Get()
        {
            return await _context.Tag.ToListAsync(); ;
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> Post(Tag tag)
        {
            if (tag == null)
                return BadRequest("Can't add an empty tag");

            _context.Tag.Add(tag);
            await _context.SaveChangesAsync();
            return Ok(tag);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Tag>> Delete(int id)
        {
            Tag tag = await _context.Tag.FirstOrDefaultAsync(x => x.id_tag == id);
            if (tag == null)
                return NotFound();

            _context.Tag.Remove(tag);
            await _context.SaveChangesAsync();
            return Ok(tag);
        }
    }
}