//Clase para implementacion da interface
using System;
using System.Collections.Generic;
using System.Linq;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlataformaRepo : IPlataformaRepo
    {
        private readonly AppDbContext _context; //campo readonly, dependency injection en todo seu esplendor

        /// <summary>
        /// Constructor de clase con Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        public PlataformaRepo(AppDbContext context)
        {
            _context = context; //dependency injection no obxeto que fai que cada obxeto PlataformaRepo conteña un obxeto AppDbContext cando se crea
        }
        public void CreatePlataforma(Plataforma plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }

            _context.Plataformas.Add(plat);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0); //si o resultado de Savechanges e superior a cero, devolvemos true, o que indica que temos algo que cambia
        }

        public Plataforma GetPlataformaById(int id)
        {
            return _context.Plataformas.FirstOrDefault(p => p.Id == id); //expresion lambda que significa que p goes to p.Id (p vai a p.Id) onde a id da plataforma e igual a id
        }

        public IEnumerable<Plataforma> GetTodasPlataformas()
        {
            return _context.Plataformas.ToList(); //Plataformas proven de public DbSet<Plataforma> Plataformas { get; set; } que esta en AppDbContext
        }
    }
}