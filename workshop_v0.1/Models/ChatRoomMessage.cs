using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class ChatRoomMessage
    {
        [Key]
        public int id_chat_room_message { get; set; }
        public int chat_room_id { get; set; } 
        [ForeignKey("chat_room_id")]
        public ChatRoom chat_room { get; set; }
        public string sender { get; set; }
        public string message_content { get; set; }
    }
}
