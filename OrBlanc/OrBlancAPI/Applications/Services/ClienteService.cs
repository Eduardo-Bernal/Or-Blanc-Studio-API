using OrBlancAPI.DTOs.ClienteDto;
using OrBlancAPI.Repositories;
using OrBlancAPI.Domains;

namespace OrBlancAPI.Applications.Services
{
    public class ClienteService 
    {
        public readonly ClienteRepository _repository;

        public ClienteService(ClienteRepository repository) 
        { 
            _repository = repository;
        }

        private static LerClienteDto LerDto (Cliente cliente)
        {
            LerClienteDto lerCliente = new LerClienteDto
            {
                id_cliente = cliente.id_cliente,
                nome = cliente.nome,
                telefone = cliente.telefone,
                email = cliente.email
            };

            return lerCliente;
        }


        public List<LerClienteDto> Listar()
        {
            List<Cliente> clientes = _repository.Listar();

            List<LerClienteDto> clienteDto = clientes.Select(c => LerDto(c)).ToList();

            return clienteDto;

        }

    }
}
