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
        Workspace CreateWorkspace(WorkspaceView workspace);
        void DeleteWorkspace(string workspaceName);
        IEnumerable<Workspace> GetAllWorkspaces();
        Workspace GetWorkspaceById(string id);
        Workspace GetWorkspaceByName(string workspaceName);


        //// not implemented
        //Task<Workspace> UpdateWorkspaceByName(string workspaceName);
        ////

        //// channel related task
        Channel CreateChannel(Channel channel, string workspaceName);
        Channel GetChannelById(string channelId);
        List<Channel> GetAllUserChannelsInWorkSpace(string workSpaceName, string emailId);
        List<Channel> GetAllChannelsInWorkspace(string workSpaceName);
        Channel GetChannelForOneToOneChat(string senderMail, string receiverMail, string workspaceName);
        //Channel GetGeneralChannelIdByWorkSpaceName(string workSpaceName);
        void DeleteChannel(string channelId);
        //Task<Channel> UpdateChannel(Channel channel);
        //Task<List<Channel>> GetChannelByuserIDandWorkspaceName(int userId, string workspaceName);
        User AddUserToChannel(User user, string channelId);
        void DeleteUserFromChannel(string emailId, string channelId);
        //Task<List<Message>> GetMessagesInChannel(int channelId, string workspaceName);

        //// user related task
        List<User> GetAllUsersInWorkspace(string workspaceName);
        User AddUserToWorkspace(UserAccountView user, string workspaceName);  
        User GetUserByEmail(string emailId);
        //Task DeleteUserFromWorkspace(string workspaceName, int userId);
        //Task UpdateUserInWorkspace(User user);

        ////Message related task
        //Task DeleteMessageInChannel(string workspaceName, int channelId, int messageId);

    }
}
