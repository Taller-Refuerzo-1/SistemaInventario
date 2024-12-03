using CRM.API.Models.EN;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Models.DAL
{
    public class BodegaDAL
    {
        readonly CRMContext _context;

        // Constructor que recibe un objeto CRMContext para
        // interactuar con la base de datos.
        public BodegaDAL(CRMContext cRMContext)
        {
            _context = cRMContext;
        }

        // Método para crear una nueva bodega en la base de datos.
        public async Task<int> Create(Bodega bodega)
        {
            _context.Add(bodega);
            return await _context.SaveChangesAsync();
        }

        // Método para obtener una bodega por su ID. 
        public async Task<Bodega> GetById(int id)
        {
            var bodega = await _context.Bodegas.FirstOrDefaultAsync(s => s.Id == id);
            return bodega ?? new Bodega();
        }

        // Método para editar una bodega existente en la base de datos.
        public async Task<int> Edit(Bodega bodega)
        {
            int result = 0;
            var companyUpdate = await GetById(bodega.Id);
            if (companyUpdate.Id != 0)
            {
                // Actualiza los datos de la empresa.
                companyUpdate.Name = bodega.Name;
                companyUpdate.Address = bodega.Address;

                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método para eliminar una bodega de la base de datos por su ID.
        public async Task<int> Delete(int id)
        {
            int result = 0;
            var bodegaDelete = await GetById(id);
            if (bodegaDelete.Id > 0)
            {
                // Elimina la bodega de la base de datos.
                _context.Bodegas.Remove(bodegaDelete);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método privado para construir una consulta IQueryable para buscar bodegas con filtros.
        private IQueryable<Bodega> Query(Bodega bodega)
        {
            var query = _context.Bodegas.AsQueryable();
            if (!string.IsNullOrWhiteSpace(bodega.Name))
                query = query.Where(s => s.Name.Contains(bodega.Name));
            if (!string.IsNullOrWhiteSpace(bodega.Address))
                query = query.Where(s => s.Address.Contains(bodega.Address));
            return query;
        }

        // Método para contar la cantidad de resultados de búsqueda con filtros.
        public async Task<int> CountSearch(Bodega bodega)
        {
            return await Query(bodega).CountAsync();
        }

        // Método para buscar bodega con filtros, paginación y ordenamiento.
        public async Task<List<Bodega>> Search(Bodega bodega, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(bodega);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}
