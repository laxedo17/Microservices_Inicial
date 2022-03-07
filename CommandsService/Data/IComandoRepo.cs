using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface IComandoRepo
    {
        bool SaveChanges();

        //Plataformas
        IEnumerable<Plataforma> GetTodasPlataformas();
        void CreatePlataforma(Plataforma plat);
        bool PlataformaExiste(int plataformaId);
        bool PlataformaExternaExiste(int plataformaExternaId);

        //Comandos
        IEnumerable<Comando> GetComandosParaPlataforma(int plataformaId);
        Comando GetComando(int plataformaId, int comandoId);
        void CreateComando(int plataformaId, Comando comando);
    }
}