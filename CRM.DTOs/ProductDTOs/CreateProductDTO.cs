using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo Nombre no puede tener más de 50 caracteres.")]
        public string Name { get; set; }


        [Display(Name = "Precio")]
        [MaxLength(10, ErrorMessage = "El campo precio no puede tener más de 255 caracteres.")]
        public double Price { get; set; }
    }
}
