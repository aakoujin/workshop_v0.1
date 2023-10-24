using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using workshop_v0._1.Controllers;
using workshop_v0._1.Models;


namespace workshop_v0._1.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _bot;

        public ChatHub()
        {
            _bot = "Chat bot";
        }

        /*
        persist room in DB

        retrieve room connection string for SendMessage. Send message to the room 

        link rooms to user_id. Display all chats(rooms) on separate page
         */

        public async Task SendMessage(string message, string chatConnectionString)
        {
            await Clients.Group(chatConnectionString).SendAsync("ReceiveMessage", chatConnectionString.Split("_")[0], message);
            //await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinRoom(ChatConnection chatConnection)
        {
           

            await Groups.AddToGroupAsync(Context.ConnectionId, chatConnection.chatConnectionString);

            await Clients.Group(chatConnection.chatConnectionString).SendAsync("ReceiveMessage",
                _bot, $"Chat started");
        }
    }
}
