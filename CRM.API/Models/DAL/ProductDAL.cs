using CRM.API.Models.EN;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace CRM.API.Models.DAL
{
    public class ProductDAL
    {
        readonly CRMContext _context;

        // Constructor que recibe un objeto CRMContext para
        // interactuar con la base de datos.
        public ProductDAL(CRMContext cRMContext)
        {
            _context = cRMContext;
        }

        // Método para crear un nuevo producto en la base de datos.
        public async Task<int> Create(Product product)
        {
            _context.Add(product);
            return await _context.SaveChangesAsync();
        }

        // Método para obtener un producto por su ID.
        public async Task<Product> GetById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            return product ?? new Product();
        }

        // Método para editar un producto existente en la base de datos.
        public async Task<int> Edit(Product product)
        {
            int result = 0;
            var productUpdate = await GetById(product.Id);
            if (productUpdate.Id != 0)
            {
                // Actualiza los datos de la empresa.
                productUpdate.Name = product.Name;
                productUpdate.Price = product.Price;

                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método para eliminar un producto de la base de datos por su ID.
        public async Task<int> Delete(int id)
        {
            int result = 0;
            var productDelete = await GetById(id);
            if (productDelete.Id > 0)
            {
                // Elimina un producto de la base de datos.
                _context.Products.Remove(productDelete);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método privado para construir una consulta IQueryable para buscar productos con filtros.
        private IQueryable<Product> Query(Product product)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(product.Name))
                query = query.Where(s => s.Name.Contains(product.Name));
            if (!string.IsNullOrWhiteSpace(Convert.ToString(product.Price)))
                query = query.Where(s => Convert.ToString( s.Price).Contains((char)product.Price));
            return query;
        }

        // Método para contar la cantidad de resultados de búsqueda con filtros.
        public async Task<int> CountSearch(Product product)
        {
            return await Query(product).CountAsync();
        }

        // Método para buscar productos con filtros, paginación y ordenamiento.
        public async Task<List<Product>> Search(Product product, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(product);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}
