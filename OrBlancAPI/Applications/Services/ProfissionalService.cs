using OrBlancAPI.Domains;
using OrBlancAPI.DTOs.ProfissionalDto;
using OrBlancAPI.Interfaces;
using System.Security.Cryptography;
using OrBlancAPI.Exceptions;
using System.Text;
using OrBlancAPI.Applications.Conversoes;


namespace OrBlancAPI.Applications.Services
{
    public class ProfissionalService
    {
        private readonly IProfissionalRepository _repository;

        public ProfissionalService(IProfissionalRepository repository)
        {
            _repository = repository;
        }

        private static ListarProfissionalDto ListarDto(Profissional profissional)
        {
            ListarProfissionalDto listarProfissionalDto = new ListarProfissionalDto()
            {
                id_profissional = profissional.id_profissional,
                nome = profissional.nome,
                telefone = profissional.telefone,
                especialidade = profissional.especialidade,
                ativo = profissional.ativo,
                email = profissional.email
            };
            return listarProfissionalDto;
        }

        public List<ListarProfissionalDto> Listar()
        {
            List<Profissional> profissionais = _repository.Listar();
            List<ListarProfissionalDto> profissionalDtos = profissionais.Select(ProfissionalParaDto.ConverterParaDto).ToList();

            return profissionalDtos;
        }

        private static void ValidarTelefone(string telefone)
        {
            if (string.IsNullOrEmpty(telefone) || telefone.Length < 10 || telefone.Length > 11)
            {
                throw new DomainException("telefone inválido.");
            }
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                throw new DomainException("email invalido");
            }
        }

        public ListarProfissionalDto BuscarPorTelefone(string telefone)
        {
            ValidarTelefone(telefone);
            Profissional profissional = _repository.BuscarPorTelefone(telefone);

            if (profissional == null)
            {
                throw new DomainException("Profissional não encontrado.");
            }
            return ProfissionalParaDto.ConverterParaDto(profissional);
        }

        public ListarProfissionalDto BuscarPorEmail(string email)
        {
            ValidarEmail(email);
            Profissional profissional = _repository.BuscarPorEmail(email);

            if(profissional == null)
            {
                throw new DomainException("Profissional não encontrado");
            }
            return ListarDto(profissional);
        }

        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                throw new DomainException("Senha não pode ser vazia.");
            }

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public ListarProfissionalDto BuscarPorId(Guid id)
        {
            Profissional profissional = _repository.BuscarPorId(id);
            if (profissional == null)
            {
                throw new DomainException("Profissional não encontrado.");
            }
            return ProfissionalParaDto.ConverterParaDto(profissional);
        }

        public byte[] ObterImagem(Guid id)
        {
            byte[] imagem = _repository.ObterImagem(id);

            if (imagem == null || imagem.Length == 0)
            {
                throw new DomainException("Imagem não encontrada");
            }

            return imagem;
        }

        public static void ValidarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("Nome não pode ser vazio.");
            }
        }

        public ListarProfissionalDto Cadastrar(CriarProfissionalDto criarProfissionalDto)
        {
            ValidarNome(criarProfissionalDto.nome);
            ValidarEmail(criarProfissionalDto.email);
            ValidarTelefone(criarProfissionalDto.telefone);

            if (_repository.TelefoneExiste(criarProfissionalDto.telefone))
            {
                throw new DomainException("Telefone ja cadastrado.");
            }

            if(_repository.EmailExiste(criarProfissionalDto.email))
            {
                throw new DomainException("Email ja cadastrado.");
            }

            Profissional profissional = new Profissional()
            {
                nome = criarProfissionalDto.nome,
                telefone = criarProfissionalDto.telefone,
                email = criarProfissionalDto.email,
                especialidade = criarProfissionalDto.especialidade,
                imagem = ImagemParaBytes.ConverterImagem(criarProfissionalDto.imagem),
                senha = HashSenha(criarProfissionalDto.senha),
                ativo = true
            };
            _repository.Adicionar(profissional);
            return ProfissionalParaDto.ConverterParaDto(profissional);
        }

        public ListarProfissionalDto Atualizar(Guid id, CriarProfissionalDto criarProfissionalDto)
        {
            Profissional profissionalBanco = _repository.BuscarPorId(id);

            if (profissionalBanco == null)
            {
                throw new DomainException("Profissional não encontrado");
            }

            ValidarTelefone(criarProfissionalDto.telefone);
            ValidarNome(criarProfissionalDto.nome);
            ValidarEmail(criarProfissionalDto.email);

            Profissional profissionalTelefone = _repository.BuscarPorTelefone(criarProfissionalDto.telefone);
            Profissional profissionalEmail = _repository.BuscarPorEmail(criarProfissionalDto.email);

            if (profissionalTelefone != null && profissionalTelefone.id_profissional != id)
            {
                throw new DomainException("Telefone já cadastrado.");
            }

            if(profissionalEmail != null && profissionalEmail.id_profissional != id)
            {
                throw new DomainException("Email ja cadastrado.");
            }

            profissionalBanco.nome = criarProfissionalDto.nome;
            profissionalBanco.telefone = criarProfissionalDto.telefone;
            profissionalBanco.email = criarProfissionalDto.email;
            profissionalBanco.senha = HashSenha(criarProfissionalDto.senha);
            profissionalBanco.ativo = criarProfissionalDto.ativo;
            profissionalBanco.especialidade = criarProfissionalDto.especialidade;

            //if (criarProfissionalDto.imagem != null && criarProfissionalDto.imagem.Length > 0)
            //{
            //    profissionalBanco.imagem = ImagemParaBytes.ConverterImagem(profissionalBanco.imagem);
            //}

            _repository.Atualizar(profissionalBanco);

            return ProfissionalParaDto.ConverterParaDto(profissionalBanco);
        }

        public void Deletar(Guid id)
        {
            Profissional profissional = _repository.BuscarPorId(id);

            if (profissional == null)
            {
                throw new DomainException("Profissional não encontrado.");
            }

            profissional.ativo = false;
            _repository.Atualizar(profissional);
        }
    }
}
