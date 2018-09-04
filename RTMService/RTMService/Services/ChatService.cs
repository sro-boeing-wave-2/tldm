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
        public Workspace CreateWorkspace(Workspace workSpace)
        {
            //Channel generalChannel = new Channel();
            //generalChannel.ChannelName = "general";
            //_dbChannel.GetCollection<Channel>("Channel").Save(generalChannel);
            ////Workspace newWorkSpace = new Workspace();
            ////workSpace.workspaceName = workSpace;
            //workSpace.Channels.Add(generalChannel);
            _dbWorkSpace.GetCollection<Workspace>("Workspace").Save(workSpace);
            return workSpace;
        }
        public void DeleteWorkspace(string id)
        {
            var result = Query<Workspace>.EQ(e => e.WorkspaceId, id);
            var operation = _dbWorkSpace.GetCollection<Workspace>("Workspace").Remove(result);
        }

        public Channel CreateChannel(Channel channel, string workspaceId)
        {
            channel.WorkspaceId = workspaceId;
            _dbChannel.GetCollection<Channel>("Channel").Save(channel);
            var result = GetWorkspaceById(workspaceId);
            result.Channels.Add(channel);
            result.WorkspaceId = workspaceId;
            var res = Query<Workspace>.EQ(pd => pd.WorkspaceId, workspaceId);
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

        //public void DeleteUserFromChannel(string userName, int channelId)
        //{
        //    var result = Query<Channel>.EQ(e => e.channelId, channelId);
        //    var operation = _db.GetCollection<Workspace>("Workspace").Remove(result);
        //}

        public List<User> GetAllUsersInWorkspace(string workspaceName)
        {
            var resultWorkspace = GetWorkspaceByName(workspaceName);
            return resultWorkspace.Users;
        }

        //public User AddUserToWorkspace( User user)
        //{
        //    _dbUser.GetCollection<User>("User").Save(user);
        //    var resultWorkspace = GetWorkspaceByName(user.workspaceName);
        //    resultWorkspace.channels.Find(i => i.channelName == "general").users.Add(user);
        //    var resworkspace = Query<Workspace>.EQ(pd => pd.workspaceName, user.workspaceName);
        //    var operationWorkspace = Update<Workspace>.Replace(resultWorkspace);
        //    _dbWorkSpace.GetCollection<Workspace>("Workspace").Update(resworkspace, operationWorkspace);
        //    return user;
        //}

        //public Channel GetGeneralChannelIdByWorkSpaceName(string workSpaceName)
        //{
        //    var resultWorkspace = GetWorkspaceByName(workSpaceName);
        //    var generalChannel = resultWorkspace.Channels.Find(i => i.ChannelName == "general");
        //    return generalChannel;
        //}



        public List<Channel> GetAllUserChannelsInWorkSpace(string workSpaceName, string emailid)
        {
            var workspace = GetWorkspaceByName(workSpaceName);
            var listOfChannels = workspace.Channels.FindAll(m => m.Users.Any(u => u.EmailId == emailid));
            return listOfChannels;
        }

        public List<Channel> GetAllChannelsInWorkspace(string workSpaceName)
        {
            var workspace = GetWorkspaceByName(workSpaceName);
            return workspace.Channels;
        }

        public User GetUserById(string emailid)
        {
            var result = Query<User>.EQ(p => p.EmailId, emailid);
            return _dbUser.GetCollection<User>("User").FindOne(result);
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
    }
}
