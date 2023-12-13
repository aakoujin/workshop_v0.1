using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class ChatRoomDTO
    {
        public int id_chat_room { get; set; }
        public int first_user { get; set; }
        public int second_user { get; set; }
        public int listing { get; set; }
        public string connection_string { get; set; }
        public string media { get; set; }
        public int price { get; set; }
        public string name { get; set; }
        public virtual HashSet<ChatRoomMessage> chat_room_messages { get; set; }
    }
}
