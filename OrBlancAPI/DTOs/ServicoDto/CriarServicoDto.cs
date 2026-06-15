namespace OrBlancAPI.DTOs.ServicoDto
{
    public class CriarServicoDto
    {
        public string nome { get; set; }
        public string descricao { get; set; }
        public decimal valor { get; set; }
        public IFormFile? imagem { get; set; } = null;
        public bool ativo { get; set; } = true; 
    }
}
