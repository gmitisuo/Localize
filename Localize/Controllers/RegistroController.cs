using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Localize.Context;
using Localize.Models;
using Localize.Services;

namespace Localize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly TtDbContext _context;
        private readonly TokenService _tokenService;

        public UsuarioController(TtDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        [HttpPost("cadastro")]
        public async Task<IActionResult> Register(UsuarioRegistro modelo)
        {
            //Verifica se o email já foi cadastrado
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelo.Email))
            {
                return BadRequest("Email já cadastrado.");
            }
            //Criptografa a senha se não houver email registrado
            var hash = BCrypt.Net.BCrypt.HashPassword(modelo.Senha);
            //Guarda as informações na Classe Usuario
            var user = new Usuario
            {
                Nome = modelo.NomeUsuario,
                Email = modelo.Email,
                Senha = hash
            };
            //Salva as novas informações no banco de dados
            _context.Usuarios.Add(user);
            _context.SaveChanges();
            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLogin model)
        {
            //Verifica se o email e a senha estão preencidas
            if (string.IsNullOrWhiteSpace(model.Senha) || string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest("Obrigatório email ou senha");
            }
            //Valida o email e a senha
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Senha, user.Senha))
            {
                return Unauthorized("Email ou senha inválidos.");
            }
            //Gera o Token do JWT
            var token = _tokenService.GerarToken(user);
            return Ok(new { token });
        }
    }
}
