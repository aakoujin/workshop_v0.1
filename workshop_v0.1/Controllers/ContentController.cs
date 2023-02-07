using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.DAL;
using workshop_v0._1.Models;

namespace workshop_v0._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class ContentController : ControllerBase
    {
        ContentContext _context;

        public ContentController(ContentContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Content>>> Get()
        {
            return await _context.Content.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Content>>> Get(int id)
        {
            Content content = await _context.Content.FirstOrDefaultAsync(x => x.id_content == id);
            if (content == null)
                return NotFound("Content doesn't exist");
            return new ObjectResult(content);
        }

        [HttpPost]
        public async Task<ActionResult<Content>> Post(Content content)
        {
            if (content == null)
                return BadRequest("Can't add an empty media");

            _context.Content.Add(content);
            await _context.SaveChangesAsync();
            return Ok(content);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Content>> Delete(int id)
        {
            Content content = await _context.Content.FirstOrDefaultAsync(x => x.id_content == id);
            if (content == null)
                return NotFound();

            _context.Content.Remove(content);
            await _context.SaveChangesAsync();
            return Ok(content);
        }

    }
}
