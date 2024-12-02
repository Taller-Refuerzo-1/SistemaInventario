using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CategoryDTOs;

namespace CRM.API.Endpoints
{
    public static class CategoryEndpoint
    {
        // Método para configurar los endpoints relacionados con las empresas 
        public static void AddCategoryEndpoints(this WebApplication app)
        {
            // Configurar un endpoint de tipo POST para buscar empresas
            app.MapPost("/category/search", async (SearchQueryCategoryDTOs categoryDTO, CategoryDAL categoryDAL) =>
            {
                // Crear un objeto 'Category' a partir de los datos proporcionados
                var category = new Category
                {
                    Name = categoryDTO.Name_Like != null ? categoryDTO.Name_Like : string.Empty
                };

                // Inicializar una lista de empresas y una variable para contar las filas
                var companies = new List<Category>();
                int countRow = 0;

                // Verificar si se debe enviar la cantidad de filas
                if (categoryDTO.SendRowCount == 2)
                {
                    // Realizar una búsqueda de empresas y contar las filas
                    companies = await categoryDAL.Search(category, skip: categoryDTO.Skip, take: categoryDTO.Take);
                    if (companies.Count > 0)
                        countRow = await categoryDAL.CountSearch(category);
                }
                else
                {
                    // Realizar una búsqueda de empresas sin contar las filas
                    companies = await categoryDAL.Search(category, skip: categoryDTO.Skip, take: categoryDTO.Take);
                }

                // Crear un objeto 'SearchResultCategoryDTO' para almacenar los resultados
                var categoryResult = new SearchResultCategoryDTO
                {
                    Data = new List<SearchResultCategoryDTO.CategoryDTO>(),
                    CountRow = countRow
                };

                // Mapear los resultados a objetos 'CategoryDTO' y agregarlos al resultado
                companies.ForEach(s =>
                {
                    categoryResult.Data.Add(new SearchResultCategoryDTO.CategoryDTO
                    {
                        Id = s.Id,
                        Name = s.Name
                    });
                });

                // Devolver los resultados
                return categoryResult;
            });

            // Configurar un endpoint de tipo GET para obtener una empresa por ID
            app.MapGet("/category/{id}", async (int id, CategoryDAL categoryDAL) =>
            {
                // Obtener una empresa por ID
                var category = await categoryDAL.GetById(id);

                // Crear un objeto 'GetIdResultCategoryDTO' para almacenar el resultado
                var categoryResult = new GetIdResultCategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                };

                // Verificar si se encontró la empresa y devolver la respuesta correspondiente
                if (categoryResult.Id > 0)
                    return Results.Ok(categoryResult);
                else
                    return Results.NotFound(categoryResult);
            });

            // Configurar un endpoint de tipo POST para crear una nueva empresa
            app.MapPost("/category", async (CreateCategoryDTO categoryDTO, CategoryDAL categoryDAL) =>
            {
                // Crear un objeto 'Category' a partir de los datos proporcionados
                var category = new Category
                {
                    Name = categoryDTO.Name
                };

                // Intentar crear la empresa y devolver el resultado correspondiente
                int result = await categoryDAL.Create(category);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Configurar un endpoint de tipo PUT para editar una empresa existente
            app.MapPut("/category", async (EditCategoryDTO categoryDTO, CategoryDAL categoryDAL) =>
            {
                // Crear un objeto 'Category' a partir de los datos proporcionados
                var category = new Category
                {
                    Id = categoryDTO.Id,
                    Name = categoryDTO.Name
                };

                // Intentar editar la empresa y devolver el resultado correspondiente
                int result = await categoryDAL.Edit(category);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Configurar un endpoint de tipo DELETE para eliminar una empresa por ID
            app.MapDelete("/category/{id}", async (int id, CategoryDAL categoryDAL) =>
            {
                // Intentar eliminar la empresa y devolver el resultado correspondiente
                int result = await categoryDAL.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });
        }
    }
}
