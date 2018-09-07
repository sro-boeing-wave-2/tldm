using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RTMService.Hubs;
using RTMService.Models;
using RTMService.Services;

namespace RTMService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private IChatService iservice;


        public ChatController(IChatService c)
        {
            iservice = c;
        }

        // creating a workspace
        [HttpPost]
        [Route("workspaces")]
        public IActionResult CreateWorkspace([FromBody] WorkspaceView workspace) // frombody workspace object or string name
        {
            var searchedWorkspace = iservice.GetWorkspaceById(workspace.Id);
            if(searchedWorkspace != null)
            {
                return NotFound("Workspace already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Workspace newWorkspace = iservice.CreateWorkspace(workspace);
            return new ObjectResult(newWorkspace);
        }

        // getting all the workspaces
        [HttpGet]
        [Route("workspaces")]
        public IActionResult GetAllWorkspace()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ListofWorkspace = iservice.GetAllWorkspaces();
            return new ObjectResult(ListofWorkspace);
        }
        // getting the workspace by id
        [HttpGet]
        [Route("workspaces/{id}")]
        public IActionResult GetWorkspaceById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Workspace = iservice.GetWorkspaceById(id);
            if (Workspace == null)
            {
                return NotFound("No Workspcae Found");
            }
            return new ObjectResult(Workspace);
        }

        // deleting a workspace by id 
        [HttpDelete]
        [Route("workspaces/{id}")]
        public IActionResult DeleteWorkspaceById(string id)
        {
            var workspaceToDelete = iservice.GetWorkspaceById(id);
            if (workspaceToDelete == null)
            {
                return NotFound("Workspace trying to delete not found");
            }

            iservice.DeleteWorkspace(workspaceToDelete.WorkspaceId);
            return NoContent();
        }
        // deleting a channel by id and workspacename
        [HttpDelete]
        [Route("workspaces/channels/{id}")]
        public IActionResult DeleteChannelById(string channelId)
        {
            var ChannelToDelete = iservice.GetChannelById(channelId);
            if (ChannelToDelete == null)
            {
                return NotFound("Channel trying to delete not found");
            }

            iservice.DeleteChannel(ChannelToDelete.ChannelId);
            return NoContent();
        }
        // deleting a user from channel
        [HttpDelete]
        [Route("workspaces/channels/{channelId:length(24)}/{emailId}")]
        public IActionResult DeleteuserFromChannel(string channelId, string emailId)
        {
            var ChannelToDelete = iservice.GetChannelById(channelId);
            if (ChannelToDelete == null)
            {
                return NotFound("Channel not found");
            }

            iservice.DeleteUserFromChannel(emailId,channelId);
            return NoContent();
        }
        //// deleting a user from workspace
        //[HttpDelete]
        //[Route("workspaces/channels/{channelId:length(24)}/{emailId}")]
        //public IActionResult DeleteuserFromWorkspace(User user, string workspaceName)
        //{
        //    var searchedWorkspace = iservice.GetWorkspaceByName(workspaceName);
        //    if (searchedWorkspace == null)
        //    {
        //        return NotFound("Workspace not found");
        //    }

        //    iservice.DeleteUserFromChannel(emailId, channelId);
        //    return NoContent();
        //}


        // creating a Channel
        [HttpPut]
        [Route("workspaces/{workspaceName}")]
        public IActionResult CreateChannelInWorkSpace([FromBody] Channel channel, string workspaceName) 
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            iservice.CreateChannel(channel, workspaceName);
            return new ObjectResult(channel);
        }
        // Adding a user to a channel
        [HttpPut]
        [Route("workspaces/channel/{channelId}")]
        public IActionResult AddUserToChannel([FromBody] User user, string ChannelId) 
        {
            var searchedChannel = iservice.GetChannelById(ChannelId);
            var userAlreadyAddedInChannel = searchedChannel.Users.Find(u => u.UserId == user.UserId);
            if(userAlreadyAddedInChannel != null)
            {
                return NotFound("User already added in Channel");
            }
            var searchedWorkspace = iservice.GetWorkspaceById(searchedChannel.WorkspaceId);
            var searchedUser = searchedWorkspace.Users.Find(u => u.UserId == user.UserId);
            if(searchedUser==null)
            {
                return NotFound("User is not added in Workspace. First complete onboarding process");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            iservice.AddUserToChannel(user, ChannelId);
            return new ObjectResult(user);
        }

        // getting all the users
        [HttpGet]
        [Route("user/{workspaceName}")]
        public IActionResult GetAllUsersInWorkspace(string workspaceName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ListofUsers = iservice.GetAllUsersInWorkspace(workspaceName);
            return new ObjectResult(ListofUsers);
        }

       // Adding a user to a workspace
       [HttpPut]
        [Route("workspaces/user/{workspaceName}")]
        public IActionResult AddUserToWorkspace([FromBody] UserAccountView user,string workspaceName) // frombody workspace object or string name
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var searchedWorkSpace = iservice.GetWorkspaceByName(workspaceName);
            var userAlreadyInWorkspace = searchedWorkSpace.Users.Find(u => u.UserId == user.Id);
            if(userAlreadyInWorkspace != null )
            {
                return NotFound("User already added in Workspace");
            }
            var userAdded = iservice.AddUserToWorkspace(user,workspaceName);
            return new ObjectResult(userAdded);
        }

        // getting general channel id by workspace name
        //[HttpGet]
        //[Route("workspaces/{workspaceName}")]
        //public IActionResult GetChannelIdByWorkSpaceName(string workspaceName)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    Channel channel = iservice.GetGeneralChannelIdByWorkSpaceName(workspaceName);
        //    return new ObjectResult(channel);

        //}

        // getting all channels in a workspace by workspace name
        [HttpGet]
        [Route("workspaces/channels/{workspaceName}")]
        public IActionResult GetAllChannelsInWorkSpace(string workspaceName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Channel> channels = iservice.GetAllChannelsInWorkspace(workspaceName);
            return new ObjectResult(channels);
        }


        // getting all channels a user is part of in a workspace by workspace name and emailid
        [HttpGet]
        [Route("workspaces/{workspaceName}/{emailId}")]
        public IActionResult GetAllChannelsOfUserInWorkSpace(string workspaceName, string emailId)
        {
            if(workspaceName==null || emailId== null)
            {
                return NotFound("Please enter both workspaceName and email id");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Channel> channels = iservice.GetAllUserChannelsInWorkSpace(workspaceName, emailId);
            return new ObjectResult(channels);
        }

        // get user by id
        [HttpGet]
        [Route("user/{userId}")]
        public IActionResult GetUserByEmail(string userEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = iservice.GetUserByEmail(userEmail);
            return new ObjectResult(user);
        }


    }
}