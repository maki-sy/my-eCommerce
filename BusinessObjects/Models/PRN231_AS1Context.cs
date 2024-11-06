using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Models
{
    public partial class PRN231_AS1Context : DbContext
    {
        public PRN231_AS1Context()
        {
        }

        public PRN231_AS1Context(DbContextOptions<PRN231_AS1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.OrderId, od.ProductId });
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "iPhone" },
                new Category { CategoryId = 2, CategoryName = "Samsung" },
                new Category { CategoryId = 3, CategoryName = "Xiaomi" },
                new Category { CategoryId = 4, CategoryName = "Realme" },
                new Category { CategoryId = 5, CategoryName = "Huawei" },
                new Category { CategoryId = 6, CategoryName = "Oppo" },
                new Category { CategoryId = 7, CategoryName = "ROG" }
                );
        }
    }
}
