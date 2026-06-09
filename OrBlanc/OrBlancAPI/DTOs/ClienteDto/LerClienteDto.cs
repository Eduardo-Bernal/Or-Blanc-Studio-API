namespace OrBlancAPI.DTOs.ClienteDto
{
    public class LerClienteDto
    {
        public Guid id_cliente { get; set; }
        public string nome { get; set; } = string.Empty;
        public string telefone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}
