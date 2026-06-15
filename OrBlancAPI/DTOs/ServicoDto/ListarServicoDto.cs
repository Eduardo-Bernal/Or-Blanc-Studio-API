namespace OrBlancAPI.DTOs.ServicoDto
{
    public class ListarServicoDto
    {
        public int id_servico { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string imagemUrl { get; set; }
        public decimal valor { get; set; }
        public bool ativo { get; set; }
    }
}
