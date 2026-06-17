using OrBlancAPI.DTOs.ServicoDto;
using OrBlancAPI.Interfaces;
using OrBlancAPI.Domains;
using OrBlancAPI.Exceptions;
using OrBlancAPI.Applications.Conversoes;

namespace OrBlancAPI.Applications.Services
{
    public class ServicoService
    {
        private readonly IServicoRepository _servicoRepository;

        public ServicoService(IServicoRepository _repository)
        {
            _servicoRepository = _repository;
        }

        private static ListarServicoDto ListarDto(Servico servico)
        {
            ListarServicoDto listarServicoDto = new ListarServicoDto()
            {
                id_servico = servico.id_servico,
                nome = servico.nome,
                descricao = servico.descricao,
                valor = servico.valor,
                ativo = servico.ativo
            };
            return listarServicoDto;
        }

        public List<ListarServicoDto> Listar()
        {
            List<Servico> servicos = _servicoRepository.Listar();
            List<ListarServicoDto> servicoDtos = servicos.Select(ServicoParaDto.ConverterParaDto).ToList();

            return servicoDtos;
        }

        private static void ValidarValor(decimal valor)
        {
            if (valor < 0 || valor == null)
            {
                throw new DomainException("Valor do serviço invalido.");
            }
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("Nome do serviço é obrigatório.");
            }
        }

        public ListarServicoDto BuscarPorNome(string nome)
        {
            ValidarNome(nome);
            Servico servico = _servicoRepository.BuscarPorNome(nome);
            if (servico == null)
            {
                throw new DomainException("Serviço não encontrado.");
            }
            return ListarDto(servico);
        }

        public ListarServicoDto BuscarPorId(int id)
        {
            Servico servico = _servicoRepository.BuscarPorId(id);
            if (servico == null)
            {
                throw new DomainException("Serviço não encontrado.");
            }
            return ServicoParaDto.ConverterParaDto(servico);
        }

        public byte[] ObterImagem(int id)
        {
            byte[] imagem = _servicoRepository.ObterImagem(id);

            if (imagem == null || imagem.Length == 0)
            {
                throw new DomainException("Imagem não encontrada");
            }

            return imagem;
        }

        public ListarServicoDto Adicionar(CriarServicoDto criarServico)
        {

            ValidarNome(criarServico.nome);
            ValidarValor(criarServico.valor);
            if (_servicoRepository.NomeExiste(criarServico.nome))
            {
                throw new DomainException("Já existe um serviço com esse nome.");
            }

            Servico servico = new Servico()
            {
                nome = criarServico.nome,
                descricao = criarServico.descricao,
                valor = criarServico.valor,
                imagem = ImagemParaBytes.ConverterImagem(criarServico.imagem),
                ativo = true
            };
            _servicoRepository.Adicionar(servico);
            return ServicoParaDto.ConverterParaDto(servico);
        }

        public ListarServicoDto Atualizar(int id, CriarServicoDto atualizarServico)
        {
            ValidarNome(atualizarServico.nome);
            ValidarValor(atualizarServico.valor);

            Servico? servicoBanco = _servicoRepository.BuscarPorId(id);

            if (servicoBanco == null)
            {
                throw new DomainException("Serviço não encontrado.");
            }
            if (servicoBanco.nome != atualizarServico.nome && _servicoRepository.NomeExiste(atualizarServico.nome))
            {
                throw new DomainException("Já existe um serviço com esse nome.");
            }

            servicoBanco.nome = atualizarServico.nome;
            servicoBanco.descricao = atualizarServico.descricao;
            servicoBanco.valor = atualizarServico.valor;
            servicoBanco.ativo = atualizarServico.ativo;
            if (atualizarServico.imagem != null)
            {
                servicoBanco.imagem = ImagemParaBytes.ConverterImagem(atualizarServico.imagem);
            }
            _servicoRepository.Atualizar(servicoBanco);
            return ServicoParaDto.ConverterParaDto(servicoBanco);
        }

        public void Deletar(int id)
        {
            Servico? servicoBanco = _servicoRepository.BuscarPorId(id);
            if (servicoBanco == null)
            {
                throw new DomainException("Serviço não encontrado.");
            }
            servicoBanco.ativo = false;
            _servicoRepository.Atualizar(servicoBanco);
        }
    }
}
