using System.ComponentModel.DataAnnotations;

namespace collabBackend.Models
{
    public class assingTaskDTO
    {
        [Required(ErrorMessage = "The taskID field is required.")]
        public Guid taskID {  get; set; }
        [Required(ErrorMessage = "The userId field is required.")]

        public Guid userId { get; set; }
    }
}
