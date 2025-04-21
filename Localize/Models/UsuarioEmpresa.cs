using System.Collections.ObjectModel;
using Localize.Models;
namespace Localize.Models
{
    public class UsuarioEmpresa
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
