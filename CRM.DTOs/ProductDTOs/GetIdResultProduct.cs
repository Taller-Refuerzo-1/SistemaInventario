using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.ProductDTOs
{
    public class GetIdResultProductDTO
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }


        [Display(Name = "Precio")]
        public double? Price { get; set; }

    }
}
