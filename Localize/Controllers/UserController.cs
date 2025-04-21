using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Localize.Context;
using Localize.Models;
using Localize.Services;
using System.Text.Json;
using System.Security.Claims;

namespace Localize.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class EmpresaController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TtDbContext _context;

        public EmpresaController(IHttpClientFactory httpClientFactory, TtDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        [HttpGet("listar_empresas")]
        public ActionResult<IEnumerable<Empresa>> Get()
        {
            //Identifica o Id usuario logado
            var usuario = User.FindFirst(ClaimTypes.NameIdentifier);

            if (usuario == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }
            int usuarioId = int.Parse(usuario.Value);


            //Lista as empresas salvas do Usuario
            var empresas = _context.UsuariosEmpresas.Where(ue => ue.UsuarioId == usuarioId).Select(e => new
            {
                e.Empresa.NomeEmpresarial,
                e.Empresa.NomeFantasia,
                e.Empresa.CNPJ,
                e.Empresa.Situacao,
                e.Empresa.Abertura,
                e.Empresa.Tipo,
                e.Empresa.NaturezaJuridica,
                e.Empresa.AtividadePrincipalCode,
                e.Empresa.AtividdadePrincipalText,
                Endereco = new
                {
                    e.Empresa.Endereco.Logradouro,
                    e.Empresa.Endereco.Numero,
                    e.Empresa.Endereco.Complemento,
                    e.Empresa.Endereco.Bairro,
                    e.Empresa.Endereco.Municipio,
                    e.Empresa.Endereco.UF,
                    e.Empresa.Endereco.CEP,
                }
            })
                .ToList();

            if (empresas.Count == 0)
            {
                return NotFound("Nenhuma empresa encontrada para este usuário.");
            }
            return Ok(empresas);
        }

        [HttpPost("cadastrar_cnpj/{cnpj}")]
        public async Task<IActionResult> BuscarEmpresaPorCnpj(string cnpj)
        {
            var client = _httpClientFactory.CreateClient();

            //Identifica o Id usuario logado
            var usuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (usuario == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }
            int usuarioid = int.Parse(usuario);

            //Verifica o Cnpj Inserido
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                return BadRequest("CNPJ inválido");
            }

            //Conecção com Api da receitaws
            var url = $"https://receitaws.com.br/v1/cnpj/{cnpj}";
            try
            {
                //Resultado da conecção da Api
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound("CNPJ não encontrado ou API externa com erro");
                }
                //Armazenamento das informações recebidas da Api
                var json = await response.Content.ReadAsStringAsync();
                //Retirando a informação do formato Json e guardando na Classe RespostaEmpresa
                var dados = JsonSerializer.Deserialize<RespostaEmpresa>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                //Verificando se a empresa ja foi cadastrada por outro usuario
                var resposta = await _context.Empresas.FirstOrDefaultAsync(e => e.CNPJ == dados.CNPJ);
                //Se não tiver registrado inicia o registro da nova empresa
                if (resposta == null)
                {
                    var empresa = new Empresa
                    {
                        NomeEmpresarial = dados.Nome,
                        NomeFantasia = dados.Fantasia,
                        CNPJ = dados.CNPJ,
                        Situacao = dados.Situacao,
                        Abertura = dados.Abertura,
                        Tipo = dados.Tipo,
                        NaturezaJuridica = dados.Natureza_Juridica,
                        AtividadePrincipalCode = dados.Atividade_Principal.FirstOrDefault()?.Code,
                        AtividdadePrincipalText = dados.Atividade_Principal.FirstOrDefault()?.Code,
                        Endereco = new Endereco
                        {
                            Logradouro = dados.Logradouro,
                            Numero = dados.Numero,
                            Complemento = dados.Complemento,
                            Bairro = dados.Bairro,
                            Municipio = dados.Municipio,
                            UF = dados.UF,
                            CEP = dados.CEP
                        },
                        UsuarioId = usuarioid
                    };
                    //Salva as novas informações no banco de dados
                    _context.Empresas.Add(empresa);
                    _context.SaveChanges();
                }
                //Verifica se já há uma relação com o Usuario
                bool relacaoExiste = await _context.UsuariosEmpresas.AnyAsync(ue => ue.UsuarioId == usuarioid && ue.EmpresaId == resposta.Id);
                //Se não houver cria uma nova relação
                if (!relacaoExiste)
                {
                    var usuarioempresa = new UsuarioEmpresa
                    {
                        UsuarioId = usuarioid,
                        EmpresaId = resposta.Id
                    };
                    //Salva as novas informações no banco de dados
                    _context.UsuariosEmpresas.Add(usuarioempresa);
                    await _context.SaveChangesAsync();
                    return Ok("Empresa salva");
                }
                return Ok("Empresa já relacionada");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consultar CNPJ: {ex.Message}");
            }
        }
    }
}
