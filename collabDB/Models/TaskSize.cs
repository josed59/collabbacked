using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace collabDB.Models
{
    public class TaskSize
    {
        public int TaskSizeId { get; set; }
        public string TaskDescription { get; set; }
        public int Weigth { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsersTask>? UsersTask { get; set; }

    }
}
