using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Localize.Models
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string NomeEmpresarial { get; set; }
        [Required]
        [StringLength(200)]
        public string NomeFantasia { get; set; }
        [Required]
        [StringLength(20)]
        public string CNPJ { get; set; }
        [Required]
        [StringLength(200)]
        public string Situacao { get; set; }
        [Required]
        [StringLength(200)]
        public string Abertura { get; set; }
        [Required]
        [StringLength(200)]
        public string Tipo { get; set; }
        [Required]
        [StringLength(200)]
        public string NaturezaJuridica { get; set; }
        [Required]
        [StringLength(200)]
        public string AtividadePrincipalCode { get; set; }
        [Required]
        [StringLength(200)]
        public string AtividdadePrincipalText { get; set; }
        public Endereco Endereco { get; set; }
        [JsonIgnore]
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioEmpresa> UsuarioEmpresa { get; set; }
    }
    public class Endereco
    {
        [Key]
        public int EnderecoID { get; set; }
        [Required]
        [StringLength(200)]
        public string Logradouro { get; set; }
        [Required]
        [StringLength(200)]
        public string Numero { get; set; }
        [Required]
        [StringLength(200)]
        public string Complemento { get; set; }
        [Required]
        [StringLength(200)]
        public string Bairro { get; set; }
        [Required]
        [StringLength(200)]
        public string Municipio { get; set; }
        [Required]
        [StringLength(200)]
        public string UF { get; set; }
        [Required]
        [StringLength(200)]
        public string CEP { get; set; }
        [Required]
        [StringLength(10)]
        public int EmpresaId { get; set; }
        [JsonIgnore]
        public Empresa Empresa { get; set; }

    }
}
