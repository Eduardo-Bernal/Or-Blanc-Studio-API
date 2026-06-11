using OrBlancAPI.Domains;

namespace OrBlancAPI.Interfaces
{
    public interface IServicoRepository
    {
        List<Servico> Listar(); 

        Servico? BuscarPorId(int id);
        Servico BuscarPorNome(string nome);

        bool NomeExiste(string nome);

        void Adicionar(Servico servico);
        void Atualizar(Servico servico);
        void Remover(int id);

    }
}
