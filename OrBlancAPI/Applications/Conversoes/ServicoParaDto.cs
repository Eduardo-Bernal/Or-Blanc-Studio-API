using OrBlancAPI.Domains;
using OrBlancAPI.DTOs.ServicoDto;

namespace OrBlancAPI.Applications.Conversoes
{
    public class ServicoParaDto
    {
        public static ListarServicoDto ConverterParaDto(Servico servico)
        {

            return new ListarServicoDto 
            {
                id_servico = servico.id_servico,
                nome = servico.nome,
                descricao = servico.descricao,
                valor = servico.valor,
                ativo = servico.ativo,

                imagemUrl = $"servico/{servico.id_servico}/imagem"
            };
        }
    }
}

