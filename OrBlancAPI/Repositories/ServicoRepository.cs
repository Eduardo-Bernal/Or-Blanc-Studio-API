using OrBlancAPI.Contexts;
using OrBlancAPI.Domains;
using OrBlancAPI.Interfaces;

namespace OrBlancAPI.Repositories
{
    public class ServicoRepository : IServicoRepository
    {
        private readonly OrBlancDBContext _context;

        public ServicoRepository(OrBlancDBContext context)
        {
            _context = context;
        }

        public List<Servico> Listar()
        {
            return _context.Servico.OrderBy(s => s.nome).ToList();
        }

        public void Adicionar(Servico servico)
        {
            _context.Servico.Add(servico);
            _context.SaveChanges();
        }
        

        public void Atualizar(Servico servico)
        {
            Servico? servicoBanco = _context.Servico.FirstOrDefault(s => s.id_servico == servico.id_servico);

            if (servicoBanco == null) return;

            servicoBanco.nome = servico.nome;
            servicoBanco.descricao = servico.descricao;
            servicoBanco.valor = servico.valor;
            servicoBanco.ativo = servico.ativo;

            _context.SaveChanges();
        }

        public Servico? BuscarPorId(int id)
        {
            return _context.Servico.Find(id);
        }

        public Servico BuscarPorNome(string nome)
        {
            return _context.Servico.FirstOrDefault(s => s.nome == nome);
        }

        public bool NomeExiste(string nome)
        {
            return _context.Servico.Any(s => s.nome == nome);
        }

        public void Remover(int id)
        {
            Servico? servicoBanco = _context.Servico.FirstOrDefault(s => s.id_servico == id);

            if (servicoBanco == null) return;

            _context.Servico.Remove(servicoBanco);
            _context.SaveChanges();
        }
    }
}
