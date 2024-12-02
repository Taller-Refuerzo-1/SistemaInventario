using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CompanyDTOs;

namespace CRM.API.Endpoints
{
        public static class CompanyEndpoint
        {
            // Método para configurar los endpoints relacionados con las empresas
            public static void AddCompanyEndpoints(this WebApplication app)
            {
                // Configurar un endpoint de tipo POST para buscar empresas
                app.MapPost("/company/search", async (SearchQueryCompanyDTO companyDTO, CompanyDAL companyDAL) =>
                {
                    // Crear un objeto 'Company' a partir de los datos proporcionados
                    var company = new Company
                    {
                        Name = companyDTO.Name_Like != null ? companyDTO.Name_Like : string.Empty
                    };

                    // Inicializar una lista de empresas y una variable para contar las filas
                    var companies = new List<Company>();
                    int countRow = 0;

                    // Verificar si se debe enviar la cantidad de filas
                    if (companyDTO.SendRowCount == 2)
                    {
                        // Realizar una búsqueda de empresas y contar las filas
                        companies = await companyDAL.Search(company, skip: companyDTO.Skip, take: companyDTO.Take);
                        if (companies.Count > 0)
                            countRow = await companyDAL.CountSearch(company);
                    }
                    else
                    {
                        // Realizar una búsqueda de empresas sin contar las filas
                        companies = await companyDAL.Search(company, skip: companyDTO.Skip, take: companyDTO.Take);
                    }

                    // Crear un objeto 'SearchResultCompanyDTO' para almacenar los resultados
                    var companyResult = new SearchResultCompanyDTO
                    {
                        data = new List<SearchResultCompanyDTO.CompanyDTO>(),
                        CountRow = countRow
                    };

                    // Mapear los resultados a objetos 'CompanyDTO' y agregarlos al resultado
                    companies.ForEach(s =>
                    {
                        companyResult.data.Add(new SearchResultCompanyDTO.CompanyDTO
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Address = s.Address,
                            Telephone = s.Telephone,
                            Email = s.Email
                        });
                    });

                    // Devolver los resultados
                    return companyResult;
                });

                // Configurar un endpoint de tipo GET para obtener una empresa por ID
                app.MapGet("/company/{id}", async (int id, CompanyDAL companyDAL) =>
                {
                    // Obtener una empresa por ID
                    var company = await companyDAL.GetById(id);

                    // Crear un objeto 'GetIdResultCompanyDTO' para almacenar el resultado
                    var companyResult = new GetIdResultCompanyDTO
                    {
                        Id = company.Id,
                        Name = company.Name,
                        Address = company.Address,
                        Telephone = company.Telephone,
                        Email = company.Email
                    };

                    // Verificar si se encontró la empresa y devolver la respuesta correspondiente
                    if (companyResult.Id > 0)
                        return Results.Ok(companyResult);
                    else
                        return Results.NotFound(companyResult);
                });

                // Configurar un endpoint de tipo POST para crear una nueva empresa
                app.MapPost("/company", async (CreateCompanyDTO companyDTO, CompanyDAL companyDAL) =>
                {
                    // Crear un objeto 'Company' a partir de los datos proporcionados
                    var company = new Company
                    {
                        Name = companyDTO.Name,
                        Address = companyDTO.Address,
                        Telephone = companyDTO.Telephone,
                        Email = companyDTO.Email
                    };

                    // Intentar crear la empresa y devolver el resultado correspondiente
                    int result = await companyDAL.Create(company);
                    if (result != 0)
                        return Results.Ok(result);
                    else
                        return Results.StatusCode(500);
                });

                // Configurar un endpoint de tipo PUT para editar una empresa existente
                app.MapPut("/company", async (EditCompanyDTO companyDTO, CompanyDAL companyDAL) =>
                {
                    // Crear un objeto 'Company' a partir de los datos proporcionados
                    var company = new Company
                    {
                        Id = companyDTO.Id,
                        Name = companyDTO.Name,
                        Address = companyDTO.Address,
                        Telephone = companyDTO.Telephone,
                        Email = companyDTO.Email
                    };

                    // Intentar editar la empresa y devolver el resultado correspondiente
                    int result = await companyDAL.Edit(company);
                    if (result != 0)
                        return Results.Ok(result);
                    else
                        return Results.StatusCode(500);
                });

                // Configurar un endpoint de tipo DELETE para eliminar una empresa por ID
                app.MapDelete("/company/{id}", async (int id, CompanyDAL companyDAL) =>
                {
                    // Intentar eliminar la empresa y devolver el resultado correspondiente
                    int result = await companyDAL.Delete(id);
                    if (result != 0)
                        return Results.Ok(result);
                    else
                        return Results.StatusCode(500);
                });
            }
        }

    }

