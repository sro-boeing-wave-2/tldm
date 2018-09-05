using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTMService.Models
{
    public class DummyUserAccount
    {
        public DummyUserAccount()
        {
            this.Workspaces = new List<DummyWorkspace>();
            this.UserWorkspaces = new List<DummyUserWorkspace>();
        }
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }

        public bool IsVerified { get; set; }

        public List<DummyWorkspace> Workspaces { get; set; }
        public List<DummyUserWorkspace> UserWorkspaces { get; set; }
    }
}
