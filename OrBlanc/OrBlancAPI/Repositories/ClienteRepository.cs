using OrBlancAPI.Contexts;
using OrBlancAPI.Domains;
using OrBlancAPI.Interfaces;

namespace OrBlancAPI.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly OrBlancDBContext _context;

        public ClienteRepository(OrBlancDBContext context)
        {
            _context = context;
        }

        public void Adicionar(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            _context.SaveChanges();
        }



        public VW_AgendaCompleta BuscarAgendamentosPendentes(string status)
        {
           return _context.VW_AgendaCompleta.FirstOrDefault(cliente => cliente.status == status);
        }




        public List<Cliente> Listar()
        {
           return _context.Cliente.OrderBy(c => c.nome).ToList();
        }



        public Cliente ListarClientePorID(Guid id)
        {
            return _context.Cliente.Find(id);
        }


        
        public Cliente BuscarPorEmail(string email)
        {
            return _context.Cliente.FirstOrDefault(c => c.email == email);
        }



        public void Atualizar(Cliente cliente)
        {
            Cliente? clienteBanco = _context.Cliente.FirstOrDefault(c => c.id_cliente == cliente.id_cliente);

            if (clienteBanco == null) return;

            clienteBanco.email = cliente.email;
            clienteBanco.telefone = cliente.telefone;
            clienteBanco.nome = cliente.nome;
            clienteBanco.senha = cliente.senha;
           
            _context.SaveChanges();
        }



        public void Remover(Guid id)
        {
            Cliente? cliente = _context.Cliente.FirstOrDefault(c => c.id_cliente == id);

            if (cliente == null) return;

            _context.Cliente.Remove(cliente);
            _context.SaveChanges();

        }


        public bool EmailExiste(string email)
        {
            return _context.Cliente.Any(c => c.email == email);
        }


    }
}
