using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collabDB.Models
{
    public class Teams
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<TeamsMembers> TeamsMembers { get; set; }


    }
}
