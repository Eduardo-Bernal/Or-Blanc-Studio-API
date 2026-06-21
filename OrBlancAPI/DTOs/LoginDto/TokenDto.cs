namespace VHBurguer.DTOs.AutenticacaoDto
{
    public class TokenDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiracao { get; set; }
        public string Nome { get; set; } = null!;
        public string Role { get; set; } = null!;

        public Guid? IdCliente { get; set; }
        public Guid? IdProfissional { get; set; }
    }
}
