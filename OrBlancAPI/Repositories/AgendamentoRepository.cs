using OrBlancAPI.Contexts;
using OrBlancAPI.Domains;
using OrBlancAPI.Interfaces;

namespace OrBlancAPI.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly OrBlancDBContext _context;

        public AgendamentoRepository(OrBlancDBContext context)
        {
            _context = context;
        }

        public List<VW_AgendaCompleta> Buscar()
        {
            return _context.VW_AgendaCompleta
                .OrderBy(a => a.data_hora_inicio)
                .ToList();
        }

        public VW_AgendaCompleta? BuscarPorId(int id)
        {
            return _context.VW_AgendaCompleta
                .FirstOrDefault(a => a.id_agendamento == id);
        }

        public List<VW_AgendaCompleta> BuscarPorCliente(Guid id)
        {
            return _context.VW_AgendaCompleta
                .Where(a => a.id_cliente == id)
                .OrderBy(a => a.data_hora_inicio)
                .ToList();
        }

        public List<VW_AgendaCompleta> BuscarPorProfissional(Guid id)
        {
            return _context.VW_AgendaCompleta
                .Where(a => a.id_profissional == id)
                .OrderBy(a => a.data_hora_inicio)
                .ToList();
        }

        public Agendamento Adicionar(Agendamento agendamento)
        {
            _context.Agendamento.Add(agendamento);
            _context.SaveChanges();
            return agendamento;
        }

        public void Atualizar(int id, string novoStatus)
        {
            var agendamento = _context.Agendamento.Find(id);

            if (agendamento == null) return;

            agendamento.status = novoStatus;
            _context.SaveChanges();
        }


        public void Remover(int id)
        {
            var agendamento = _context.Agendamento.Find(id);

            if (agendamento == null) return;

            _context.Agendamento.Remove(agendamento);
            _context.SaveChanges();
        }

    }
}