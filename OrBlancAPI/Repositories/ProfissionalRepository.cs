using OrBlancAPI.Contexts;
using OrBlancAPI.Interfaces;
using OrBlancAPI.Domains;

namespace OrBlancAPI.Repositories
{

    public class ProfissionalRepository : IProfissionalRepository
    {
        
        private readonly OrBlancDBContext _context;

        public ProfissionalRepository(OrBlancDBContext context)
        {
            _context = context;
        }

        public List<Profissional> Listar()
        {
            return _context.Profissional.OrderBy(p => p.nome).ToList();
        }

        public Profissional? BuscarPorId(Guid id)
        {
            return _context.Profissional.Find(id);
        }


        public void Adicionar(Profissional profissional)
        {
            _context.Profissional.Add(profissional);
            _context.SaveChanges();
        }

        public void Atualizar(Profissional profissional)
        {
            Profissional? profissionalBanco = _context.Profissional.FirstOrDefault(p => p.id_profissional == profissional.id_profissional);

            if (profissionalBanco == null) return;

            profissionalBanco.nome = profissional.nome;
            profissionalBanco.email = profissional.email;
            profissionalBanco.senha = profissional.senha;
            profissionalBanco.telefone = profissional.telefone;
            profissionalBanco.especialidade = profissional.especialidade;

            _context.SaveChanges();
        }

        public void Remover(Guid id)
        {
            Profissional? profissionalBanco = _context.Profissional.FirstOrDefault(p => p.id_profissional == id);

            if (profissionalBanco == null) return;

            _context.Profissional.Remove(profissionalBanco);
            _context.SaveChanges();
        }

        public Profissional? BuscarPorTelefone(string telefone)
        {
            return _context.Profissional.FirstOrDefault(p => p.telefone == telefone);
        }

        public Profissional? BuscarPorEmail(string email)
        {
            return _context.Profissional.FirstOrDefault(p => p.email == email);
        }

        public bool TelefoneExiste(string telefone)
        {
            return _context.Profissional.Any(p => p.telefone == telefone);
        }
        
        public bool EmailExiste(string email)
        {
            return _context.Profissional.Any(p => p.email == email);
        }

        public byte[] ObterImagem(Guid id)
        {
            var profissional = _context.Profissional
                .Where(profissional => profissional.id_profissional == id)
                .Select(profissional => profissional.imagem)
                .FirstOrDefault();

            return profissional;
        }
    }
}
