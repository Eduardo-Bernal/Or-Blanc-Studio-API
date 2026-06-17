namespace OrBlancAPI.DTOs.ProfissionalDto
{
    public class CriarProfissionalDto
    {
        public string nome { get; set; } = null!;
<<<<<<< .merge_file_p4UW8I
        public string telefone { get; set; } = null!;
        public string email { get; set; } = null!;
        public string especialidade { get; set; } = null!;
=======
        public string telefone { get; set; } = null!;   
        public string email { get; set; } = null!;
        public string especialidade { get; set; } = null!;
        public IFormFile imagem { get; set; } = null!;
        //public byte[] imagem { get; set; }
>>>>>>> .merge_file_6JgyOP
        public string senha { get; set; } = null!;
        public bool ativo { get; set; } = true;
    }
}