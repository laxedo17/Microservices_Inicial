using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Plataforma> Plataformas { get; set; }
        public DbSet<Comando> Comandos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Plataforma>()
                .HasMany(p => p.Comandos)
                .WithOne(p => p.Plataforma!)
                .HasForeignKey(p => p.PlataformaId);

            modelBuilder
                .Entity<Comando>()
                .HasOne(p => p.Plataforma)
                .WithMany(p => p.Comandos)
                .HasForeignKey(p => p.PlataformaId);
        }
    }
}
