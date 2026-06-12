namespace OrBlancAPI.DTOs.AgendamentoDto
{
    public class CriarAgendamentoDto
    {
        public Guid id_cliente { get; set; } 

        public Guid id_profissional { get; set; }

        public int id_servico { get; set; }

        public DateTime data_hora_inicio { get; set; }

        public DateTime data_hora_fim { get; set; }

        public string status { get; set; } = null!;

        public string? observacao { get; set; }
    }
}
