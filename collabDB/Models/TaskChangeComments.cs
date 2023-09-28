using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collabDB.Models
{
    public class TaskChangeLogs
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public Guid TaskId { get; set; }
        public UsersTask UsersTask { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public bool isUatCompleted { get; set; }    
        public Guid userid { get; set; }
        public User user { get; set; }

    }
}
