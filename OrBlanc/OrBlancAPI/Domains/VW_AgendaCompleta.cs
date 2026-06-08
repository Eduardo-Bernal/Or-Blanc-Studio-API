using System;
using System.Collections.Generic;

namespace OrBlancAPI.Domains;

public partial class VW_AgendaCompleta
{
    public int id_agendamento { get; set; }

    public string nome_cliente { get; set; } = null!;

    public string telefone_cliente { get; set; } = null!;

    public string nome_profissional { get; set; } = null!;

    public string especialidade { get; set; } = null!;

    public string nome_servico { get; set; } = null!;

    public decimal valor_servico { get; set; }

    public DateTime data_hora_inicio { get; set; }

    public DateTime data_hora_fim { get; set; }

    public int? duracao_minutos { get; set; }

    public string status { get; set; } = null!;

    public string? observacao { get; set; }
}
