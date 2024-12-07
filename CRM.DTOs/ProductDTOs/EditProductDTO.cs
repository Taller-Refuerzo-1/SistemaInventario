using CRM.DTOs.CustomerDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.ProductDTOs
{
    public class EditProductDTO
    {
        // Constructor que inicializa el DTO usando otro DTO
        public EditProductDTO(GetIdResultProductDTO getIdResultProductDTO)
        {
            Id = getIdResultProductDTO.Id;
            Name = getIdResultProductDTO.Name;
            Price = (decimal)getIdResultProductDTO.Price;
        }

        // Constructor sin parámetros para inicializar valores por defecto
        public EditProductDTO()
        {
            Name = string.Empty;
            Price = 0;
        }

        [Required(ErrorMessage = "El campo Id es obligatorio.")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Nombre no puede tener más de 50 caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo Precio es obligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "El campo Precio debe estar entre 0.01 y 999,999.99.")]
        public decimal Price { get; set; }
    }
}