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

<<<<<<< .merge_file_w28RbV
    public byte[]? imagem { get; set; }
=======
    public byte[] imagem { get; set; } = null!;
>>>>>>> .merge_file_YLN8gA

    public virtual ICollection<Agendamento> Agendamento { get; set; } = new List<Agendamento>();
}
