using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tryingWithAngularChat.Models;

namespace tryingWithAngularChat.Services
{
    public interface IChatService
    {
        // workspace related task
        Task<Workspace> CreateWorkspace(string workspaceName);
        Task DeleteWorkspace(string workspaceName);
        Task<List<Workspace>> GetAllWorkspaces();
        Task<Workspace> GetWorkspaceByName(string workspaceName);
        Task<Workspace> UpdateWorkspaceByName(string workspaceName);
         
        // channel related task
        Task<Channel> CreateChannel(Channel channel, string workspaceName);
        Task DeleteChannel(int channelId, string workspaceName);
        Task<Channel> UpdateChannel(Channel channel);
        Task<List<Channel>> GetChannelByuserIDandWorkspaceName(int userId, string workspaceName);
        Task AddUserToChannel(string userName, int channelId);
        Task DeleteUserFromChannel(string userName, int channelId);
        Task<List<Message>> GetMessagesInChannel(int channelId, string workspaceName);

        // user related task
        Task<List<User>> GetAllUsersInWorkspace(string workspaceName);
        Task AddUserToWorkspace(string workspaceName, User user);  ////????? 
        Task DeleteUserFromWorkspace(string workspaceName, int userId);
        Task UpdateUserInWorkspace(User user);

        //Message related task
        Task DeleteMessageInChannel(string workspaceName, int channelId, int messageId);

    }
}
