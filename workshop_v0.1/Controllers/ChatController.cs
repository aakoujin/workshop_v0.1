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
                chatConnectionString = id + "_" + userCoonection.listing + userCoonection.username
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
        public async Task<ActionResult<IEnumerable<ChatRoomDTO>>> GetOutcommingChats()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var chats = await _appDBContext.ChatRoom.Where(x => x.first_user == id).ToListAsync();
            List<ChatRoomDTO> chatsDTO = new List<ChatRoomDTO>();

            foreach (ChatRoom chatRoom in chats)
            {
                var content = await _appDBContext.Content.FirstOrDefaultAsync(x => x.id_listing == chatRoom.listing);
                var listing = await _appDBContext.Listing.FirstOrDefaultAsync(x => x.id_listing == chatRoom.listing);


                if (listing != null)
                {

                    if (content != null && content.media != null)
                    {
                        chatsDTO.Add(new ChatRoomDTO
                        {
                            id_chat_room = chatRoom.id_chat_room,
                            first_user = chatRoom.first_user,
                            second_user = chatRoom.second_user,
                            connection_string = chatRoom.connection_string,
                            listing = chatRoom.listing,
                            media = content.media,
                            price = listing.price,
                            name = listing.post_name,
                            chat_room_messages = chatRoom.chat_room_messages
                        });
                    }
                    else if (content == null || content.media == null)
                    {
                        chatsDTO.Add(new ChatRoomDTO
                        {
                            id_chat_room = chatRoom.id_chat_room,
                            first_user = chatRoom.first_user,
                            second_user = chatRoom.second_user,
                            connection_string = chatRoom.connection_string,
                            listing = chatRoom.listing,
                            price = listing.price,
                            name = listing.post_name,
                            chat_room_messages = chatRoom.chat_room_messages
                        });
                    }
                }
            }

            return chatsDTO;
        }

        [HttpGet("getIncommingChats"), Authorize]
        public async Task<ActionResult<IEnumerable<ChatRoomDTO>>> GetIncommingChats()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var chats = await _appDBContext.ChatRoom.Where(x => x.second_user == id).ToListAsync();
            List<ChatRoomDTO> chatsDTO = new List<ChatRoomDTO>();

            foreach (ChatRoom chatRoom in chats)
            {
                var content = await _appDBContext.Content.FirstOrDefaultAsync(x => x.id_listing == chatRoom.listing);
                var listing = await _appDBContext.Listing.FirstOrDefaultAsync(x => x.id_listing == chatRoom.listing);


                if (listing != null)
                {

                    if (content != null && content.media != null)
                    {
                        chatsDTO.Add(new ChatRoomDTO
                        {
                            id_chat_room = chatRoom.id_chat_room,
                            first_user = chatRoom.first_user,
                            second_user = chatRoom.second_user,
                            connection_string = chatRoom.connection_string,
                            listing = chatRoom.listing,
                            media = content.media,
                            price = listing.price,
                            name = listing.post_name,
                            chat_room_messages = chatRoom.chat_room_messages
                        });
                    }
                    else if (content == null || content.media == null)
                    {
                        chatsDTO.Add(new ChatRoomDTO
                        {
                            id_chat_room = chatRoom.id_chat_room,
                            first_user = chatRoom.first_user,
                            second_user = chatRoom.second_user,
                            connection_string = chatRoom.connection_string,
                            listing = chatRoom.listing,
                            price = listing.price,
                            name = listing.post_name,
                            chat_room_messages = chatRoom.chat_room_messages
                        });
                    }

                }

            }

            return chatsDTO;
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ChatRoom>> GetChat(int id)
        {
            int id_user = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return await _appDBContext.ChatRoom.FirstOrDefaultAsync(x => x.id_chat_room == id);
        }

        [HttpPost("saveMessage"), Authorize]
        public async Task<ActionResult<ChatRoomMessage>> SaveMessage(ChatMessageDto chatMessageDto)
        {
            int id_user = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ChatRoom chatRoom = await _appDBContext.ChatRoom.FirstOrDefaultAsync(x => x.connection_string.Equals(chatMessageDto.connection_string));

            if (chatRoom != null)
            {
                ChatRoomMessage chatRoomMessage = new ChatRoomMessage
                {
                    message_content = chatMessageDto.message_content,
                    sender = id_user.ToString(),
                    chat_room = chatRoom
                };

                chatRoom.chat_room_messages = new HashSet<ChatRoomMessage>();
                chatRoom.chat_room_messages.Add(chatRoomMessage);
                await _appDBContext.SaveChangesAsync();

                return chatRoomMessage;
            }
            else
            {
                return BadRequest("An error happened with your request");
            }

        }

        [HttpPost("chatHistory"), Authorize]
        public async Task<ActionResult<IEnumerable<ChatRoomMessage>>> GetChatHistory(ChatMessageDto chatMessageDto)
        {
            ChatRoom tmp = await _appDBContext.ChatRoom.FirstOrDefaultAsync(x => x.connection_string.Equals(chatMessageDto.connection_string));

            if (tmp == null)
            {
                return new List<ChatRoomMessage>();
            }

            return await _appDBContext.ChatRoomMessage.Where(x => x.chat_room_id == tmp.id_chat_room).ToListAsync();
        }


    }
}