using CRM.API.Models.DAL;
using CRM.DTOs.SucursalDTOs;
using CRM.DTOs.UsersDTOs;

namespace CRM.API.Endpoints
{
    public static class SucursalEndpoint
    {
        public static void AddSucursalEndpoint(this WebApplication app)
        {
            // GET: LISTADO
            app.MapGet("/api/Sucursal", async (SucursalDAL _sucursalDAL) =>
            {
                // DTO a Retornar:
                Registrados_Sucursal registrados = await _sucursalDAL.Obtener_Todos();

                return registrados;


            });

             
            // GET: REGISTRO
            app.MapGet("/api/Sucursal/{id}", async (int id, SucursalDAL _sucursalDAL) =>
            {

                Obtener_PorID? Objeto_Obtenido = await _sucursalDAL.Obtener_PorId(id);

                if (Objeto_Obtenido == null)
                {
                    return Results.NotFound("Registro No Existente.");
                }

                return Results.Ok(Objeto_Obtenido);
            });


            // POST: CREAR
            app.MapPost("/api/Sucursal", async (Crear_Sucursal crear_Sucursal, SucursalDAL _sucursalDAL) =>
            {
                int Respuesta = await _sucursalDAL.Crear(crear_Sucursal);

                if (Respuesta > 0)
                {
                    return Results.Ok("Sucursal Creada Correctamente");
                }
                else
                {
                    return Results.NotFound("Error El Registro No Existe.");
                }
            });


            // PUT: EDITAR
            app.MapPut("/api/Sucursal", async (Editar_Sucursal editar_Sucursal, SucursalDAL _sucursalDAL) =>
            {
                int Respuesta = await _sucursalDAL.Editar(editar_Sucursal);

                if (Respuesta > 0)
                {
                    return Results.Ok("Sucursal Editada Correctamente");
                }
                else
                {
                    return Results.NotFound("Error El Registro No Existe.");
                }
            });

            // DELETE: ELIMINAR
            app.MapDelete("/api/Sucursal/{id}", async (int id, SucursalDAL _sucursalDAL) =>
            {
                int Respuesta = await _sucursalDAL.Eliminar(id);

                if (Respuesta > 0)
                {
                    return Results.Ok("Sucursal Eliminada Correctamente");
                }
                else
                {
                    return Results.NotFound("Error El Registro No Existe.");
                }
            });

        }

    }
}
