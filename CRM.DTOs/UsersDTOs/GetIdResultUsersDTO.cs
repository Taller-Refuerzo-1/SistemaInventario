using System.ComponentModel.DataAnnotations;

namespace CRM.DTOs.UsersDTOs
{
    public class GetIdResultUsersDTO
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}
