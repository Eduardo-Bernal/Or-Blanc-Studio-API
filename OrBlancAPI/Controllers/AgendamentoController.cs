using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.DTOs.AgendamentoDto;
using OrBlancAPI.Exceptions;
using System.Security.Claims;

namespace OrBlancAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoController : ControllerBase
    {
        private readonly AgendamentoService _service;

        public AgendamentoController(AgendamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerAgendamentoDto>> Listar()
        {
            List<LerAgendamentoDto> agendamentos = _service.Buscar();
            return Ok(agendamentos);
        }

        [HttpGet("{id}")]
        public ActionResult<LerAgendamentoDto> BuscarPorId(int id)
        {
            try
            {
                LerAgendamentoDto agendamento = _service.BuscarPorId(id);
                return Ok(agendamento);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("cliente/{id}")]
        public ActionResult<List<LerAgendamentoDto>> BuscarPorCliente(Guid id)
        {
            try
            {
                var idToken = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var role = User.FindFirstValue(ClaimTypes.Role);

                if (role == "Cliente" && idToken != id)
                    return Forbid();

                List<LerAgendamentoDto> agendamentos = _service.BuscarPorCliente(id);
                return Ok(agendamentos);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("profissional/{id}")]
        public ActionResult<List<LerAgendamentoDto>> BuscarPorProfissional(Guid id)
        {
            try
            {
                var idToken = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var role = User.FindFirstValue(ClaimTypes.Role);

                List<LerAgendamentoDto> agendamentos = _service.BuscarPorProfissional(id);
                return Ok(agendamentos);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult<LerAgendamentoDto> Criar(CriarAgendamentoDto dto)
        {
            try
            {
                var idToken = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (idToken != dto.id_cliente)
                    return Forbid();

                LerAgendamentoDto agendamento = _service.Criar(dto);
                return StatusCode(201, agendamento);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/cancelar")]
        public ActionResult Cancelar(int id)
        {
            try
            {
                var role = User.FindFirstValue(ClaimTypes.Role);
                var isAdmin = role == "Profissional"; 

                _service.Cancelar(id, isAdmin);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/reagendar")]
        [Authorize]
        public ActionResult<LerAgendamentoDto> Reagendar(int id, [FromBody] ReagendarAgendamentoDto dto)
        {
            try
            {
                var idToken = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var role = User.FindFirstValue(ClaimTypes.Role);
                var isAdmin = role == "Profissional";

                if (role == "Cliente")
                {
                    var agendamento = _service.BuscarPorId(id);
                    if (agendamento.id_cliente != idToken)
                        return Forbid();
                }

                LerAgendamentoDto resultado = _service.Reagendar(id, dto.data_hora_inicio, dto.data_hora_fim, isAdmin);
                return Ok(resultado);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/concluir")]
        [Authorize(Roles = "Profissional")]
        public ActionResult Concluir(int id)
        {
            try
            {
                _service.Concluir(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Profissional")]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.BuscarPorId(id); 
                return NoContent();
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}