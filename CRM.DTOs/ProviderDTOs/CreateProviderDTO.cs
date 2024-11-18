using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.ProviderDTOs
{
    public class CreateProviderDTO
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Nombre no puede tener más de 50 caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "El campo Empresa es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Empresa no puede tener más de 50 caracteres.")]
        public string Empresa { get; set; }

        [Display(Name = "Email")]
        [MaxLength(50, ErrorMessage = "El campo Email no puede tener más de 50 caracteres.")]
        public string Email { get; set; }

        [Display(Name = "Telefono")]
        [MaxLength(50, ErrorMessage = "El campo Telefono no puede tener más de 50 caracteres.")]
        public string Phone { get; set; }
    }
}
