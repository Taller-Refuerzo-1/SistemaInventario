using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.UsersDTOs
{
    public class CreateUsersDTO
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Nombre no puede tener más de 50 caracteres.")]
        public string Name { get; set; }
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Apellido no puede tener más de 50 caracteres.")]
        public string LastName { get; set; }
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Correo no puede tener más de 50 caracteres.")]
        public string Email { get; set; }
        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El campo Telefono es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Telefono no puede tener más de 50 caracteres.")]
        public string Phone { get; set; }
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Contraseña no puede tener más de 50 caracteres.")]
        public string Password { get; set; }
    }

}
