using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Models;

namespace CommandsService.Data
{
    public class ComandoRepo : IComandoRepo
    {
        private readonly AppDbContext _context;

        public ComandoRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateComando(int plataformaId, Comando comando)
        {
            if (comando == null)
            {
                throw new ArgumentNullException(nameof(comando));
            }

            comando.PlataformaId = plataformaId;
            _context.Comandos.Add(comando);
        }

        public void CreatePlataforma(Plataforma plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Plataformas.Add(plat);
        }

        public Comando GetComando(int plataformaId, int comandoId)
        {
            return _context.Comandos
            .Where(c => c.PlataformaId == plataformaId && c.Id == comandoId).FirstOrDefault();
        }

        public IEnumerable<Comando> GetComandosParaPlataforma(int plataformaId)
        {
            return _context.Comandos
            .Where(c => c.PlataformaId == plataformaId)
            .OrderBy(c => c.Plataforma.Nome);
        }

        public IEnumerable<Plataforma> GetTodasPlataformas()
        {
            return _context.Plataformas.ToList();
        }

        public bool PlataformaExiste(int plataformaId)
        {
            return _context.Plataformas.Any(p => p.Id == plataformaId);
        }

        public bool PlataformaExternaExiste(int plataformaExternaId)
        {
            return _context.Plataformas.Any(p => p.ExternaId == plataformaExternaId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}