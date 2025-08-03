using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkManagement.Enums
{
    public enum TaskItemStatus
    {
        [Display(Name = "Chưa làm")]
        Todo = 1,
        [Display(Name = "Đang làm")]
        InProgress = 2,
        [Display(Name = "Hoàn thành")]
        Done = 3
    }
}
