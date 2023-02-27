using Microsoft.EntityFrameworkCore;
using ProductsServer.Models;

namespace ProductsServer.DataBase
{
    public class ProductsDBContext : DbContext
    {
        public ProductsDBContext(DbContextOptions<ProductsDBContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> Groups { get; set; }
        public DbSet<ProductUnit> Units { get; set; }
        public DbSet<Manufacture> Manufactures { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<SaleProduct> SaleProducts { get; set; } 
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ArrivalProduct> ArrivalProducts { get; set; }
        public DbSet<NoteProduct> NoteProducts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
