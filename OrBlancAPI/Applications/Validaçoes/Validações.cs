using OrBlancAPI.Exceptions;

namespace OrBlancAPI.Applications.Validaçoes
{
    public class Validações
    {
        public static void ValidarSenhaHash(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new DomainException("Senha é obrigatória.");
            }
        }

        public static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException("Email é obrigatório.");
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new DomainException("Email inválido.");
            }
        }

        public static void ValidarTelefone(string telefone)
        {
            if(string.IsNullOrWhiteSpace(telefone))
            {
                throw new DomainException("Telefone é obrigatório.");
            }
             if (telefone.Length < 10 || telefone.Length > 11)
            {
                throw new DomainException("Telefone deve conter entre 10 e 11 dígitos.");
            }
        }
    }
}
