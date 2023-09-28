using System.ComponentModel.DataAnnotations;

namespace collabBackend.Models
{
    public class assingMassiveTaskDTO
    {
        [Required(ErrorMessage = "The taskIds field is required.")]
        public List<Guid> taskIds {  get; set; }
        [Required(ErrorMessage = "The userId field is required.")]

        public Guid userId { get; set; }
    }
}
