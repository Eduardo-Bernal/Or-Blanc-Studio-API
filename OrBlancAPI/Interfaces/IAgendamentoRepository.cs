using OrBlancAPI.Domains;

namespace OrBlancAPI.Interfaces
{
    public interface IAgendamentoRepository
    {
        VW_AgendaCompleta Buscar();
        VW_AgendaCompleta BuscarPorId(int id);
        VW_AgendaCompleta BuscarPorCliente(Guid id);
        VW_AgendaCompleta BuscarPorProfissional(Guid id);
        Agendamento Adicionar(Agendamento agendamento);
        Agendamento Atualizar(Guid id);
        Agendamento Remover(Guid id);
    }
}
