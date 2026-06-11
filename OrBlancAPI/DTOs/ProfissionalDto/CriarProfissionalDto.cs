namespace OrBlancAPI.DTOs.ProfissionalDto
{
    public class CriarProfissionalDto
    {
        public string nome { get; set; } = null!;
        public string telefone { get; set; } = null!;
        public string email { get; set; } = null!;
        public string especialidade { get; set; } = null!;
        public string senha { get; set; } = null!;
        public bool ativo { get; set; } = true;
    }
}