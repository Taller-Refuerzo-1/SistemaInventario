using System;


namespace CRM.DTOs.SucursalDTOs
{
    public class Registrados_Sucursal
    {
        public class Sucursal 
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Direccion { get; set; }
            public string Telefono { get; set; }
            public int Empleados { get; set; }
        }

        public List<Sucursal> Lista_Sucursales { get; set; }

        public Registrados_Sucursal()
        {
            Lista_Sucursales = new List<Sucursal>();
        }
    }
}
