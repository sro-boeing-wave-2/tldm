using MongoDB.Driver;
using MongoDB.Driver.Builders;
using RTMService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTMService.Services
{
    public class ChatService : IChatService
    {
        MongoClient _client;
        MongoServer _server;
        MongoDatabase _dbWorkSpace;
        MongoDatabase _dbChannel;
        MongoDatabase _dbUser;
        MongoDatabase _dbMessage;

        public ChatService()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _server = _client.GetServer();
            _dbWorkSpace = _server.GetDatabase("AllWorkspace");
            _dbChannel = _server.GetDatabase("AllChannels");
            _dbUser = _server.GetDatabase("AllUsers");
            _dbMessage = _server.GetDatabase("AllMessages");

        }

        public IEnumerable<Workspace> GetAllWorkspaces()
        {
            return _dbWorkSpace.GetCollection<Workspace>("Workspace").FindAll();
        }



        public Workspace GetWorkspaceById(string id)
        {
            var result = Query<Workspace>.EQ(p => p.WorkspaceId, id);
            return _dbWorkSpace.GetCollection<Workspace>("Workspace").FindOne(result);
        }

        public Workspace GetWorkspaceByName(string workspaceName)
        {
            var result = Query<Workspace>.EQ(p => p.WorkspaceName, workspaceName);
            return _dbWorkSpace.GetCollection<Workspace>("Workspace").FindOne(result);
        }

        public Workspace CreateWorkspace(WorkspaceView workSpace)
        {
            Workspace newWorkspace = new Workspace
            {
                WorkspaceId = workSpace.Id,
                WorkspaceName = workSpace.WorkspaceName
            };

            _dbWorkSpace.GetCollection<Workspace>("Workspace").Save(newWorkspace);
            //User user = new User
            //{
            //    UserId = workSpace.UserWorkspaces[0].UserAccount.Id,
            //    EmailId = workSpace.UserWorkspaces[0].UserAccount.EmailId,
            //    FirstName = workSpace.UserWorkspaces[0].UserAccount.FirstName,
            //    LastName = workSpace.UserWorkspaces[0].UserAccount.LastName
            //};
            //_dbUser.GetCollection<User>("User").Save(user);
            //AddUserToWorkspace(user, newWorkspace.WorkspaceName);
            foreach(var channel in workSpace.Channels)
            {
                Channel newChannel = new Channel
                {
                    ChannelName = channel.ChannelName,
                    //Admin = user,
                    WorkspaceId = newWorkspace.WorkspaceId
                };
               // newChannel.Users.Add(user);
                CreateChannel(newChannel, workSpace.WorkspaceName);
            }

             return GetWorkspaceByName(workSpace.WorkspaceName);
        }
        public void DeleteWorkspace(string id)
        {
            var result = Query<Workspace>.EQ(e => e.WorkspaceId, id);
            var operation = _dbWorkSpace.GetCollection<Workspace>("Workspace").Remove(result);
        }

        public Channel CreateChannel(Channel channel, string workspaceName)
        {
            var searchedWorkspace = GetWorkspaceByName(workspaceName);
            channel.WorkspaceId = searchedWorkspace.WorkspaceId;
            _dbChannel.GetCollection<Channel>("Channel").Save(channel);
            var result = GetWorkspaceById(searchedWorkspace.WorkspaceId);
            result.Channels.Add(channel);
            result.WorkspaceId = searchedWorkspace.WorkspaceId;
            var res = Query<Workspace>.EQ(pd => pd.WorkspaceId, searchedWorkspace.WorkspaceId);
            var operation = Update<Workspace>.Replace(result);
            _dbWorkSpace.GetCollection<Workspace>("Workspace").Update(res, operation);
            return channel;
        }
        public Channel GetChannelById(string channelId)
        {
            var result = Query<Channel>.EQ(p => p.ChannelId, channelId);
            return _dbChannel.GetCollection<Channel>("Channel").FindOne(result);
        }
        public User AddUserToChannel(User newUser, string channelId)
        {

            // add user to channel and updating channel
            var resultChannel = GetChannelById(channelId);
            resultChannel.Users.Add(newUser);
            resultChannel.ChannelId = channelId;
            var res = Query<Channel>.EQ(pd => pd.ChannelId, channelId);
            var operation = Update<Channel>.Replace(resultChannel);
            _dbChannel.GetCollection<Channel>("Channel").Update(res, operation);

            // update channel in workspace
            var resultWorkspace = GetWorkspaceById(resultChannel.WorkspaceId);
            resultWorkspace.Channels.First(i => i.ChannelId == channelId).Users.Add(newUser);
            var resWorkspace = Query<Workspace>.EQ(pd => pd.WorkspaceId, resultWorkspace.WorkspaceId);
            var operationWorkspace = Update<Workspace>.Replace(resultWorkspace);
            _dbWorkSpace.GetCollection<Workspace>("Workspace").Update(resWorkspace, operationWorkspace);
            return newUser;

        }
        public void DeleteChannel(string channelId)
        {
            var channelresult = GetChannelById(channelId);
            var result = Query<Channel>.EQ(e => e.ChannelId, channelId);
            var operation = _dbChannel.GetCollection<Channel>("Channel").Remove(result);
            var workspace = GetWorkspaceById(channelresult.WorkspaceId);
            var channelToDelete = workspace.Channels.Find(c => c.ChannelId == channelId);
            workspace.Channels.Remove(channelToDelete);
            var resworkspace = Query<Workspace>.EQ(pd => pd.WorkspaceId, channelresult.WorkspaceId);
            var operationWorkspace = Update<Workspace>.Replace(workspace);
            _dbWorkSpace.GetCollection<Workspace>("Workspace").Update(resworkspace, operationWorkspace);

        }
        //not working 
        public void DeleteUserFromChannel(string emailId, string channelId)
        {
            var channel = GetChannelById(channelId);

            var resultUser = channel.Users.Find(u => u.EmailId == emailId);//GetUserByEmail(emailId);

            channel.Users.Remove(resultUser);
            var resultChannel = Query<Channel>.EQ(pd => pd.ChannelId, channelId);
            var operationChannel = Update<Channel>.Replace(channel);
            _dbChannel.GetCollection<Channel>("Channel").Update(resultChannel, operationChannel);

            var resultWorkspace = GetWorkspaceById(channel.WorkspaceId);
            resultWorkspace.WorkspaceId = channel.WorkspaceId;
            var userToDelete=  resultWorkspace.Channels.Find(c => c.ChannelId == channelId).Users.Find(u => u.EmailId == emailId);
            resultWorkspace.Channels.Find(c => c.ChannelId == channelId).Users.Remove(userToDelete);
            var result = Query<Workspace>.EQ(pd => pd.WorkspaceId, channel.WorkspaceId);
            var operationWorkspace = Update<Workspace>.Replace(resultWorkspace);
            _dbWorkSpace.GetCollection<Workspace>("Workspace").Update(result, operationWorkspace);
        }

        public List<User> GetAllUsersInWorkspace(string workspaceName)
        {
            var resultWorkspace = GetWorkspaceByName(workspaceName);
            return resultWorkspace.Users;
        }

        public User AddUserToWorkspace(UserAccountView newuser, string workspaceName)
        {
            User user = new User
            {
                UserId = newuser.Id,
                EmailId = newuser.EmailId,
                FirstName = newuser.FirstName,
                LastName = newuser.LastName
            };
            _dbUser.GetCollection<User>("User").Save(user);
            var resultWorkspace = GetWorkspaceByName(workspaceName);
            resultWorkspace.Users.Add(user);
            var resworkspace = Query<Workspace>.EQ(pd => pd.WorkspaceName, workspaceName);
            var operationWorkspace = Update<Workspace>.Replace(resultWorkspace);
            _dbWorkSpace.GetCollection<Workspace>("Workspace").Update(resworkspace, operationWorkspace);
            return user;
        }

        //public Channel GetGeneralChannelIdByWorkSpaceName(string workSpaceName)
        //{
        //    var resultWorkspace = GetWorkspaceByName(workSpaceName);
        //    var generalChannel = resultWorkspace.Channels.Find(i => i.ChannelName == "general");
        //    return generalChannel;
        //}



        public List<Channel> GetAllUserChannelsInWorkSpace(string workSpaceName, string emailId)
        {
            var workspace = GetWorkspaceByName(workSpaceName);
            var listOfChannels = workspace.Channels.FindAll(m => m.Users.Any(u => u.EmailId == emailId));
            return listOfChannels;
        }

        public List<Channel> GetAllChannelsInWorkspace(string workSpaceName)
        {
            
                var workspace = GetWorkspaceByName(workSpaceName);
                return workspace.Channels;
           
            
        }

        public User GetUserByEmail(string emailId)
        {
            var result = Query<User>.EQ(p => p.EmailId, emailId);
            return _dbUser.GetCollection<User>("User").FindOne(result);
        }

        
    }
}
