using CRM.API.Models.EN;
using Microsoft.EntityFrameworkCore;


namespace CRM.API.Models.DAL
{
    public class CompanyDAL
    {
        readonly CRMContext _context;

        // Constructor que recibe un objeto CRMContext para
        // interactuar con la base de datos.
        public CompanyDAL(CRMContext cRMContext)
        {
            _context = cRMContext;
        }

        // Método para crear una nueva empresa en la base de datos.
        public async Task<int> Create(Company company)
        {
            _context.Add(company);
            return await _context.SaveChangesAsync();
        }

        // Método para obtener una empresa por su ID.
        public async Task<Company> GetById(int id)
        {
            var company = await _context.Companys.FirstOrDefaultAsync(s => s.Id == id);
            return company ?? new Company();
        }

        // Método para editar una empresa existente en la base de datos.
        public async Task<int> Edit(Company company)
        {
            int result = 0;
            var companyUpdate = await GetById(company.Id);
            if (companyUpdate.Id != 0)
            {
                // Actualiza los datos de la empresa.
                companyUpdate.Name = company.Name;
                companyUpdate.Address = company.Address;
                companyUpdate.Telephone = company.Telephone;
                companyUpdate.Email = company.Email;
               
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método para eliminar una empresa de la base de datos por su ID.
        public async Task<int> Delete(int id)
        {
            int result = 0;
            var companyDelete = await GetById(id);
            if (companyDelete.Id > 0)
            {
                // Elimina la empresa de la base de datos.
                _context.Companys.Remove(companyDelete);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        // Método privado para construir una consulta IQueryable para buscar empresas con filtros.
        private IQueryable<Company> Query(Company company)
        {
            var query = _context.Companys.AsQueryable();
            if (!string.IsNullOrWhiteSpace(company.Name))
                query = query.Where(s => s.Name.Contains(company.Name));
            if (!string.IsNullOrWhiteSpace(company.Address))
                query = query.Where(s => s.Address.Contains(company.Address));
            if (!string.IsNullOrWhiteSpace(company.Telephone))
                query = query.Where(s => s.Telephone.Contains(company.Telephone));
            return query;
        }

        // Método para contar la cantidad de resultados de búsqueda con filtros.
        public async Task<int> CountSearch(Company company)
        {
            return await Query(company).CountAsync();
        }

        // Método para buscar empresas con filtros, paginación y ordenamiento.
        public async Task<List<Company>> Search(Company company, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(company);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}
