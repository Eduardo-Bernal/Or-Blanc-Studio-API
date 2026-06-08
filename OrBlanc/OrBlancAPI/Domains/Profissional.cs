using System;
using System.Collections.Generic;

namespace OrBlancAPI.Domains;

public partial class Profissional
{
    public Guid id_profissional { get; set; }

    public string nome { get; set; } = null!;

    public string especialidade { get; set; } = null!;

    public string telefone { get; set; } = null!;

    public bool ativo { get; set; }

    public virtual ICollection<Agendamento> Agendamento { get; set; } = new List<Agendamento>();
}
