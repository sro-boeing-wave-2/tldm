using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTMService.Hubs
{
    public class ChatHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            Clients.All.SendAsync("sendToAll", name, message);
        }

        public void PrintId(string id)
        {
            Clients.All.SendAsync("printId", Context.ConnectionId);
        }

        public void JoinGroup(string name)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, name);
        }

        public void JoinChannel(string ChannelId)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, ChannelId);
        }

        public void LeaveGroup(string name)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "foo");
        }

        public void SendMessageToGroups(string sender, string message)
        {
            List<string> groups = new List<string>() { "foo" };
            Clients.Groups(groups).SendAsync("SendMessageToGroups", sender, message);
        }

        public void SendMessageInChannel(string sender, string message, string channelId)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, channelId);
            Clients.Group(channelId).SendAsync("SendMessageInChannel", sender, message);
        }
    }
}
