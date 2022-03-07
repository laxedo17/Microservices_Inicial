namespace CommandsService.Dtos
{
    public class ComandoReadDto
    {
        public int Id { get; set; }
        public string Instruccion { get; set; }
        public string LineaComandos { get; set; }
        public int PlataformaId { get; set; }
    }
}