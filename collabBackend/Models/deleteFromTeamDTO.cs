using System.ComponentModel.DataAnnotations;

namespace collabBackend.Models
{
    public class deleteFromTeamDTO
    {
        [Required(ErrorMessage = "The TeamId field is required.")]
        public int TeamId { get; set; }
        [Required(ErrorMessage = "The UserId field is required.")]
        public Guid UserId { get; set; }
    }
}
