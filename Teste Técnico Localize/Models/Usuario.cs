using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Localize.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        [Required]
        [StringLength(80)]
        public string Nome { get; set; }
        [Required]
        [StringLength(80)]
        public string Email { get; set; }
        [Required]
        [StringLength(80)]
        public string Senha { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioEmpresa> UsuarioEmpresa { get; set; }
    }
}