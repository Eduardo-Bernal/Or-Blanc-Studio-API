using Microsoft.AspNetCore.Mvc;
using OrBlancAPI.Contexts;
using OrBlancAPI.Domains;
using OrBlancAPI.DTOs.AgendamentoDto;
using OrBlancAPI.Exceptions;
using OrBlancAPI.Interfaces;

namespace OrBlancAPI.Applications.Services
{
    public class AgendamentoService
    {
        private readonly IAgendamentoRepository _repository;
        private readonly OrBlancDBContext _context;

        private const int MinutosAntecedenciaCriacao = 30;

        private const int HorasAntecedenciaCancelamento = 2;

        private const int LimiteAgendamentosAtivos = 3;

        private static readonly TimeOnly AberturasSalao = new TimeOnly(10, 0);
        private static readonly TimeOnly FechamentoSalao = new TimeOnly(22, 0);

        public AgendamentoService(IAgendamentoRepository repository, OrBlancDBContext context)
        {
            _repository = repository;
            _context = context;
        }

        public List<LerAgendamentoDto> Buscar()
        {
            return _repository.Buscar().Select(LerDto).ToList();
        }

        public LerAgendamentoDto BuscarPorId(int id)
        {
            var agendamento = _repository.BuscarPorId(id)
                ?? throw new DomainException("Agendamento não encontrado.");

            return LerDto(agendamento);
        }

        public List<LerAgendamentoDto> BuscarPorCliente(Guid idCliente)
        {
            var cliente = _context.Cliente.Find(idCliente)
                ?? throw new DomainException("Cliente não encontrado.");

            return _repository.BuscarPorCliente(idCliente).Select(LerDto).ToList();
        }

        public List<LerAgendamentoDto> BuscarPorProfissional(Guid idProfissional)
        {
            var profissional = _context.Profissional.Find(idProfissional)
                ?? throw new DomainException("Profissional não encontrado.");

            return _repository.BuscarPorProfissional(idProfissional).Select(LerDto).ToList();
        }

        private static void ValidarCriaçao(CriarAgendamentoDto dto)
        {
            var profissional = dto.id_profissional;
            if(profissional == null)
            {
                throw new DomainException("Insira um profissional");
            }

            var servico = dto.id_servico;
            if(servico == null)
            {
                throw new DomainException("Insira um servico");
            }

            var dataInicio = dto.data_hora_inicio;
            if(dataInicio == null)
            {
                throw new DomainException("Insira uma hora de inicio");
            }
            var dataFinal = dto.data_hora_fim;
            if(dataFinal == null)
            {
                throw new DomainException("Insira uma hora final");
            }

        }

        public LerAgendamentoDto Criar(CriarAgendamentoDto dto)
        {
            ValidarCriaçao(dto);

            var cliente = _context.Cliente.Find(dto.id_cliente)
                ?? throw new DomainException("Cliente não encontrado.");

            var profissional = _context.Profissional.Find(dto.id_profissional)
                ?? throw new DomainException("Profissional não encontrado.");

            if (!profissional.ativo)
                throw new DomainException("Profissional está inativo e não pode receber agendamentos.");

            var servico = _context.Servico.Find(dto.id_servico)
                ?? throw new DomainException("Serviço não encontrado.");

            if (!servico.ativo)
                throw new DomainException("Serviço está inativo.");

            var especialidades = profissional.especialidade
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.ToLower());

            if (!especialidades.Contains(servico.nome.ToLower()))
                throw new DomainException(
                    $"O profissional '{profissional.nome}' não está qualificado para o serviço '{servico.nome}'.");

            var minimoPermitido = DateTime.Now.AddMinutes(MinutosAntecedenciaCriacao);
            if (dto.data_hora_inicio < minimoPermitido)
                throw new DomainException(
                    $"O agendamento deve ser criado com pelo menos {MinutosAntecedenciaCriacao} minutos de antecedência. " + $"Horário mínimo aceito: {minimoPermitido:dd/MM/yyyy HH:mm}.");

            if (dto.data_hora_fim <= dto.data_hora_inicio)
                throw new DomainException("O horário de término deve ser maior que o horário de início.");


            ValidarHorarioFuncionamento(dto.data_hora_inicio, dto.data_hora_fim);

            if (_repository.ExisteConflitoHorario(dto.id_profissional, dto.data_hora_inicio, dto.data_hora_fim))
                throw new DomainException($"O profissional '{profissional.nome}' já possui agendamento nesse horário.");

            var totalAtivos = _repository.ContarAgendamentosAtivos(dto.id_cliente);
            if (totalAtivos >= LimiteAgendamentosAtivos)
                throw new DomainException(
                    $"O cliente já possui {LimiteAgendamentosAtivos} agendamentos ativos. " + "Cancele um agendamento antes de criar outro.");

            var novoAgendamento = new Agendamento
            {
                id_cliente = dto.id_cliente,
                id_servico = dto.id_servico,
                data_hora_inicio = dto.data_hora_inicio,
                data_hora_fim = dto.data_hora_fim,
                status = "Agendado",
                observacao = dto.observacao
            };

            var salvo = _repository.Adicionar(novoAgendamento, dto.id_profissional);

            var resultado = _repository.BuscarPorId(salvo.id_agendamento)
                ?? throw new DomainException("Erro ao recuperar o agendamento criado.");

            return LerDto(resultado);
        }


        public void Cancelar(int id, bool isAdmin = false)
        {
            var agendamento = _repository.BuscarPorId(id)
                ?? throw new DomainException("Agendamento não encontrado.");

            if (agendamento.status == "Cancelado")
                throw new DomainException("Agendamento já está cancelado.");

            if (agendamento.status == "Concluído")
                throw new DomainException("Não é possível cancelar um agendamento já concluído.");

            if (!isAdmin)
            {
                var antecedencia = agendamento.data_hora_inicio - DateTime.Now;
                if (antecedencia.TotalHours < HorasAntecedenciaCancelamento)
                    throw new DomainException(
                        $"O cancelamento só é permitido com pelo menos {HorasAntecedenciaCancelamento} horas de antecedência. " +
                        "Apenas o administrador pode cancelar fora desse prazo.");
            }

            _repository.Atualizar(id, "Cancelado");
        }


        public LerAgendamentoDto Reagendar(int id, DateTime novoInicio, DateTime novoFim, bool isAdmin = false)
        {
            var agendamento = _repository.BuscarPorId(id)
                ?? throw new DomainException("Agendamento não encontrado.");

            if (agendamento.status == "Cancelado")
                throw new DomainException("Não é possível reagendar um agendamento cancelado.");

            if (agendamento.status == "Concluído")
                throw new DomainException("Não é possível reagendar um agendamento já concluído.");

            if (!isAdmin)
            {
                var antecedencia = agendamento.data_hora_inicio - DateTime.Now;
                if (antecedencia.TotalHours < HorasAntecedenciaCancelamento)
                    throw new DomainException(
                        $"O reagendamento só é permitido com pelo menos {HorasAntecedenciaCancelamento} horas de antecedência.");
            }

            var minimoPermitido = DateTime.Now.AddMinutes(MinutosAntecedenciaCriacao);
            if (novoInicio < minimoPermitido)
                throw new DomainException(
                    $"O novo horário deve ser pelo menos {MinutosAntecedenciaCriacao} minutos no futuro.");

            if (novoFim <= novoInicio)
                throw new DomainException("O horário de término deve ser maior que o horário de início.");

            ValidarHorarioFuncionamento(novoInicio, novoFim);

            if (_repository.ExisteConflitoHorario(agendamento.id_profissional, novoInicio, novoFim, ignorarId: id))
                throw new DomainException(
                    $"O profissional '{agendamento.nome_profissional}' já possui agendamento nesse novo horário.");

            var entidade = _context.Agendamento.Find(id)!;
            entidade.data_hora_inicio = novoInicio;
            entidade.data_hora_fim = novoFim;
            entidade.status = "Agendado";
            _context.SaveChanges();

            return LerDto(_repository.BuscarPorId(id)!);
        }


        public void Concluir(int id)
        {
            var agendamento = _repository.BuscarPorId(id)
                ?? throw new DomainException("Agendamento não encontrado.");

            if (agendamento.status != "Confirmado" && agendamento.status != "Agendado")
                throw new DomainException($"Não é possível concluir um agendamento com status '{agendamento.status}'.");

            _repository.Atualizar(id, "Concluído");
        }

        private static void ValidarHorarioFuncionamento(DateTime inicio, DateTime fim)
        {
            var horaInicio = TimeOnly.FromDateTime(inicio);
            var horaFim = TimeOnly.FromDateTime(fim);

            if (horaInicio < AberturasSalao || horaFim > FechamentoSalao)
                throw new DomainException(
                    $"Agendamentos só são permitidos entre {AberturasSalao:HH:mm} e {FechamentoSalao:HH:mm}.");

            if (inicio.DayOfWeek == DayOfWeek.Sunday)
                throw new DomainException("O salão não funciona aos domingos.");
        }

        private static LerAgendamentoDto LerDto(VW_AgendaCompleta a) => new()
        {
            id_agendamento = a.id_agendamento,
            id_cliente = a.id_cliente,
            nome_cliente = a.nome_cliente,
            telefone_cliente = a.telefone_cliente,
            id_profissional = a.id_profissional,
            nome_profissional = a.nome_profissional,
            especialidade = a.especialidade,
            nome_servico = a.nome_servico,
            valor_servico = a.valor_servico,
            data_hora_inicio = a.data_hora_inicio,
            data_hora_fim = a.data_hora_fim,
            duracao_minutos = a.duracao_minutos,
            status = a.status,
            observacao = a.observacao
        };
    }
}