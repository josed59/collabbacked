using System.ComponentModel.DataAnnotations;

namespace collabBackend.Models
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Password { get; set; }
    }
}
