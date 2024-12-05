using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.ProductDTOs
{
    public class SearchResultProductDTO
    {
        public int CountRow { get; set; }
        public List<ProductDTO> data { get; set; }
        public class ProductDTO
        {
            public int Id { get; set; }

            [Display(Name = "Nombre")]
            public string Name { get; set; }

            [Display(Name = "Price")]
            public double Price { get; set; }

            public int Skip { get; set; }

            [Display(Name = "CantReg X Pagina")]
            public int Take { get; set; }

        }
    }
}
