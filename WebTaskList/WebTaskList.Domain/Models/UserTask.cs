using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTaskList.Domain.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public bool Complete { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
