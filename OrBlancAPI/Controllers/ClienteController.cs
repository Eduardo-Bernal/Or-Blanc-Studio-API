using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.DTOs.ClienteDto;
using OrBlancAPI.Exceptions;

namespace OrBlancAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public readonly ClienteService _service;

        public ClienteController(ClienteService service) 
        {
            _service = service;
        }

        

       [HttpGet]
       public ActionResult <List<LerClienteDto>> Listar()
        {
            List<LerClienteDto> clientes = _service.Listar();

            return clientes;
        }


        [HttpPost]
        public ActionResult<LerClienteDto> Adicionar(CriarClienteDto criarClienteDto)
        {
            try
            {
                LerClienteDto cliente = _service.Adicionar(criarClienteDto);
                return StatusCode(201, cliente);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<LerClienteDto> ListarClientePorID(Guid id)
        {
            LerClienteDto clientes = _service.ListarClientePorID(id);

            if(clientes == null)
            {
                return NotFound();
            }

            return Ok(clientes);
        }


        [HttpPut("{id}")]
        public ActionResult<LerClienteDto> Atualizar(Guid id ,CriarClienteDto AtualizarCliente)
        {
            try
            {
                LerClienteDto cliente = _service.Atualizar(id, AtualizarCliente);
                return StatusCode(200, cliente);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Profissional")]
        public ActionResult<LerClienteDto> Remover(Guid id)
        {
            try
            {
                _service.Remover(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
