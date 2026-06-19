using System;
using System.Collections.Generic;

namespace OrBlancAPI.Domains;

public partial class Servico
{
    public int id_servico { get; set; }

    public string nome { get; set; } = null!;

    public string? descricao { get; set; }

    public decimal valor { get; set; }

    public bool ativo { get; set; }

    public byte[]? imagem { get; set; }

    public virtual ICollection<Agendamento> Agendamento { get; set; } = new List<Agendamento>();
}
