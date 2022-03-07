using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos
{
    public class PlataformaCreateDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Creador { get; set; }

        [Required]
        public string Coste { get; set; }
    }
}