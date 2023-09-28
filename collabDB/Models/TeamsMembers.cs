using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collabDB.Models
{
    public class TeamsMembers
    {
        public int TeamsMembersId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int TeamId { get; set; }
        public Teams Teams { get; set; }
    }
}
