using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Plataforma
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternaId { get; set; }

        [Required]
        public string Nome { get; set; }

        public ICollection<Comando> Comandos { get; set; } = new List<Comando>();
    }
}