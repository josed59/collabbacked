using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace collabDB.Models
{
    public class TaskState
    {

        public int TaskStateId { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsersTask>? UsersTask { get; set; }
    }
}
