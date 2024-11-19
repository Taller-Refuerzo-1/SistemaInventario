using CRM.API.Models.EN;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Models.DAL
{
    public class UsersDAL
    {
        readonly CRMContext _context;

        
        public UsersDAL(CRMContext cRMContext)
        {
            _context = cRMContext;
        }

        public async Task<int> Create(Users users)
        {
            _context.Add(users);
            return await _context.SaveChangesAsync();
        }

        public async Task<Users> GetById(int id)
        {
            var user = await _context.users.FirstOrDefaultAsync(s => s.Id == id);
            return user != null ? user : new Users();
        }

        public async Task<int> Edit(Users users)
        {
            int result = 0;
            var customerUpdate = await GetById(users.Id);
            if (customerUpdate.Id != 0)
            {
                // Actualiza los datos del cliente.
                customerUpdate.Name = users.Name;
                customerUpdate.LastName = users.LastName;
                customerUpdate.Email = users.Email;
                customerUpdate.Phone = users.Phone;
                customerUpdate.Password = users.Password;
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Delete(int id)
        {
            int result = 0;
            var customerDelete = await GetById(id);
            if (customerDelete.Id > 0)
            {
                // Elimina el cliente de la base de datos.
                _context.users.Remove(customerDelete);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        private IQueryable<Users> Query(Users users)
        {
            var query = _context.users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(users.Name))
                query = query.Where(s => s.Name.Contains(users.Name));
            if (!string.IsNullOrWhiteSpace(users.LastName))
                query = query.Where(s => s.LastName.Contains(users.LastName));
            return query;
        }

        public async Task<int> CountSearch(Users users)
        {
            return await Query(users).CountAsync();
        }

        public async Task<List<Users>> Search(Users users, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(users);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }

        public async Task<Users> ObtenerUsuarioPorDUIyPassword(string name, string password)
        {
            return await _context.users
                .FirstOrDefaultAsync(u => u.Name == name && u.Password == password);  // Asumiendo que tienes campos DUI y Password en tu base de datos
        }
    }
}
