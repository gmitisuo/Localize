using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Teste_Técnico_Localize.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Registro/[controller]")]
    public class ValuesController : ControllerBase
    {

        public class EmpresaController : ControllerBase
        {
            private readonly IHttpClientFactory _httpClientFactory;

            public EmpresaController(IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory;
            }

            [HttpGet("buscar_cnpj/{cnpj}")]
            public async Task<IActionResult> BuscarEmpresaPorCnpj(string cnpj)
            {
                if (string.IsNullOrWhiteSpace(cnpj))
                    return BadRequest("CNPJ inválido");

                var client = _httpClientFactory.CreateClient();
                // EXEMPLO: usar a API pública do Gov. ou Receitaws
                var url = $"https://publica.cnpj.ws/cnpj/{cnpj}";

                try
                {
                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return NotFound("CNPJ não encontrado ou API externa com erro");

                    var json = await response.Content.ReadAsStringAsync();

                    return Ok(json);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro ao consultar CNPJ: {ex.Message}");
                }
            }
        }
    }
}
