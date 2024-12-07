using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CustomerDTOs;
using CRM.DTOs.ProductDTOs;

namespace CRM.API.Endpoints
{
    public static class ProductEndpoint
    {
        public static void AddProductEndpoints(this WebApplication app)
        {

            app.MapPost("/product/search", async (SearchQueryProductDTO productDTO, ProductDAL productDAL) =>
            {
                // Crear un objeto 'Customer' a partir de los datos proporcionados
                var product = new Product
                {
                    Name = productDTO.Name_Like != null ? productDTO.Name_Like : string.Empty,
                };

                // Inicializar una lista de clientes y una variable para contar las filas
                var products = new List<Product>();
                int countRow = 0;

                // Verificar si se debe enviar la cantidad de filas
                if (productDTO.SendRowCount == 2)
                {
                    // Realizar una búsqueda de clientes y contar las filas
                    products = await productDAL.Search(product, skip: productDTO.Skip, take: productDTO.Take);
                    if (products.Count > 0)
                        countRow = await productDAL.CountSearch(product);
                }
                else
                {
                    // Realizar una búsqueda de clientes sin contar las filas
                    products = await productDAL.Search(product, skip: productDTO.Skip, take: productDTO.Take);
                }

                // Crear un objeto 'SearchResultCustomerDTO' para almacenar los resultados
                var productResult = new SearchResultProductDTO
                {
                    Data = new List<SearchResultProductDTO.ProductDTO>(),
                    CountRow = countRow
                };

                // Mapear los resultados a objetos 'CustomerDTO' y agregarlos al resultado
                products.ForEach(s =>
                {
                    productResult.Data.Add(new SearchResultProductDTO.ProductDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Price = s.Price,
                    });
                });

                // Devolver los resultados
                return productResult;
            });

            // Endpoint para obtener un producto por ID
            app.MapGet("/product/{id}", async (int id, ProductDAL productDAL) =>
            {
                var product = await productDAL.GetById(id);

                if (product == null)
                    return Results.NotFound();

                var productResult = new GetIdResultProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                };

                return Results.Ok(productResult);
            });

            // Endpoint para crear un nuevo producto
            app.MapPost("/product", async (CreateProductDTO productDTO, ProductDAL productDAL) =>
            {
                // Validamos los datos de entrada
                if (string.IsNullOrWhiteSpace(productDTO.Name) || productDTO.Price <= 0)
                    return Results.BadRequest("Invalid product data.");

                var product = new Product
                {
                    Name = productDTO.Name,
                    Price = productDTO.Price
                };

                int result = await productDAL.Create(product);
                return result > 0 ? Results.Ok(result) : Results.StatusCode(500);
            });

            // Endpoint para editar un producto existente
            app.MapPut("/product", async (EditProductDTO productDTO, ProductDAL productDAL) =>
            {
                // Validamos los datos de entrada
                if (productDTO.Id <= 0 || string.IsNullOrWhiteSpace(productDTO.Name) || productDTO.Price <= 0)
                    return Results.BadRequest("Invalid product data.");

                var product = new Product
                {
                    Id = productDTO.Id,
                    Name = productDTO.Name,
                    Price = productDTO.Price
                };

                int result = await productDAL.Edit(product);
                return result > 0 ? Results.Ok(result) : Results.StatusCode(500);
            });

            // Endpoint para eliminar un producto por ID
            app.MapDelete("/product/{id}", async (int id, ProductDAL productDAL) =>
            {
                // Validamos que el ID sea válido
                if (id <= 0)
                    return Results.BadRequest("Invalid product ID.");

                int result = await productDAL.Delete(id);
                return result > 0 ? Results.NoContent() : Results.StatusCode(500);
            });
        }
    }
}
