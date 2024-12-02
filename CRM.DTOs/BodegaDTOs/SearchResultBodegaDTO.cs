using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.BodegaDTOs
{
    public class SearchResultBodegaDTO
    {
        public int CountRow { get; set; }
        public List<BodegaDTO> data { get; set; }
        public class BodegaDTO
        {
            public int Id { get; set; }

            [Display(Name = "Nombre")]
            public string Name { get; set; }

            [Display(Name = "Direccion")]
            public string Address { get; set; }

            [Display(Name = "Telefono")]
            public string? Telephone { get; set; }

            [Display(Name = "Email")]
            public string Email { get; set; }
        }
    }
}
