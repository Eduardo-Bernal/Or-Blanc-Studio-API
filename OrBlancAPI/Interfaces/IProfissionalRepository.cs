using OrBlancAPI.Domains;

namespace OrBlancAPI.Interfaces
{
    public interface IProfissionalRepository
    {
        List<Profissional> Listar();
        Profissional? BuscarPorId(Guid id);
        Profissional? BuscarPorTelefone(string telefone);
        Profissional? BuscarPorEmail(string email);

        byte[] ObterImagem(Guid id);
        bool TelefoneExiste(string telefone); 
        bool EmailExiste(string email);

        void Adicionar(Profissional profissional);
        void Atualizar(Profissional profissional);
        void Remover(Guid id);
    }
}