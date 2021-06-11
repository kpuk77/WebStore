using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context
{
    public class WebStoreDB : DbContext
    {
        public WebStoreDB(DbContextOptions<WebStoreDB> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Section> Sections { get; set; }
    }
}
