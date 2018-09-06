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
        public IActionResult CreateWorkspace([FromBody] DummyWorkspace workspace) // frombody workspace object or string name
        {
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
                return NotFound();
            }
            return new ObjectResult(Workspace);
        }

        // deleting a workspace by id 
        [HttpDelete]
        [Route("workspaces/{id}")]
        public IActionResult Delete(string id)
        {
            var workspaceToDelete = iservice.GetWorkspaceById(id);
            if (workspaceToDelete == null)
            {
                return NotFound();
            }

            iservice.DeleteWorkspace(workspaceToDelete.WorkspaceId);
            return new OkResult();
        }
        // deleting a channel by id and workspacename
        [HttpDelete]
        [Route("workspaces/channels/{id:length(24)}")]
        public IActionResult DeleteChannelByIdAndWorkspaceName(string id)
        {
            var ChannelToDelete = iservice.GetChannelById(id);
            if (ChannelToDelete == null)
            {
                return NotFound();
            }

            iservice.DeleteChannel(ChannelToDelete.ChannelId);
            return new OkResult();
        }
        // deleting a user from channel
        [HttpDelete]
        [Route("workspaces/channels/{channelId:length(24)}/{emailId}")]
        public IActionResult DeleteuserFromChannel(string channelId, string emailId)
        {
            var ChannelToDelete = iservice.GetChannelById(channelId);
            if (ChannelToDelete == null)
            {
                return NotFound();
            }

            iservice.DeleteUserFromChannel(emailId,channelId);
            return new OkResult();
        }


        // creating a Channel
        [HttpPut]
        [Route("workspaces/{id}")]
        public IActionResult CreateChannelInWorkSpace([FromBody] Channel channel, string id) // frombody workspace object or string name
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            iservice.CreateChannel(channel, id);
            return new ObjectResult(channel);
        }
        // Adding a user to a channel
        [HttpPut]
        [Route("workspaces/channel/{channelId}")]
        public IActionResult AddUserToChannel([FromBody] User user, string ChannelId) // frombody workspace object or string name
        {
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
        public IActionResult AddUserToWorkspace([FromBody] DummyUserAccount user,string workspaceName) // frombody workspace object or string name
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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