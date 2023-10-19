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
    public class ChatController : ControllerBase
    {

        AppDBContext _appDBContext;

        public ChatController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpPost("getConnection"), Authorize]
        public async Task<ActionResult<ChatConnection>> GetChatConnectionString(UserConnection userCoonection)
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ChatConnection result = new ChatConnection()
            {
                chatConnectionString = id +"_"+ userCoonection.listing + userCoonection.username
            };

            return result;
        }

        [HttpPost("registerRoom"), Authorize]
        public async Task<ActionResult<ChatRoom>> RegisterRoom(UserConnection userCoonection)
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User second_user = _appDBContext.User.FirstOrDefault(x => x.creds.First().username.Equals(userCoonection.username));

            ChatRoom chatRoom = new ChatRoom()
            {
                first_user = id,
                second_user = second_user.id_user,
                listing = userCoonection.listing,
                connection_string = id + "_" + userCoonection.listing + userCoonection.username
            };

            ChatRoom stored = _appDBContext.ChatRoom.FirstOrDefault(x => x.connection_string.Equals(chatRoom.connection_string));

            if (stored != null)
            {
                return stored;
            }

            _appDBContext.Add(chatRoom);
            await _appDBContext.SaveChangesAsync();
            

            return chatRoom;
        }

        [HttpGet("getOutcommingChats"), Authorize]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> GetOutcommingChats()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return await _appDBContext.ChatRoom.Where(x => x.first_user == id).ToListAsync();
        }

        [HttpGet("getIncommingChats"), Authorize]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> GetIncommingChats()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return await _appDBContext.ChatRoom.Where(x => x.second_user == id).ToListAsync();
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ChatRoom>> GetChat(int id)
        {
            int id_user = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return await _appDBContext.ChatRoom.FirstOrDefaultAsync(x => x.id_chat_room == id);
        }
    }
}
