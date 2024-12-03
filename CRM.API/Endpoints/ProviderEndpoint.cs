using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CustomerDTOs;
using CRM.DTOs.ProviderDTOs;

namespace CRM.API.Endpoints
{
    public static class ProviderEndpoint
    {
        // Método para configurar los endpoints relacionados con los proveedores
        public static void AddProviderEndpoints(this WebApplication app)
        {
            // Configurar un endpoint de tipo POST para buscar proveedores
            app.MapPost("/Provider/search", async (SearchQueryProviderDTO providerDTO, ProvidersDAL prov) =>
            {
                // Crear un objeto 'Providers' a partir de los datos proporcionados
                var providers = new Providers
                {
                    Name = providerDTO.Name_Like != null ? providerDTO.Name_Like : string.Empty,
                    Empresa = providerDTO.Empresa_Like != null ? providerDTO.Empresa_Like : string.Empty
                };

                // Inicializar una lista de proveedores y una variable para contar las filas
                var Providers = new List<Providers>();
                int countRow = 0;

                // Verificar si se debe enviar la cantidad de filas
                if (providerDTO.SendRowCount == 2)
                {
                    // Realizar una búsqueda de proveedores y contar las filas
                    Providers = await prov.Search(providers, skip: providerDTO.Skip, take: providerDTO.Take);
                    if (Providers.Count > 0)
                        countRow = await prov.CountSearch(providers);
                }
                else
                {
                    // Realizar una búsqueda de proveedores sin contar las filas
                    Providers = await prov.Search(providers, skip: providerDTO.Skip, take: providerDTO.Take);
                }

                // Crear un objeto 'SearchResultProviderDTO' para almacenar los resultados
                var providerResult = new SearchResultProviderDTO
                {
                    Data = new List<SearchResultProviderDTO.ProviderDTO>(),
                    CountRow = countRow
                };

                // Mapear los resultados a objetos 'ProviderDTO' y agregarlos al resultado
                Providers.ForEach(s => {
                    providerResult.Data.Add(new SearchResultProviderDTO.ProviderDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Empresa = s.Empresa,
                        Email = s.Email,
                        Phone = s.Phone
                    });
                });

                // Devolver los resultados
                return providerResult;
            });

            // Configurar un endpoint de tipo GET para obtener un proveedor por ID
            app.MapGet("/Provider/{id}", async (int id, ProvidersDAL get) =>
            {
                // Obtener un cliente por ID
                var waza = await get.GetById(id);

                // Crear un objeto 'GetIdResultProviderDTO' para almacenar el resultado
                var providerResult = new GetIdResultProviderDTO
                {
                    Id = waza.Id,
                    Name = waza.Name,
                    Empresa = waza.Empresa,
                    Email = waza.Email,
                    Phone = waza.Phone
                };

                // Verificar si se encontró el proveedor y devolver la respuesta correspondiente
                if (providerResult.Id > 0)
                    return Results.Ok(providerResult);
                else
                    return Results.NotFound(providerResult);
            });

            // Configurar un endpoint de tipo POST para crear un nuevo proveedor
            app.MapPost("/Provider", async (CreateProviderDTO createProviderDTO, ProvidersDAL providersDAL) =>
            {
                var providers = new Providers
                {
                    Name = createProviderDTO.Name,
                    Empresa = createProviderDTO.Empresa,
                    Email = createProviderDTO.Email,
                    Phone = createProviderDTO.Phone
                };

                // Intentar crear el proveedor y devolver el resultado correspondiente
                int result = await providersDAL.Create(providers);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Configurar un endpoint de tipo PUT para editar un proveedor existente
            app.MapPut("/Provider", async (EditProviderDTO customerDTO, ProvidersDAL customerDAL) =>
            {
                // Crear un objeto 'proveedor' a partir de los datos proporcionados
                var customer = new Providers
                {
                    Id = customerDTO.Id,
                    Name = customerDTO.Name,
                    Empresa = customerDTO.Empresa,
                    Email = customerDTO.Email,
                    Phone = customerDTO.Phone
                };

                // Intentar editar el proveedor y devolver el resultado correspondiente
                int result = await customerDAL.Edit(customer);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Configurar un endpoint de tipo DELETE para eliminar un proveedor por ID
            app.MapDelete("/Provider/{id}", async (int id, ProvidersDAL providersDAL) =>
            {
                // Intentar eliminar el cliente y devolver el resultado correspondiente
                int result = await providersDAL.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });
        }
    }
}
