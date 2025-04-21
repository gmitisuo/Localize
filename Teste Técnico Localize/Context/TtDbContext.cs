using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Localize.Models;
namespace Localize.Context
{
    public class TtDbContext : DbContext
    {
        public TtDbContext(DbContextOptions<TtDbContext> options) : base(options)
        {
        }
        //Criação do Banco de Dados a partir do Código
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<UsuarioEmpresa> UsuariosEmpresas { get;set; }
        //Relaciona as Tabelas na Criação do Banco de Dados
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioEmpresa>()
                .HasKey(ue => new { ue.UsuarioId, ue.EmpresaId });

            modelBuilder.Entity<UsuarioEmpresa>()
                .HasOne(ue => ue.Usuario)
                .WithMany(u => u.UsuarioEmpresa)
                .HasForeignKey(ue => ue.UsuarioId);

            modelBuilder.Entity<UsuarioEmpresa>()
                .HasOne(ue => ue.Empresa)
                .WithMany(e => e.UsuarioEmpresa)
                .HasForeignKey(ue => ue.EmpresaId);
        }
    }
}
