using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.DTOs.ClienteDto;

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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
