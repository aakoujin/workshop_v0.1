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
    public class UserController : ControllerBase
    {
        UserContext _context;
        
        public UserController (UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            await _context.User.Include(x => x.listings).ToListAsync();
            return await _context.User.ToListAsync();
        }

        //api/offers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(int id)
        {
            User user = await _context.User.FirstOrDefaultAsync(x => x.id_user == id);
            await _context.User.Include(x => x.listings).ToListAsync();
            if (user == null)
                return NotFound("User doesn't exist");
            return new ObjectResult(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
                return BadRequest("Can't add an unnamed user");

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);

        }

        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (user == null)
                return BadRequest();

            if (!_context.User.Any(x => x.id_user == user.id_user))
                return NotFound();

            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            User user = await _context.User.FirstOrDefaultAsync(x => x.id_user == id);
            if (user == null)
                return NotFound();

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
