using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tryingWithAngularChat.Hubs;
using tryingWithAngularChat.Models;
using tryingWithAngularChat.Services;

namespace tryingWithAngularChat.Controllers
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
        public IActionResult CreateWorkspace([FromBody] string workspace) // frombody workspace object or string name
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            iservice.CreateWorkspace(workspace);
            return new ObjectResult(workspace);
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

        // deleting a workspace
        [HttpDelete]
        [Route("workspaces")]
        public IActionResult DeletingWorkspace([FromQuery] string workspaceName)
        {
            var workspace = iservice.GetWorkspaceByName(workspaceName);
            if (workspace == null)
            {
                return NotFound();
            }
            iservice.DeleteWorkspace(workspaceName);
            return new OkResult();
        }

        //[HttpPost]
        //[Route("member")]
        //public IActionResult AddingMemberToChannel([FromQuery] string username,[FromQuery] int channelId)
        //{

        //}

    }
}