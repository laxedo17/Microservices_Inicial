//Usamos Interface Concrete Class Pattern porque inxectaremos o noso repositorio con Dependency Injection na clase Startup
using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlataformaRepo
    {
        //Especificamos os metodos que pedira a interface
        bool SaveChanges();

        IEnumerable<Plataforma> GetTodasPlataformas(); //IEnumerable porque queremos obter varias plataformas
        Plataforma GetPlataformaById(int id); //Para obter unha plataforma individual
        void CreatePlataforma(Plataforma plat);

    }
}