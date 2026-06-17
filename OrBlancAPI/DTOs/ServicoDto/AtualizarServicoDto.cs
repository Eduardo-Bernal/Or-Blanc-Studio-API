namespace OrBlancAPI.DTOs.ServicoDto
{
    public class AtualizarServicoDto
    {
        public int id_servico { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public decimal valor { get; set; }
        public IFormFile? imagem { get; set; } = null;
        public bool ativo { get; set; } = true;
    }
}
