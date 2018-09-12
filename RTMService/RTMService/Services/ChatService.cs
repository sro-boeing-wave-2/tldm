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
            //creating default channels
            foreach(var channel in workSpace.Channels)
            {
                Channel newChannel = new Channel
                {
                    ChannelName = channel.ChannelName,
                    //Admin = user,
                    WorkspaceId = newWorkspace.WorkspaceId
                };
                // newChannel.Users.Add(user);
                CreateDefaultChannel(newChannel, workSpace.WorkspaceName);
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
        public Channel CreateDefaultChannel(Channel channel, string workspaceName)
        {
            var searchedWorkspace = GetWorkspaceByName(workspaceName);
            channel.WorkspaceId = searchedWorkspace.WorkspaceId;
            _dbChannel.GetCollection<Channel>("Channel").Save(channel);
            var result = GetWorkspaceById(searchedWorkspace.WorkspaceId);
            result.DefaultChannels.Add(channel);
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
        public User AddUserToDefaultChannel(User newUser, string channelId)
        {

            // add user to default channel and updating channel
            var resultChannel = GetChannelById(channelId);
            resultChannel.Users.Add(newUser);
            resultChannel.ChannelId = channelId;
            var res = Query<Channel>.EQ(pd => pd.ChannelId, channelId);
            var operation = Update<Channel>.Replace(resultChannel);
            _dbChannel.GetCollection<Channel>("Channel").Update(res, operation);

            // update channel in workspace
            var resultWorkspace = GetWorkspaceById(resultChannel.WorkspaceId);
            resultWorkspace.DefaultChannels.First(i => i.ChannelId == channelId).Users.Add(newUser);
            var resWorkspace = Query<Workspace>.EQ(pd => pd.WorkspaceId, resultWorkspace.WorkspaceId);
            var operationWorkspace = Update<Workspace>.Replace(resultWorkspace);
            _dbWorkSpace.GetCollection<Workspace>("Workspace").Update(resWorkspace, operationWorkspace);
            return newUser;

        }
        public Message AddMessageToChannel(Message message, string channelId, string senderMail)
        {

            // add user to channel and updating channel
            var resultChannel = GetChannelById(channelId);
            var resultWorkspace = GetWorkspaceById(resultChannel.WorkspaceId);
            var resultSender = GetUserByEmail(senderMail, resultWorkspace.WorkspaceName);
            Message newMessage = message;
            _dbMessage.GetCollection<Message>("Workspace").Save(newMessage);
            if (resultChannel.Messages.Count() < 50)
            {
                resultChannel.Messages.Add(newMessage);
            }
            else
            {
                resultChannel.Messages.RemoveAt(0);
                resultChannel.Messages.Add(newMessage);
            }

            resultChannel.ChannelId = channelId;
            var res = Query<Channel>.EQ(pd => pd.ChannelId, channelId);
            var operation = Update<Channel>.Replace(resultChannel);
            _dbChannel.GetCollection<Channel>("Channel").Update(res, operation);

            // update channel in workspace
            // var resultWorkspace = GetWorkspaceById(resultChannel.WorkspaceId);
           // try
           // {
           //     resultWorkspace.Channels.First(i => i.ChannelId == channelId).Messages.Add(newMessage);
           // }
           // catch
           // {

           // }
           // try
           // {
           //     resultWorkspace.DefaultChannels.First(i => i.ChannelId == channelId).Messages.Add(newMessage);
           // }
           // catch
           // {

           // }
           //// resultWorkspace.Channels.First(i => i.ChannelId == channelId).Messages.Add(newMessage);
           // //resultWorkspace.DefaultChannels.First(i => i.ChannelId == channelId).Messages.Add(newMessage);
           // var resWorkspace = Query<Workspace>.EQ(pd => pd.WorkspaceId, resultWorkspace.WorkspaceId);
           // var operationWorkspace = Update<Workspace>.Replace(resultWorkspace);
           // _dbWorkSpace.GetCollection<Workspace>("Workspace").Update(resWorkspace, operationWorkspace);
            return newMessage;

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

            var listOfDefaultChannels = resultWorkspace.DefaultChannels;
            foreach(var defaultChannel in listOfDefaultChannels)
            {
                AddUserToDefaultChannel(user, defaultChannel.ChannelId);
            }

            return user;
        }

        //public Channel GetGeneralChannelIdByWorkSpaceName(string workSpaceName)
        //{
        //    var resultWorkspace = GetWorkspaceByName(workSpaceName);
        //    var generalChannel = resultWorkspace.Channels.Find(i => i.ChannelName == "general");
        //    return generalChannel;
        //}
        public Channel GetChannelForOneToOneChat(string senderMail, string receiverMail, string workspaceName)
        {
            var workspace = GetWorkspaceByName(workspaceName);
            var channel = workspace.Channels.FindAll(m => (m.ChannelName == "") && m.Users.Any(u => u.EmailId == senderMail));
            var oneToOneChannel = channel.Find(c => c.Users.Any(u => u.EmailId == receiverMail));
            return oneToOneChannel;
        }


        public List<Channel> GetAllUserChannelsInWorkSpace(string workSpaceName, string emailId)
        {
            var workspace = GetWorkspaceByName(workSpaceName);
            var result = Query<Channel>.Where(p => (p.WorkspaceId== workspace.WorkspaceId) && (p.ChannelName!="") && (p.Users.Any(u => u.EmailId==emailId)));          
            return _dbChannel.GetCollection<Channel>("Channel").Find(result).ToList();
            
        }

        public List<Channel> GetAllChannelsInWorkspace(string workSpaceName)
        {

            var workspace = GetWorkspaceByName(workSpaceName);
            var result = Query<Channel>.Where(p => (p.WorkspaceId == workspace.WorkspaceId) && (p.ChannelName != ""));
            return _dbChannel.GetCollection<Channel>("Channel").Find(result).ToList();


        }

        public User GetUserByEmail(string emailId, string workspaceName)
        {
            var resultWorkspace = GetWorkspaceByName(workspaceName);
            var user = resultWorkspace.Users.Find(u => u.EmailId == emailId);
            return user;
            //var result = Query<User>.EQ(p => p.EmailId, emailId);
            //return _dbUser.GetCollection<User>("User").FindOne(result);
        }

        
    }
}
