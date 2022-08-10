using Aspnet.Basic.Auth.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet.Basic.Auth.Controllers;

[ApiController]
[Produces("application/json")]

// Versão da API
[ApiVersion("1.0", Deprecated = false)]

// Configura a rota com a versão da API
[Route("api/v{version:apiVersion}/[controller]")]

// Usa o schema configurado no Program.cs para autenticar o usuário
// Sem essa configuração "AuthenticationSchemes" o BasicAuthenticationHandler não é chamado
[Authorize(AuthenticationSchemes = BasicAuthenticationHandler.SCHEMA)]
public class UsuarioController : ControllerBase
{
    [HttpGet("[action]")]
    public IActionResult Lista()
    {
        return Ok(new[] { "1","2", User.Identity?.Name});
    }
}