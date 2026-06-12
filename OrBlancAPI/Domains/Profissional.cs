using System;
using System.Collections.Generic;

namespace OrBlancAPI.Domains;

public partial class Profissional
{
    public Guid id_profissional { get; set; }

    public string nome { get; set; } = null!;

    public string especialidade { get; set; } = null!;

    public string telefone { get; set; } = null!;

    public string? email { get; set; }

    public bool ativo { get; set; }

    public byte[] senha { get; set; } = null!;

    public byte[]? imagem { get; set; }

    public virtual ICollection<Agendamento> id_agendamento { get; set; } = new List<Agendamento>();
}
