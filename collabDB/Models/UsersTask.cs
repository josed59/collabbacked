using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collabDB.Models
{
    public class UsersTask
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool? UserTest { get; set; }
        public int TaskSizeId { get; set; }
        public TaskSize TaskSize { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
        public int TaskStateId { get; set; }
        public TaskState TaskState { get; set; }
        public DateTime? QaDateFinished { get; set; }
        public DateTime? CloseDate { get; set; }

        public ICollection<TaskChangeLogs> TaskChangeLogs { get; set; }



    }
}
