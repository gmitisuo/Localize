namespace Localize.Models
{
    public class UsuarioRegistro
    {
        public string NomeUsuario { get; set; } = "";
        public string Senha { get; set; } = "";
        public string Email { get; set; } = "";
    }
    public class UsuarioLogin
    {
        public string Email { get; set; } = "";
        public string Senha { get; set; } = "";
    }
    public class Atividade
    {
        public string Code { get; set; }
        public string Text { get; set; }
    }
    public class RespostaEmpresa
    {
        public String Nome { get; set; }
        public String Fantasia { get; set; }
        public String CNPJ { get; set; }
        public String Situacao { get; set; }
        public String Abertura { get; set; }
        public String Tipo { get; set; }
        public String Natureza_Juridica { get; set; }
        public List<Atividade> Atividade_Principal { get; set; }
        public String Logradouro { get; set; }
        public String Numero { get; set; }
        public String Complemento { get; set; }
        public String Bairro { get; set; }
        public String Municipio { get; set; }
        public String UF { get; set; }
        public String CEP { get; set; }
    }
}
