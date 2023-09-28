using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace collabBackend.Models
{
    public class TeamMemberModel
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The Name field should only contain letters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The TeamId field is required.")]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "The UserType field is required.")]
        public int UserType { get; set; }

        [JsonIgnore]
        public string Userid { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

    }
}
