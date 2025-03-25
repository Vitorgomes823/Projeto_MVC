using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Projeto_MVC.Models;

namespace Projeto_MVC.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(255);
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.CPF).HasMaxLength(11).IsRequired();
            });
        }
    }
}
