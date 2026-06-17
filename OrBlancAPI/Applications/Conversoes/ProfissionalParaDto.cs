using OrBlancAPI.Domains;
using OrBlancAPI.DTOs.ProfissionalDto;
using OrBlancAPI.DTOs.ServicoDto;

namespace OrBlancAPI.Applications.Conversoes
{
    public class ProfissionalParaDto
    {
        public static ListarProfissionalDto ConverterParaDto(Profissional profissional)
        {

            return new ListarProfissionalDto
            {
                id_profissional = profissional.id_profissional,
                nome = profissional.nome,
                especialidade = profissional.especialidade,
                email = profissional.email,
                telefone = profissional.telefone,
                ativo = profissional.ativo,

                imagemUrl = $"profissional/{profissional.id_profissional}/imagem"
            };
        }
    }
}
