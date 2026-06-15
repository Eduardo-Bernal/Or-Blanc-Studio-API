using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.DTOs.ProfissionalDto;
using OrBlancAPI.Exceptions;

namespace OrBlancAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfissionalController : ControllerBase
    {
        private readonly ProfissionalService _service;

        public ProfissionalController(ProfissionalService service)
        {
            _service = service;
        }

        [HttpGet]
        
        public ActionResult<List<ListarProfissionalDto>> Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("{id}")]
        public ActionResult<ListarProfissionalDto> BuscarPorId(Guid id)
        {
            try
            {
                return Ok(_service.BuscarPorId(id));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        public ActionResult<ListarProfissionalDto> BuscarPorEmail(string email)
        {
            try
            {
                return Ok(_service.BuscarPorEmail(email));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("telefone/{telefone}")]
        public ActionResult<ListarProfissionalDto> BuscarPorTelefone(string telefone)
        {
            try
            {
                return Ok(_service.BuscarPorTelefone(telefone));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Profissional")]
        public ActionResult<ListarProfissionalDto> Cadastrar(CriarProfissionalDto criarProfissionalDto)
        {
            try
            {
                ListarProfissionalDto profissionalCriado = _service.Cadastrar(criarProfissionalDto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = profissionalCriado.id_profissional }, profissionalCriado);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Profissional")]
        public ActionResult<ListarProfissionalDto> Atualizar(Guid id, CriarProfissionalDto criarProfissionalDto)
        {
            try
            {
                ListarProfissionalDto profissionalAtualizado = _service.Atualizar(id, criarProfissionalDto);
                return StatusCode(200, profissionalAtualizado);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Profissional")]
        public ActionResult Remover(Guid id)
        {
            try
            {
                _service.Deletar(id);
                return Ok("Profissional deletado com sucesso.");
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}