using RTMService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTMService.Services
{
    public interface IChatService
    {
        // workspace related task
        Workspace CreateWorkspace(Workspace workspace);
        void DeleteWorkspace(string workspaceName);
        IEnumerable<Workspace> GetAllWorkspaces();
        Workspace GetWorkspaceById(string id);
        Workspace GetWorkspaceByName(string workspaceName);


        //// not implemented
        //Task<Workspace> UpdateWorkspaceByName(string workspaceName);
        ////

        //// channel related task
        Channel CreateChannel(Channel channel, string workspaceId);
        Channel GetChannelById(string channelId);
        List<Channel> GetAllUserChannelsInWorkSpace(string workSpaceName, string emailid);
        List<Channel> GetAllChannelsInWorkspace(string workSpaceName);
        //Channel GetGeneralChannelIdByWorkSpaceName(string workSpaceName);
        void DeleteChannel(string channelId);
        //Task<Channel> UpdateChannel(Channel channel);
        //Task<List<Channel>> GetChannelByuserIDandWorkspaceName(int userId, string workspaceName);
        User AddUserToChannel(User user, string channelId);
        // void DeleteUserFromChannel(string userName, int channelId);
        //Task<List<Message>> GetMessagesInChannel(int channelId, string workspaceName);

        //// user related task
        List<User> GetAllUsersInWorkspace(string workspaceName);
        //User AddUserToWorkspace(User user, string workspaceName);  ////????? 
        User GetUserById(string emailid);
        //Task DeleteUserFromWorkspace(string workspaceName, int userId);
        //Task UpdateUserInWorkspace(User user);

        ////Message related task
        //Task DeleteMessageInChannel(string workspaceName, int channelId, int messageId);

    }
}
