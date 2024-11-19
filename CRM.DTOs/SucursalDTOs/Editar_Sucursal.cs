using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace CRM.DTOs.SucursalDTOs
{
    public class Editar_Sucursal
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese El Nombre De La Sucursal.")]
        public string Nombre { get; set; } 

        [Required(ErrorMessage = "Ingrese La Direccion De La Sucursal.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Ingrese El Telefono De La Sucursal.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Ingrese La Cantidad de Empleados En La Sucursal.")]
        public int Empleados { get; set; }
    }
}
