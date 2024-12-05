// Importa el espacio de nombres necesario para DbContext.
using Microsoft.EntityFrameworkCore;
using CRM.API.Models.EN;

// Define la clase CRMContext que hereda de DbContext.
namespace CRM.API.Models.DAL
{
    public class CRMContext : DbContext
    {
        // Constructor que toma DbContextOptions como parámetro para configurar la conexión a la base de datos.
        public CRMContext(DbContextOptions<CRMContext> options) : base(options)
        {
        }

        // Define un DbSet llamado "Customers" que representa una tabla de clientes en la base de datos.
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Users> users { get; set; }
        public DbSet<Providers> Providers { get; set; }
        public DbSet<Sucursal> Sucursal { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Bodega> Bodegas { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Category { get; set; }
    }
}
