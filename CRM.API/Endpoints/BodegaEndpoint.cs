using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.BodegaDTOs;
using CRM.DTOs.CompanyDTOs;

namespace CRM.API.Endpoints
{
    public static class BodegaEndpoint
    {
        // Método para configurar los endpoints relacionados con las bodegas
        public static void AddBodegaEndpoints(this WebApplication app)
        {
            // Configurar un endpoint de tipo POST para buscar bodegas
            app.MapPost("/bodega/search", async (SearchQueryBodegaDTO bodegaDTO, BodegaDAL bodegaDAL) =>
            {
                // Crear un objeto 'Bodega' a partir de los datos proporcionados
                var bodega = new Bodega
                {
                    Name = bodegaDTO.Name_Like != null ? bodegaDTO.Name_Like : string.Empty,
                    Address = bodegaDTO.Address_Like != null ? bodegaDTO.Address_Like : string.Empty
                };

                // Inicializar una lista de bodegas y una variable para contar las filas
                var companies = new List<Bodega>();
                int countRow = 0;

                // Verificar si se debe enviar la cantidad de filas
                if (bodegaDTO.SendRowCount == 2)
                {
                    // Realizar una búsqueda de bodegas y contar las filas
                    companies = await bodegaDAL.Search(bodega, skip: bodegaDTO.Skip, take: bodegaDTO.Take);
                    if (companies.Count > 0)
                        countRow = await bodegaDAL.CountSearch(bodega);
                }
                else
                {
                    // Realizar una búsqueda de empresas sin contar las filas
                    companies = await bodegaDAL.Search(bodega, skip: bodegaDTO.Skip, take: bodegaDTO.Take);
                }

                // Crear un objeto 'SearchResultBodegaDTO' para almacenar los resultados
                var bodegaResult = new SearchResultBodegaDTO
                {
                    data = new List<SearchResultBodegaDTO.BodegaDTO>(),
                    CountRow = countRow
                };

                // Mapear los resultados a objetos 'BodegaDTO' y agregarlos al resultado
                companies.ForEach(s =>
                {
                    bodegaResult.data.Add(new SearchResultBodegaDTO.BodegaDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Address = s.Address,
                        Telephone = s.Telephone,
                        Email = s.Email
                    });
                });

                // Devolver los resultados
                return bodegaResult;
            });

            // Configurar un endpoint de tipo GET para obtener una bodega por ID
            app.MapGet("/bodega/{id}", async (int id, BodegaDAL bodegaDAL) =>
            {
                // Obtener una bodega por ID
                var bodega = await bodegaDAL.GetById(id);

                // Crear un objeto 'GetIdResultBodegaDTO' para almacenar el resultado
                var bodegaResult = new GetIdResultBodegaDTO
                {
                    Id = bodega.Id,
                    Name = bodega.Name,
                    Address = bodega.Address,
                    Telephone = bodega.Telephone,
                    Email = bodega.Email
                };

                // Verificar si se encontró la bodega y devolver la respuesta correspondiente
                if (bodegaResult.Id > 0)
                    return Results.Ok(bodegaResult);
                else
                    return Results.NotFound(bodegaResult);
            });

            // Configurar un endpoint de tipo POST para crear una nueva bodega
            app.MapPost("/bodega", async (CreateBodegaDTO bodegaDTO, BodegaDAL bodegaDAL) =>
            {
                // Crear un objeto 'Bodega' a partir de los datos proporcionados
                var bodega = new Bodega
                {
                    Name = bodegaDTO.Name,
                    Address = bodegaDTO.Address,
                    Telephone = bodegaDTO.Telephone,
                    Email = bodegaDTO.Email
                };

                // Intentar crear la bodega y devolver el resultado correspondiente
                int result = await bodegaDAL.Create(bodega);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Configurar un endpoint de tipo PUT para editar una empresa existente
            app.MapPut("/bodega", async (EditBodegaDTO bodegaDTO, BodegaDAL bodegaDAL) =>
            {
                // Crear un objeto 'Bodega' a partir de los datos proporcionados
                var bodega = new Bodega
                {
                    Id = bodegaDTO.Id,
                    Name = bodegaDTO.Name,
                    Address = bodegaDTO.Address,
                    Telephone = bodegaDTO.Telephone,
                    Email = bodegaDTO.Email
                };

                // Intentar editar la bodega y devolver el resultado correspondiente
                int result = await bodegaDAL.Edit(bodega);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Configurar un endpoint de tipo DELETE para eliminar una bodega por ID
            app.MapDelete("/bodega/{id}", async (int id, BodegaDAL bodegaDAL) =>
            {
                // Intentar eliminar la bodega y devolver el resultado correspondiente
                int result = await bodegaDAL.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });
        }
    }
}
