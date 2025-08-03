using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkManagement.Entities
{
    public class TaskRead
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int IdTask { get; set; }
        public DateTime ReadAt { get; set; }
        public TaskItem Task { get; set; }
    }
}
