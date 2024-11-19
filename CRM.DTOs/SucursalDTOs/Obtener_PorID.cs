using System;


namespace CRM.DTOs.SucursalDTOs
{
    public class Obtener_PorID
    { 
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int Empleados { get; set; }
    }
}
