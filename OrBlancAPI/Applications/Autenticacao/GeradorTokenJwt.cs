using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OrBlancAPI.Domains;
using OrBlancAPI.Exceptions;

namespace OrBlancAPI.Applications.Autenticacao
{
    public class GeradorTokenJwt
    {
        private readonly IConfiguration _config;

        public GeradorTokenJwt(IConfiguration config)
        {
            _config = config;
        }

        // ✅ Método privado central — evita duplicar a lógica de geração
        private string GerarTokenInterno(List<Claim> claims)
        {
            var chave = _config["Jwt:Key"]!;
            var issuer = _config["Jwt:Issuer"]!;
            var audience = _config["Jwt:Audience"]!;
            var expiraEmMinutos = int.Parse(_config["Jwt:ExpiraEmMinutos"]!);

            var keyBytes = Encoding.UTF8.GetBytes(chave);

            if (keyBytes.Length < 32)
                throw new DomainException("Jwt: Key precisa ter pelo menos 32 caracteres (256 bits).");

            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiraEmMinutos),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ✅ Sobrecarga para Cliente
        public string GerarToken(Cliente usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.id_cliente.ToString()),
                new Claim(ClaimTypes.Name,           usuario.nome),
                new Claim(ClaimTypes.Email,          usuario.email),
                new Claim(ClaimTypes.Role,           "Cliente")        
            };

            return GerarTokenInterno(claims);
        }

        // ✅ Sobrecarga para Profissional
        public string GerarToken(Profissional usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.id_profissional.ToString()),
                new Claim(ClaimTypes.Name,           usuario.nome),
                new Claim(ClaimTypes.Role,           "Profissional")   
            };

            return GerarTokenInterno(claims);
        }
    }
}