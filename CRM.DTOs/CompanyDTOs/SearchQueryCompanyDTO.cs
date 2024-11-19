using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.CompanyDTOs
{
    public class SearchQueryCompanyDTO
    {
        [Display(Name = "Nombre")]
        public string? Name_Like { get; set; }
        [Display(Name = "Direccion")]
        public string? Address_Like { get; set; }
        [Display(Name = "Telefono")]
        public string? Telephone_Like { get; set; }
        [Display(Name = "Email")]
        public string? Email_Like { get; set; }
        [Display(Name = "Pagina")]
        public int Skip { get; set; }
        [Display(Name = "CantReg X Pagina")]
        public int Take { get; set; }
        /// <summary>
        /// 1 = No se cuenta los resultados de la busqueda
        /// 2= Cuenta los resultados de la busqueda
        /// </summary>
        public byte SendRowCount { get; set; }
    }
}

