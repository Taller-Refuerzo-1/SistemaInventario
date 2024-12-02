using System.ComponentModel.DataAnnotations;

namespace CRM.DTOs.BodegaDTOs
{
    public class EditBodegaDTO
    {
        public EditBodegaDTO(GetIdResultBodegaDTO getIdResultBodegaDTO)
        {
            Id = getIdResultBodegaDTO.Id;
            Name = getIdResultBodegaDTO.Name;
            Address = getIdResultBodegaDTO.Address;

        }
        public EditBodegaDTO()
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
        [MaxLength(255, ErrorMessage = "El campo Direccion no puede tener más de 50 caracteres.")]
        public string? Address { get; set; }

        [Display(Name = "Telefono")]
        [MaxLength(10, ErrorMessage = "El campo Telefono no puede tener más de 255 caracteres.")]
        public string Telephone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El campo Email no puede tener más de 50 caracteres.")]
        public string Email { get; set; }
    }
}
