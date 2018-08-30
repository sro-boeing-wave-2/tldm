using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tryingWithAngularChat.Hubs;

namespace tryingWithAngularChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        ChatHub chathub;
        [HttpPost]
        public async Task Post([FromBody] string user,string msg) {
           await chathub.SendMessage(user, msg);
        }
    }
}