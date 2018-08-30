using System;
using System.Threading.Tasks;
using tryingWithAngularChat.Models;
using tryingWithAngularChat.Services;
using Xunit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace XUnitTesttryingWithAngularChat
{   

    public class UnitTest1
    {
        public class FakeChatService : IChatService
        {


            // workspace related task
            public Task<Workspace> CreateWorkspace(string workspaceName)
            {
                Workspace workspace1 = new Workspace()
                {
                    workspaceId = 1,
                    workspaceName = "dummyWorkspace"
                };
                return Task.FromResult(workspace1);
            }

            public Task DeleteWorkspace(string workspaceName)
            {
                Workspace workspace1 = new Workspace()
                {
                    workspaceId = 1,
                    workspaceName = "dummyWorkspace"
                };

                return Task.FromResult(workspace1);
            }

            public Task<List<Workspace>> GetAllWorkspaces()
            {
                List<Workspace> w = new List<Workspace>
                {
                    new Workspace()
                     {
                    workspaceId = 1,
                    workspaceName = "dummyWorkspace"
                },
            };
                return Task.FromResult(w);
            }

            public Task<Workspace> GetWorkspaceByName(string workspaceName)
            {
                Workspace workspace1 = new Workspace()
                {
                    workspaceId = 1,
                    workspaceName = "dummyWorkspace"
                };
                return Task.FromResult(workspace1);
            }

            public Task<Workspace> UpdateWorkspaceByName(string workspaceName)
            {
                Workspace workspace1 = new Workspace()
                {
                    workspaceId = 1,
                    workspaceName = "dummyWorkspace"
                };
                return Task.FromResult(workspace1);
            }

            // channel related task
            public Task<Channel> CreateChannel(Channel channel, string workspaceName)
            {
                Channel channel1 = new Channel()
                {
                    channelId = 1,
                    channelName = "dummychannel"
                };

                return Task.FromResult(channel1);
            }

            public Task DeleteChannel(int channelId, string workspaceName)
            {
                Channel channel1 = new Channel()
                {
                    channelId = 1,
                    channelName = "dummychannel"
                };

                return Task.FromResult(channel1);
            }

            public Task<Channel> UpdateChannel(Channel channel)
            {
                Channel channel1 = new Channel()
                {
                    channelId = 1,
                    channelName = "dummychannel"
                };
                return Task.FromResult(channel1);
            }

            public Task<List<Channel>> GetChannelByuserIDandWorkspaceName(int userId, string workspaceName)
            {
                List<Channel> c = new List<Channel>
                { new Channel()
                    {
                        channelId = 1,
                        channelName = "dummychannel"
                    }
                };
                return Task.FromResult(c);
                
            }

            public Task AddUserToChannel(string userName, int channelId)
            {
                Channel channel1 = new Channel()
                {
                    channelId = 1,
                    channelName = "dummychannel"
                };
                return Task.FromResult(channel1);

            }

            public Task DeleteUserFromChannel(string userName, int channelId)
            {
                Channel channel1 = new Channel()
                {
                    channelId = 1,
                    channelName = "dummychannel"
                };
                return Task.FromResult(channel1);

            }

            public Task<List<Message>> GetMessagesInChannel(int channelId, string workspaceName)
            {
                List<Message> messages = new List<Message>()
                {
                    new Message()
                            {
                                messageId = 1,
                                messageBody = "hey",
                                isStarred = false,
                                timeStamp ="timestamp"
                            }
                };

                return Task.FromResult(messages);

            }

            // user related task
            public Task<List<User>> GetAllUsersInWorkspace(string workspaceName)
            {
                List<User> users = new List<User>()
                {
                    new User
                    {
                    userId = 1,
                    username = "dummyuser",
                    firstName = "dummy",
                    lastName = "jain",
                    designation = "dummyjob",
                    workspaceName = "dummyWorkspace"
                    }
                };
                return Task.FromResult(users);
            }

            public Task AddUserToWorkspace(string workspaceName, User user)
            {
                User user1 = new User()
                {
                    userId = 1,
                    username = "dummyuser",
                    firstName = "dummy",
                    lastName = "jain",
                    designation = "dummyjob",
                    workspaceName = "dummyWorkspace"
                };
                return Task.FromResult(user1);
            }

            public Task DeleteUserFromWorkspace(string workspaceName, int userId)
            {
                User user1 = new User()
                {
                    userId = 1,
                    username = "dummyuser",
                    firstName = "dummy",
                    lastName = "jain",
                    designation = "dummyjob",
                    workspaceName = "dummyWorkspace"
                };
                return Task.FromResult(user1);
            }

            public Task UpdateUserInWorkspace(User user)
            {
                 User user1 = new User()
                {
                    userId = 1,
                    username = "dummyuser",
                    firstName = "dummy",
                    lastName = "jain",
                    designation = "dummyjob",
                    workspaceName = "dummyWorkspace"
                };
                return Task.FromResult(user1);
            }

            //Message related task
            public Task DeleteMessageInChannel(string workspaceName, int channelId, int messageId)
            {
                Channel channel1 = new Channel()
                {
                    channelId = 1,
                    channelName = "dummychannel"
                };
                return Task.FromResult(channel1);
            }
        }

        // 
        public Workspace workspace1 = new Workspace()
        {
            workspaceId = 1,
            workspaceName = "dummyWorkspace"
        };

        public Channel channel1 = new Channel()
        {
            channelId = 1,
            channelName = "dummychannel"
        };

        public User user1 = new User()
        {
            userId = 1,
            username = "dummyuser",
            firstName = "dummy",
            lastName = "jain",
            designation = "dummyjob",
            workspaceName = "dummyWorkspace"
        };

        public Message message1 = new Message()
        {
            messageId = 1,
            messageBody = "hey",
            isStarred = false,
            timeStamp ="timestamp"
        };

        [Fact]
        public void Test1()
        {

        }
    }
}
