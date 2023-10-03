using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
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
        accept 2 parameters in userConnection: username, listing
         
        generate a room by combining: userId from Auth, listingId, username from FE
         */

        public async Task JoinRoom(ChatConnection chatConnection)
        {
           

            await Groups.AddToGroupAsync(Context.ConnectionId, chatConnection.chatConnectionString);

            await Clients.Group(chatConnection.chatConnectionString).SendAsync("ReceiveMessage",
                _bot, $"Chat started");
        }
    }
}
