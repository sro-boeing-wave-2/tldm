using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTMService.Models
{
    public class DummyUserWorkspace
    {
        public string UserId { get; set; }
        public DummyUserAccount UserAccount { get; set; }
        public string WorkspaceId { get; set; }
        public DummyWorkspace Workspace { get; set; }
    }
}
