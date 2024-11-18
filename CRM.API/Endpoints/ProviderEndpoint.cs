using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CustomerDTOs;
using CRM.DTOs.ProviderDTOs;

namespace CRM.API.Endpoints
{
    public static class ProviderEndpoint
    {
        // Método para configurar los endpoints relacionados con los clientes
        public static void AddProviderEndpoints(this WebApplication app)
        {
            // Configurar un endpoint de tipo POST para buscar clientes
            app.MapPost("/proveedor/search", async (SearchQueryProviderDTO providerDTO, ProvidersDAL prov) =>
            {
                // Crear un objeto 'Providers' a partir de los datos proporcionados
                var providers = new Providers
                {
                    Name = providerDTO.Name_Like != null ? providerDTO.Name_Like : string.Empty,
                    Empresa = providerDTO.Empresa_Like != null ? providerDTO.Empresa_Like : string.Empty
                };

                // Inicializar una lista de clientes y una variable para contar las filas
                var Providers = new List<Providers>();
                int countRow = 0;

                // Verificar si se debe enviar la cantidad de filas
                if (providerDTO.SendRowCount == 2)
                {
                    // Realizar una búsqueda de clientes y contar las filas
                    Providers = await prov.Search(providers, skip: providerDTO.Skip, take: providerDTO.Take);
                    if (Providers.Count > 0)
                        countRow = await prov.CountSearch(providers);
                }
                else
                {
                    // Realizar una búsqueda de clientes sin contar las filas
                    Providers = await prov.Search(providers, skip: providerDTO.Skip, take: providerDTO.Take);
                }

                // Crear un objeto 'SearchResultCustomerDTO' para almacenar los resultados
                var providerResult = new SearchResultProviderDTO
                {
                    Data = new List<SearchResultProviderDTO.ProviderDTO>(),
                    CountRow = countRow
                };

                // Mapear los resultados a objetos 'CustomerDTO' y agregarlos al resultado
                Providers.ForEach(s => {
                    providerResult.Data.Add(new SearchResultProviderDTO.ProviderDTO
                    {
                        Name = s.Name,
                        Empresa = s.Empresa,
                        Email = s.Email,
                        Phone = s.Phone
                    });
                });

                // Devolver los resultados
                return providerResult;
            });

            // Configurar un endpoint de tipo GET para obtener un cliente por ID
            app.MapGet("/provider/{id}", async (int id, ProvidersDAL get) =>
            {
                // Obtener un cliente por ID
                var waza = await get.GetById(id);

                // Crear un objeto 'GetIdResultCustomerDTO' para almacenar el resultado
                var providerResult = new GetIdResultProviderDTO
                {
                    Id = waza.Id,
                    Name = waza.Name,
                    Empresa = waza.Empresa,
                    Email = waza.Email,
                    Phone = waza.Phone
                };

                // Verificar si se encontró el cliente y devolver la respuesta correspondiente
                if (providerResult.Id > 0)
                    return Results.Ok(providerResult);
                else
                    return Results.NotFound(providerResult);
            });

            // Configurar un endpoint de tipo POST para crear un nuevo cliente
            app.MapPost("/provider", async (CreateProviderDTO create, Providers prov) =>
            {
                // Crear un objeto 'Customer' a partir de los datos proporcionados
                var providers = new Providers
                {
                    Name = prov.Name,
                    Empresa = prov.Empresa,
                    Email = prov.Email,
                    Phone = prov.Phone
                };

                // Intentar crear el cliente y devolver el resultado correspondiente
                int result = await customerDAL.Create(customer);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Configurar un endpoint de tipo PUT para editar un cliente existente
            app.MapPut("/customer", async (EditCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                // Crear un objeto 'Customer' a partir de los datos proporcionados
                var customer = new Customer
                {
                    Id = customerDTO.Id,
                    Name = customerDTO.Name,
                    LastName = customerDTO.LastName,
                    Address = customerDTO.Address
                };

                // Intentar editar el cliente y devolver el resultado correspondiente
                int result = await customerDAL.Edit(customer);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Configurar un endpoint de tipo DELETE para eliminar un cliente por ID
            app.MapDelete("/customer/{id}", async (int id, CustomerDAL customerDAL) =>
            {
                // Intentar eliminar el cliente y devolver el resultado correspondiente
                int result = await customerDAL.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });
        }
    }
}
