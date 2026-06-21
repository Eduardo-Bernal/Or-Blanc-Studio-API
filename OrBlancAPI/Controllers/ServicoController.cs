using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.DTOs.ProfissionalDto;
using OrBlancAPI.DTOs.ServicoDto;
using OrBlancAPI.Exceptions;

namespace OrBlancAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly ServicoService _service;

        public ServicoController(ServicoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ListarServicoDto>> Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("{id}")]
        public ActionResult<ListarServicoDto> BuscarPorId(int id)
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

        [HttpGet("{id}/imagem")]
        public ActionResult<byte[]> ObterImagem(int id)
        {
            try
            {
                var imagem = _service.ObterImagem(id);
                return File(imagem, "image/jpeg");
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("nome/{nome}")]
        public ActionResult<ListarServicoDto> BuscarPorNome(string nome)
        {
            try
            {
                return Ok(_service.BuscarPorNome(nome));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Profissional")]
        public ActionResult<ListarServicoDto> Adicionar(CriarServicoDto criarServicoDto)
        {
            try
            {
                ListarServicoDto servicoDto = _service.Adicionar(criarServicoDto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = servicoDto.id_servico }, servicoDto);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Profissional")]
        public ActionResult<ListarServicoDto> Atualizar(int id,CriarServicoDto atualizarServicoDto)
        {
            try
            {
                ListarServicoDto servicoAtualizado = _service.Atualizar(id, atualizarServicoDto);
                return StatusCode(200, servicoAtualizado);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Profissional")]
        public IActionResult Remover(int id)
        {
            try
            {
                _service.Deletar(id);
                return Ok("Servico deletado com sucesso");
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
