using System.ComponentModel.DataAnnotations;

namespace collabBackend.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El campo Nombre solo debe contener letras.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo Correo es requerido.")]
        [EmailAddress(ErrorMessage = "El campo Correo no es válido.")]
        public string Email { get; set; }
        
        public int UserType { get; set; }

    }
}
