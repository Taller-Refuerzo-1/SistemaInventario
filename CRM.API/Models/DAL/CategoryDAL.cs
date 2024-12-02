using CRM.API.Models.EN;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Models.DAL
{
    public class CategoryDAL
    {
        readonly CRMContext _context;

        // Constructor que recibe un objeto CRMContext para
        // interactuar con la base de datos.
        public CategoryDAL(CRMContext cRMContext)
        {
            _context = cRMContext;
        }

        // Método para crear una nueva empresa en la base de datos.
        public async Task<int> Create(Category category)
        {
            _context.Add(category);
            return await _context.SaveChangesAsync();
        }

        // Método para obtener una empresa por su ID.
        public async Task<Category> GetById(int id)
        {
            var category = await _context.Category.FirstOrDefaultAsync(s => s.Id == id);
            return category ?? new Category();
        }

        // Método para editar una empresa existente en la base de datos.
        public async Task<int> Edit(Category category)
        {
            int result = 0;
            var categoryUpdate = await GetById(category.Id);
            if (categoryUpdate.Id != 0)
            {
                // Actualiza los datos de la empresa.
                categoryUpdate.Name = category.Name;

                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método para eliminar una empresa de la base de datos por su ID.
        public async Task<int> Delete(int id)
        {
            int result = 0;
            var categoryDelete = await GetById(id);
            if (categoryDelete.Id > 0)
            {
                // Elimina la empresa de la base de datos.
                _context.Category.Remove(categoryDelete);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método privado para construir una consulta IQueryable para buscar empresas con filtros.
        private IQueryable<Category> Query(Category category)
        {
            var query = _context.Category.AsQueryable();
            if (!string.IsNullOrWhiteSpace(category.Name))
                query = query.Where(s => s.Name.Contains(category.Name));
            return query;
        }

        // Método para contar la cantidad de resultados de búsqueda con filtros.
        public async Task<int> CountSearch(Category category)
        {
            return await Query(category).CountAsync();
        }

        // Método para buscar empresas con filtros, paginación y ordenamiento.
        public async Task<List<Category>> Search(Category category, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(category);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}
