using CRM.API.Models.EN;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Models.DAL
{
    public class ProvidersDAL
    {
        readonly CRMContext _context;

        public ProvidersDAL(CRMContext cRMContext)
        {
            _context = cRMContext;
        }

        // Método para crear un nuevo cliente en la base de datos.
        public async Task<int> Create(Providers providers)
        {
            _context.Add(providers);
            return await _context.SaveChangesAsync();
        }

        // Método para obtener un cliente por su ID.
        public async Task<Providers> GetById(int id)
        {
            var proveedor = await _context.Providers.FirstOrDefaultAsync(s => s.Id == id);
            return proveedor != null ? proveedor : new Providers();
        }

        // Método para editar un cliente existente en la base de datos.
        public async Task<int> Edit(Providers prov)
        {
            int result = 0;
            var proveedorUpdate = await GetById(prov.Id);
            if (proveedorUpdate.Id != 0)
            {
                // Actualiza los datos del cliente.
                proveedorUpdate.Name = prov.Name;
                proveedorUpdate.Empresa = prov.Empresa;
                proveedorUpdate.Email = prov.Email;
                proveedorUpdate.Phone = prov.Phone;
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método para eliminar un cliente de la base de datos por su ID.
        public async Task<int> Delete(int id)
        {
            int result = 0;
            var proveedorDelete = await GetById(id);
            if (proveedorDelete.Id > 0)
            {
                // Elimina el cliente de la base de datos.
                _context.Providers.Remove(proveedorDelete);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método privado para construir una consulta IQueryable para buscar clientes con filtros.
        private IQueryable<Providers> Query(Providers prov)
        {
            var query = _context.Providers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(prov.Name))
                query = query.Where(s => s.Name.Contains(prov.Name));
            if (!string.IsNullOrWhiteSpace(prov.Empresa))
                query = query.Where(s => s.Empresa.Contains(prov.Empresa));
            return query;
        }

        // Método para contar la cantidad de resultados de búsqueda con filtros.
        public async Task<int> CountSearch(Providers prov)
        {
            return await Query(prov).CountAsync();
        }

        // Método para buscar clientes con filtros, paginación y ordenamiento.
        public async Task<List<Providers>> Search(Providers prov, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(prov);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}
