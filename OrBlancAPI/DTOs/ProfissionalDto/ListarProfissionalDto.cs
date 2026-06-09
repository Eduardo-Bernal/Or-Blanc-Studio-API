namespace OrBlancAPI.DTOs.ProfissionalDto
{
    public class ListarProfissionalDto
    {
        public Guid id_profissional { get; set; }
        public string nome { get; set; }
        public string telefone { get; set; }
        public string especialidade { get; set; }
    }
}
