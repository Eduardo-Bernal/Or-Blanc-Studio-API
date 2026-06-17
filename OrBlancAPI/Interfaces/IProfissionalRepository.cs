using OrBlancAPI.Domains;

namespace OrBlancAPI.Interfaces
{
    public interface IProfissionalRepository
    {
        List<Profissional> Listar();
        Profissional? BuscarPorId(Guid id);
        Profissional? BuscarPorTelefone(string telefone);
        Profissional? BuscarPorEmail(string email);

<<<<<<< .merge_file_UJWkyz
        bool TelefoneExiste(string telefone);
=======
        byte[] ObterImagem(Guid id);
        bool TelefoneExiste(string telefone); 
>>>>>>> .merge_file_5cLtz2
        bool EmailExiste(string email);

        void Adicionar(Profissional profissional);
        void Atualizar(Profissional profissional);
        void Remover(Guid id);
    }
}