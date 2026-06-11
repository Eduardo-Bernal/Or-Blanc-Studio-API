using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.DTOs.AutenticacaoDto;
using OrBlancAPI.Exceptions;

namespace OrBlancAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly AutenticacaoService _service;

        public AutenticacaoController(AutenticacaoService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDto loginDto)
        {
            try
            {
                var token = _service.Login(loginDto);
                return Ok(token);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
