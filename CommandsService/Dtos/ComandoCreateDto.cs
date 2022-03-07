using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos
{
    public class ComandoCreateDto
    {
        [Required]
        public string Instruccion { get; set; }
        [Required]
        public string LineaComandos { get; set; }
    }
}