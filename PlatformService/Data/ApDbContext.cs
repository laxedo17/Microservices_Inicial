using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        //Definimos un DbSet, que rexistraremos en Startup.cs
        public DbSet<Plataforma> Plataformas { get; set; }
    }
}