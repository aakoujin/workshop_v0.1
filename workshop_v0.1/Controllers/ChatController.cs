using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using workshop_v0._1.Models;

namespace workshop_v0._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class ChatController : ControllerBase
    {
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
    }
}
