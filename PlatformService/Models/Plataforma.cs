using System.ComponentModel.DataAnnotations;

namespace PlatformService.Models
{
    public class Plataforma
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Creador { get; set; }

        [Required]
        public string Coste { get; set; }
    }
}