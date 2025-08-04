using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkManagement.Entities
{
    public class TaskAssignee
    {
        public int IdTaskItem { get; set; }
        public TaskItem TaskItem { get; set; }

        public int UserId { get; set; }
    }
}
