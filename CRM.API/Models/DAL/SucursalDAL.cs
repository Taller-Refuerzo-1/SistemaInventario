using CRM.API.Models.EN;
using CRM.DTOs.SucursalDTOs;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Models.DAL
{
    public class SucursalDAL
    {
        private readonly CRMContext _CRMContext;

        public SucursalDAL(CRMContext cRMContext)
        {
            _CRMContext = cRMContext;
        }
         


        // OBTIENE TODOS LOS REGISTROS DE LA DB:
        public async Task<Registrados_Sucursal> Obtener_Todos()
        {
            List<Sucursal> Lista_Sucursal = await _CRMContext.Sucursal
                .ToListAsync();

            // DTO a retornar:
            Registrados_Sucursal registrados = new Registrados_Sucursal();

            foreach (Sucursal sucursal in Lista_Sucursal)
            {
                registrados.Lista_Sucursales.Add(new Registrados_Sucursal.Sucursal
                {
                    Id = sucursal.Id,
                    Nombre = sucursal.Nombre,
                    Direccion = sucursal.Direccion,
                    Telefono = sucursal.Telefono,
                    Empleados = sucursal.Empleados,
                });
            }

            return registrados;
        }


        // OBTIENE UN REGISTRO CON EL MISMO ID:
        public async Task<Obtener_PorID> Obtener_PorId(int id)
        {
            Sucursal? Objeto_Obtenido = await _CRMContext.Sucursal.FirstOrDefaultAsync(x => x.Id == id);

            if (Objeto_Obtenido == null)
            {
                return null;
            }

            // DTO a retornar:
            Obtener_PorID sucursal = new Obtener_PorID
            {
                Id = Objeto_Obtenido.Id,
                Nombre = Objeto_Obtenido.Nombre,
                Direccion = Objeto_Obtenido.Direccion,
                Telefono = Objeto_Obtenido.Telefono,
                Empleados = Objeto_Obtenido.Empleados,
            };

            return sucursal;
        }




        // RECIBE UN OBJETO Y LO GUARDA EN LA DB:
        public async Task<int> Crear(Crear_Sucursal crear_Sucursal)
        {
            // Objeto a Mapear:
            Sucursal sucursal = new Sucursal
            {
                Nombre = crear_Sucursal.Nombre,
                Direccion = crear_Sucursal.Direccion,
                Telefono = crear_Sucursal.Telefono,
                Empleados = crear_Sucursal.Empleados,
            };

            _CRMContext.Add(sucursal);

            return await _CRMContext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO MODIFICA
        public async Task<int> Editar(Editar_Sucursal editar_Sucursal)
        {
            Sucursal? Objeto_Obtenido = await _CRMContext.Sucursal
                .FirstOrDefaultAsync(x => x.Id == editar_Sucursal.Id);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            // Modificamos:
            Objeto_Obtenido.Nombre = editar_Sucursal.Nombre;
            Objeto_Obtenido.Direccion = editar_Sucursal.Direccion;
            Objeto_Obtenido.Telefono = editar_Sucursal.Telefono;
            Objeto_Obtenido.Empleados = editar_Sucursal.Empleados;

            _CRMContext.Update(Objeto_Obtenido);

            return await _CRMContext.SaveChangesAsync();
        }


        // BUSCA UN REGISTRO CON EL MISMO ID EN LA DB Y LO ELIMINA:
        public async Task<int> Eliminar(int id)
        {
            Sucursal? Objeto_Obtenido = await _CRMContext.Sucursal.FirstOrDefaultAsync(x => x.Id == id);

            if (Objeto_Obtenido == null)
            {
                return 0;
            }

            _CRMContext.Remove(Objeto_Obtenido);

            return await _CRMContext.SaveChangesAsync();
        }


    }
}
