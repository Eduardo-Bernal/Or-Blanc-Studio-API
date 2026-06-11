using OrBlancAPI.Domains;

namespace OrBlancAPI.Interfaces
{
    public interface IProfissionalRepository
    {
        List<Profissional> Listar();
        Profissional? BuscarPorId(Guid id);  
        
        void Adicionar(Profissional profissional);
        void Atualizar(Profissional profissional);
        void Remover(Guid id);
    }
}
