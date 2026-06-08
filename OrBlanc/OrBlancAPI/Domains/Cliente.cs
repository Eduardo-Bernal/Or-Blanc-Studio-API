using System;
using System.Collections.Generic;

namespace OrBlancAPI.Domains;

public partial class Cliente
{
    public Guid id_cliente { get; set; }

    public string nome { get; set; } = null!;

    public string telefone { get; set; } = null!;

    public string? email { get; set; }

    public virtual ICollection<Agendamento> Agendamento { get; set; } = new List<Agendamento>();
}
