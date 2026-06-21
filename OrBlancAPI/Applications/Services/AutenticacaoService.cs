using OrBlancAPI.Applications.Autenticacao;
using OrBlancAPI.Domains;
using OrBlancAPI.DTOs.AutenticacaoDto;
using OrBlancAPI.Exceptions;
using OrBlancAPI.Interfaces;
using VHBurguer.DTOs.AutenticacaoDto;

namespace OrBlancAPI.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IProfissionalRepository _profissionalRepository; // 👈 novo
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(
            IClienteRepository clienteRepository,
            IProfissionalRepository profissionalRepository, // 👈 novo
            GeradorTokenJwt tokenJwt)
        {
            _clienteRepository = clienteRepository;
            _profissionalRepository = profissionalRepository;
            _tokenJwt = tokenJwt;
        }

        private static bool VerificarSenha(string senhaDigitada, byte[] senhaHashBanco)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigitada));
            return hashDigitado.SequenceEqual(senhaHashBanco);
        }

        public TokenDto Login(LoginDto loginDto)
        {
            // 1️⃣ Tenta encontrar como Cliente
            var cliente = _clienteRepository.BuscarPorEmail(loginDto.Email);

            if (cliente != null && VerificarSenha(loginDto.Senha, cliente.senha))
            {
                return new TokenDto
                {
                    Token = _tokenJwt.GerarToken(cliente),
                    Nome = cliente.nome,
                    Role = "Cliente",
                    IdCliente = cliente.id_cliente
                };
            }

            //  Tenta encontrar como Profissional
            // Profissional não tem email — adapte o campo de login conforme seu caso
            var profissional = _profissionalRepository.BuscarPorEmail(loginDto.Email);

            if (profissional != null && VerificarSenha(loginDto.Senha, profissional.senha))
            {
                return new TokenDto
                {
                    Token = _tokenJwt.GerarToken(profissional),
                    Nome = profissional.nome,
                    Role = "Profissional",
                    IdProfissional = profissional.id_profissional
                };
            }

            // 3️⃣ Não encontrou em nenhuma tabela
            throw new DomainException("E-mail ou senha inválidos");
        }
    }
}