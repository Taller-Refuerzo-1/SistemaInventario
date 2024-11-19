using CRM.DTOs.CustomerDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.CompanyDTOs
{
    public class EditCompanyDTO
    {
        public EditCompanyDTO(GetIdResultCompanyDTO getIdResultCompanyDTO)
        {
            Id = getIdResultCompanyDTO.Id;
            Name = getIdResultCompanyDTO.Name;
            Address = getIdResultCompanyDTO.Address;

        }
        public EditCompanyDTO()
        {
            Name = string.Empty;
            Address = string.Empty;
        }
        [Required(ErrorMessage = "El campo Id es obligatorio.")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Nombre no puede tener más de 50 caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "El campo Direccion es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El campo Apellido no puede tener más de 50 caracteres.")]
        public string? Address { get; set; }

        [Display(Name = "Telefono")]
        [MaxLength(10, ErrorMessage = "El campo Telefono no puede tener más de 255 caracteres.")]
        public string Telephone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El campo Apellido no puede tener más de 50 caracteres.")]
        public string Email { get; set; }

    }
}
