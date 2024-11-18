using System.ComponentModel.DataAnnotations;

namespace CRM.DTOs.UsersDTOs
{
    public class EditUsersDTO
    {
        // Constructor que inicializa el DTO a partir de un modelo de entidad Users
        public EditUsersDTO(GetIdResultUsersDTO user)
        {
            Id = user.Id;
            Name = user.Name;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
            Password = user.Password;
        }

        // Constructor vacío
        public EditUsersDTO()
        {
            Name = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }

        [Required(ErrorMessage = "El campo Id es obligatorio.")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Nombre no puede tener más de 50 caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Apellido no puede tener más de 50 caracteres.")]
        public string LastName { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "El campo Correo Electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del Correo Electrónico es inválido.")]
        [MaxLength(100, ErrorMessage = "El campo Correo Electrónico no puede tener más de 100 caracteres.")]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo Teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El formato del Teléfono es inválido.")]
        [MaxLength(15, ErrorMessage = "El campo Teléfono no puede tener más de 15 caracteres.")]
        public string Phone { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(100, ErrorMessage = "El campo Contraseña no puede tener más de 100 caracteres.")]
        public string? Password { get; set; }
    }
}
