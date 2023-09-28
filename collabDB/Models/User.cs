using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collabDB.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public int UserTypeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public int Capacity { get; set; }
        public ICollection<TeamsMembers> TeamsMembers { get; set; }
        public ICollection<Teams> Teams { get; set; }
        public ICollection<UsersTask> UsersTasks { get; set; }
        public ICollection<TaskChangeLogs> TaskChangeLogs { get; set; }

    }
}
