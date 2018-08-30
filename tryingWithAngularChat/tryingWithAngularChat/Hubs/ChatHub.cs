using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tryingWithAngularChat.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task PrintAll(string user)
        {
            var id = Context.ConnectionId;
            await Clients.All.SendAsync("printall", user, id);
        }

        public Task Join(string connid)
        {
            return Groups.Add(connid, "foo");
        }
        public Task LeaveRoom(string conid)
        {
            string roomName = "foo";
            return Groups.Remove(conid, roomName);
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "foo" };
            return Clients.Groups(groups).SendAsync("ReceiveMessageGrp", message);
        }
    }
}
