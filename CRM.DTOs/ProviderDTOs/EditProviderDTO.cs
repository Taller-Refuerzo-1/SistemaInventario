using System.ComponentModel.DataAnnotations;

namespace CRM.DTOs.ProviderDTOs
{
    public class EditProviderDTO
    {
        public EditProviderDTO(GetIdResultProviderDTO get)
        {
            Id = get.Id;
            Name = get.Name;
            Empresa = get.Empresa;
            Email = get.Email;
            Phone = get.Phone;
        }
        public EditProviderDTO()
        {
            Name = string.Empty;
            Empresa = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }

        [Required(ErrorMessage = "El campo Id es obligatorio.")]
        public int Id { get; set; }

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
