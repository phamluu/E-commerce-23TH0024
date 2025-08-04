using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkManagement.Entities
{
    public class ProjectMember
    {
        public int IdProject { get; set; }
        public Project Project { get; set; }
        public int UserId { get; set; }
    }
}
