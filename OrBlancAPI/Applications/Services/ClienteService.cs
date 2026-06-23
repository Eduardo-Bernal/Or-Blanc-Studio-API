using OrBlancAPI.Domains;
using OrBlancAPI.DTOs.ClienteDto;
using OrBlancAPI.Exceptions;
using OrBlancAPI.Interfaces;
using OrBlancAPI.Applications.Validaçoes;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace OrBlancAPI.Applications.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        private static LerClienteDto LerDto(Cliente cliente)
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

        public LerClienteDto ListarClientePorID(Guid id)
        {
            Cliente? cliente = _repository.ListarClientePorID(id);

            if(cliente == null)
            {
                throw new DomainException("Cliente não existe");
            }
            return LerDto(cliente);
        }

        private static byte[] HashSenha(string senha)
        {
            Validações.ValidarSenhaHash(senha);

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerClienteDto Adicionar(CriarClienteDto criarClienteDto)
        {
            if (_repository.EmailExiste(criarClienteDto.email))
            {
                throw new DomainException("O email já está cadastrado.");
            }

            Validações.ValidarEmail(criarClienteDto.email);
            Validações.ValidarTelefone(criarClienteDto.telefone);

            Cliente cliente = new Cliente
            {
                nome = criarClienteDto.nome,
                telefone = criarClienteDto.telefone,
                email = criarClienteDto.email,
                senha = HashSenha(criarClienteDto.senha)
            };

            _repository.Adicionar(cliente);

            return LerDto(cliente);
        }

        public LerClienteDto Atualizar(Guid id, CriarClienteDto criarClienteDto)
        {
            Cliente clienteBanco = _repository.ListarClientePorID(id);

            if (_repository.EmailExisteAtualizar(criarClienteDto.email, id))
            {
                throw new DomainException("O email já está cadastrado.");
            }

            Validações.ValidarEmail(criarClienteDto.email);
            Validações.ValidarTelefone(criarClienteDto.telefone);

            clienteBanco.nome = criarClienteDto.nome;
            clienteBanco.email = criarClienteDto.email;
            clienteBanco.telefone = criarClienteDto.telefone;
            clienteBanco.senha = HashSenha(criarClienteDto.senha);

            _repository.Atualizar(clienteBanco);
            return LerDto(clienteBanco);
        }

        public void Remover (Guid id)
        {
            Cliente? cliente = _repository.ListarClientePorID(id);

            if(cliente == null )
            {
                throw new DomainException("Cliente não encontrado");
            }

            _repository.Remover(id);
        }

    }
}
