using System;
using System.Collections.Generic;

namespace OrBlancAPI.Domains;

public partial class Agendamento
{
    public int id_agendamento { get; set; }

    public Guid id_cliente { get; set; }

    public Guid id_profissional { get; set; }

    public int id_servico { get; set; }

    public DateTime data_hora_inicio { get; set; }

    public DateTime data_hora_fim { get; set; }

    public string status { get; set; } = null!;

    public string? observacao { get; set; }

    public virtual Cliente id_clienteNavigation { get; set; } = null!;

    public virtual Profissional id_profissionalNavigation { get; set; } = null!;

    public virtual Servico id_servicoNavigation { get; set; } = null!;
}
