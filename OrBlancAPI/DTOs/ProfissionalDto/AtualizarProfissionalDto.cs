namespace OrBlancAPI.DTOs.ProfissionalDto
{
    public class AtualizarProfissionalDto
    {
        public string? Nome { get; set; }
        public string? Telefone { get; set; }    
        public string? Email { get; set; }
        public string? Especialidade { get; set; }   
        public IFormFile? Imagem { get; set; }
        public string? senha { get; set; }
        public bool ativo { get; set; } 
    }
}
