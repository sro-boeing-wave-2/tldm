using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTMService.Models
{
    public class DummyWorkspace
    {
        public DummyWorkspace()
        {
            this.UsersState = new List<DummyUserState>();
            this.Channels = new List<DummyChannel>();
            this.UserWorkspaces = new List<DummyUserWorkspace>();
        }
        public string Id { get; set; }
        public string WorkspaceName { get; set; } 
        public string PictureUrl { get; set; }
        public List<DummyChannel> Channels { get; set; }
        public List<DummyUserState> UsersState { get; set; }
        public List<DummyUserWorkspace> UserWorkspaces { get; set; }
    }
}
