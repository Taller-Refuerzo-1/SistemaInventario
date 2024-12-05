using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.ProductDTOs;
using CRM.DTOs.ProuctDTOs;

namespace CRM.API.Endpoints
{
    public static class ProductEndpoint
    {
        public static void AddProductEndpoints(this WebApplication app)
        {
            // Endpoint para buscar productos
            app.MapPost("/product/search", async (SearchResultProductDTO productDTO, SearchQueryProductDTO productDTO1, ProductDAL productDAL) =>
            {
                // Inicializamos el objeto 'Product' con los datos proporcionados
                var product = new Product
                {
                    Name = productDTO.data.FirstOrDefault()?.Name ?? string.Empty,
                    Price = double.Parse(productDTO.data?.FirstOrDefault()?.Price.ToString() ?? string.Empty)
                };

                var products = new List<Product>();
                int countRow = 0;

                // Realizamos la búsqueda y, si es necesario, contamos las filas
                if (productDTO.CountRow == 2)
                {
                    products = await productDAL.Search(product, skip: productDTO1.Skip, take:productDTO1.Take);
                    if (products.Any())
                        countRow = await productDAL.CountSearch(product);
                }
                else
                {
                    products = await productDAL.Search(product, skip: productDTO1.Skip, take: productDTO1.Take);
                }

                var productResult = new SearchResultProductDTO
                {
                    data = products.Select(p => new SearchResultProductDTO.ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price
                    }).ToList(),
                    CountRow = countRow
                };

                // Retornar los resultados
                return products.Any() ? Results.Ok(productResult) : Results.NotFound();
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
