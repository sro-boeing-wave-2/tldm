using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RTMService.Controllers;
using RTMService.Models;
using RTMService.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace ChatUnitTesting
{
    public class UnitTest1
    {
        public ObjectId Id1 = ObjectId.GenerateNewId();
        public Workspace Postworkspace = new Workspace()
        {
            WorkspaceId = "5b71298a6a2e663634872a65",
            WorkspaceName = "dummyWorkspace",
            Channels = new List<Channel> { },
            Users = new List<User> { }
        };
        public class FakeChatService : IChatService
        {
            public Workspace CreateWorkspace(Workspace workspace)
            {
                Workspace workspace1 = new Workspace()
                {
                    WorkspaceId = "5b71298a6a2e663634872a65",
                    WorkspaceName = "dummyWorkspace",
                    Channels = new List<Channel> { },
                    Users = new List<User> { }
                };
                return (workspace1);
            }
            public void DeleteWorkspace(string workspaceName)
            {
                Workspace workspace1 = new Workspace()
                {
                    WorkspaceId = "5b71298a6a2e663634872a65",
                    WorkspaceName = "dummyWorkspace",
                    Channels = new List<Channel> { },
                    Users = new List<User> { }
                };
            }
            public IEnumerable<Workspace> GetAllWorkspaces()
            {
                List<Workspace> allworkspace = new List<Workspace>
                     {
                    new Workspace(){
                    WorkspaceId = "5b71298a6a2e663634872a65",
                    WorkspaceName = "dummyWorkspace",
                    Channels = new List<Channel> { },
                    Users = new List<User> { }
                    }
                };
                return allworkspace;
            }
            public Workspace GetWorkspaceById(string workspaceName)
            {
                Workspace workspace1 = new Workspace()
                {
                    WorkspaceId = "5b71298a6a2e663634872a65",
                    WorkspaceName = "dummyWorkspace",
                    Channels = new List<Channel> { },
                    Users = new List<User> { }
                };
                return (workspace1);
            }
            public Workspace GetWorkspaceByName(string workspaceName)
            {
                Workspace workspace1 = new Workspace()
                {
                    WorkspaceId = "5b71298a6a2e663634872a65",
                    WorkspaceName = "dummyWorkspace",
                    Channels = new List<Channel> { },
                    Users = new List<User> { }
                };
                return (workspace1);
            }
            public Channel CreateChannel(Channel channel, string workspaceId)
            {
                Channel channel1 = new Channel()
                {
                    ChannelId = "5b8932523f0c56095c70d82d",
                    ChannelName = "firstchannel",
                    Users = new List<User> { },
                    Admin = new User { },
                    Messages = new List<Message> { },
                    WorkspaceId = null
                };
                return (channel1);
            }
            public Channel GetChannelById(string channelId)
            {
                Channel channel1 = new Channel()
                {
                    ChannelId = "5b8932523f0c56095c70d82d",
                    ChannelName = "firstchannel",
                    Users = new List<User> { },
                    Admin = new User { },
                    Messages = new List<Message> { },
                    WorkspaceId = null
                };
                return (channel1);
            }
            public List<Channel> GetAllUserChannelsInWorkSpace(string workSpaceName, string emailid)
            {
                List<Channel> allchannels = new List<Channel> {
                    new Channel()
                    {
                         ChannelId = "5b8932523f0c56095c70d82d",
                    ChannelName = "firstchannel",
                    Users = new List<User> { },
                    Admin = new User { },
                    Messages = new List<Message> { },
                    WorkspaceId = null
                    },
                     new Channel()
                    {
                         ChannelId = "5b8932523f0c56095c70d82d",
                    ChannelName = "firstchannel",
                    Users = new List<User> { },
                    Admin = new User { },
                    Messages = new List<Message> { },
                    WorkspaceId = null
                    }
                };
                return (allchannels);
            }
            public List<Channel> GetAllChannelsInWorkspace(string workSpaceName)
            {
                List<Channel> allchannels = new List<Channel> {
                    new Channel()
                    {
                         ChannelId = "5b8932523f0c56095c70d82d",
                    ChannelName = "firstchannel",
                    Users = new List<User> { },
                    Admin = new User { },
                    Messages = new List<Message> { },
                    WorkspaceId = null
                    },
                     new Channel()
                    {
                         ChannelId = "5b8932523f0c56095c70d82d",
                    ChannelName = "firstchannel",
                    Users = new List<User> { },
                    Admin = new User { },
                    Messages = new List<Message> { },
                    WorkspaceId = null
                    }
                };
                return (allchannels);
            }
            public void DeleteChannel(string channelId)
            {
                Channel channel1 = new Channel()
                {
                    ChannelId = "5b8932523f0c56095c70d82d",
                    ChannelName = "firstchannel",
                    Users = new List<User> { },
                    Admin = new User { },
                    Messages = new List<Message> { },
                    WorkspaceId = null
                };
            }
            /// //////////////Check
            public User AddUserToChannel(User user, string channelId)
            {
                User user1 = new User()
                {
                    UserId = null,
                    FirstName = null,
                    LastName = null,
                    EmailId = null,
                    Designation = null
                };
                return user1;
            }
            public List<User> GetAllUsersInWorkspace(string workspaceName)
            {
                List<User> allusers = new List<User>() {
                    new User(){
                        UserId=null,
                        FirstName=null,
                        LastName=null,
                        EmailId=null,
                        Designation=null
                    }
                };
                return allusers;
            }
            public User GetUserById(string emailid)
            {
                User user1 = new User()
                {
                    UserId = null,
                    FirstName = null,
                    LastName = null,
                    EmailId = null,
                    Designation = null
                };
                return user1;
            }
        }
        /// ////////////////////////////////////////////////
        [Fact]
        public void CreateWorkspace()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.CreateWorkspace(Postworkspace);
            var workspaceposted = result as OkObjectResult;
            var workspace = workspaceposted.Value as Workspace;
            Assert.Equal("1", workspace.WorkspaceId);
        }
        [Fact]
        public void Getallworkspace()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.GetAllWorkspace();
            var workspaceposted = result as OkObjectResult;
            var workspace = workspaceposted.Value as Workspace;
            Assert.Equal("dummyWorkspace", workspace.WorkspaceName);
        }
        [Fact]
        public void Getworkspacebyid()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.GetWorkspaceById("5b71298a6a2e663634872a65");
            var workspaceposted = result as OkObjectResult;
            var workspace = workspaceposted.Value as Workspace;
            Assert.Equal("5b71298a6a2e663634872a65", workspace.WorkspaceId);
        }
        [Fact]
        public void deleteaworkspace()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.Delete("5b71298a6a2e663634872a65");
            var notePosted = result as OkResult;
            Assert.Equal(200, notePosted.StatusCode);
        }
        [Fact]
        public void deletechannel()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.DeleteChannelByIdAndWorkspaceName("5b8932523f0c56095c70d82d");
            var notePosted = result as OkResult;
            Assert.Equal(200, notePosted.StatusCode);
        }
        [Fact]
        public void createchannelinworkspace()
        {
            var putworkspace = new Workspace()
            {
                WorkspaceId = "5b71298a6a2e663634872a65",
                WorkspaceName = "dummyWorkspace",
                Channels = new List<Channel> { },
                Users = new List<User> { }
            };
            Channel channel1 = new Channel()
            {
                ChannelId = "5b8932523f0c56095c70d82d",
                ChannelName = "firstchannel",
                Users = new List<User> { },
                Admin = new User { },
                Messages = new List<Message> { },
                WorkspaceId = null
            };
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.CreateChannelInWorkSpace(channel1, "5b71298a6a2e663634872a65");
            var notePosted = result as OkResult;
            Assert.Equal(200, notePosted.StatusCode);
        }
        [Fact]
        public void addusertoachannel()
        {
            Channel channel1 = new Channel()
            {
                ChannelId = "5b8932523f0c56095c70d82d",
                ChannelName = "firstchannel",
                Users = new List<User> { },
                Admin = new User { },
                Messages = new List<Message> { },
                WorkspaceId = null
            };
            User user1 = new User()
            {
                UserId = null,
                FirstName = null,
                LastName = null,
                EmailId = null,
                Designation = null
            };
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.AddUserToChannel(user1, "5b8932523f0c56095c70d82d");
            var notePosted = result as OkResult;
            Assert.Equal(200, notePosted.StatusCode);
        }
        [Fact]
        public void gettallusersinaworkspace()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.GetAllUsersInWorkspace("dummyWorkspace");
            var workspaceposted = result as OkObjectResult;
            var workspace = workspaceposted.Value as Workspace;
            Assert.Empty(workspace.Users);
        }
        [Fact]
        public void getallchannelsinworkspace()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.GetAllChannelsInWorkSpace("dummyWorkspace");
            var workspaceposted = result as OkObjectResult;
            var workspace = workspaceposted.Value as Workspace;
            Assert.Empty(workspace.Channels);
        }
        [Fact]
        public void getallchannelsofuserinworkspace()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.GetAllChannelsOfUserInWorkSpace("dummyWorkspace", null);
            var workspaceposted = result as OkObjectResult;
            var workspace = workspaceposted.Value as Workspace;
            Assert.Equal("dummyWorkspace", workspace.WorkspaceName);
        }
        [Fact]
        public void getuserbyid()
        {
            FakeChatService fakechat = new FakeChatService();
            ChatController _chatcontroller = new ChatController(fakechat);
            var result = _chatcontroller.GetUserById(null);
            var workspaceposted = result as OkObjectResult;
            var workspace = workspaceposted.Value as Workspace;
            Assert.Empty(workspace.Users);
        }
    }
}
