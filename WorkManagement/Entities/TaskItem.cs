using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkManagement.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public int Status { get; set; }

        [ForeignKey("Project")]
        public int IdProject{ get; set; }

        public Project Project { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? SortOrder { get; set; }
        public ICollection<TaskRead> TaskReads { get; set; } = new List<TaskRead>();
    }
}
