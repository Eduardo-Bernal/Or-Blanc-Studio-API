using OrBlancAPI.Domains;

namespace OrBlancAPI.Interfaces
{
    public interface IClienteRepository
    {
        List<Cliente> Listar();
        Cliente ListarClientePorID(Guid id);

        VW_AgendaCompleta BuscarAgendamentosPendentes(string status);
        Cliente BuscarPorEmail(string email);

        bool EmailExiste(string email);

        void Adicionar(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Remover(Guid id);
    }
}
