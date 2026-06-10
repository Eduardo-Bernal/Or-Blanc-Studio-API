using OrBlancAPI.DTOs.AutenticacaoDto;
using OrBlancAPI.Applications.Autenticacao;
using OrBlancAPI.Exceptions;
using OrBlancAPI.Interfaces;
using VHBurguer.DTOs.AutenticacaoDto;
using OrBlancAPI.Domains;

namespace OrBlancAPI.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IClienteRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(IClienteRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        // compara a hash SHA256 
        private static bool VerificarSenha(string senhaDigitada, byte[] senhaHashBanco)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigitada));

            return hashDigitado.SequenceEqual(senhaHashBanco);
        }

        public TokenDto Login(LoginDto loginDto)
        {
            Cliente cliente = _repository.BuscarPorEmail(loginDto.Email);

            if (cliente == null)
            {
                throw new DomainException("E-mail ou senha inválidos");
            }

            // comparar a senha digitada com a senha armazenada
            if (!VerificarSenha(loginDto.Senha, cliente.senha))
            {
                throw new DomainException("E-mail ou senha inválidos");
            }

            // gerando o token
            var token = _tokenJwt.GerarToken(cliente);

            TokenDto novoToken = new TokenDto { Token = token };

            return novoToken;
        }
    }
}
