using System.ComponentModel.DataAnnotations;

namespace CRM.DTOs.CategoryDTOs
{
    public class EditCategoryDTO
    {
        public EditCategoryDTO(GetIdResultCategoryDTO getIdResultCategoryDTO)
        {
            Id = getIdResultCategoryDTO.Id;
            Name = getIdResultCategoryDTO.Name;
      
        }
        public EditCategoryDTO()
        {
            Name = string.Empty;
        }
        [Required(ErrorMessage = "El campo Id es obligatorio.")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Nombre no puede tener más de 50 caracteres.")]
        public string Name { get; set; }

    }
}
