using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Comando
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Instruccion { get; set; }

        [Required]
        public string LineaComandos { get; set; }

        [Required]
        public int PlataformaId { get; set; }

        public Plataforma Plataforma { get; set; }
    }
}