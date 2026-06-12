using OrBlancAPI.Domains;

namespace OrBlancAPI.Interfaces
{
    public interface IAgendamentoRepository
    {
        List<VW_AgendaCompleta> Buscar();
        VW_AgendaCompleta? BuscarPorId(int id);
        List<VW_AgendaCompleta> BuscarPorCliente(Guid id);
        List<VW_AgendaCompleta> BuscarPorProfissional(Guid id);
        Agendamento Adicionar(Agendamento agendamento, Guid idProfissional);
        void Atualizar(int id, string novoStatus);
        void Remover(int id);
        bool ExisteConflitoHorario(Guid idProfissional, DateTime inicio, DateTime fim, int? ignorarId = null);
        int ContarAgendamentosAtivos(Guid idCliente);
    }
}