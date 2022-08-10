using Aspnet.Basic.Auth.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet.Basic.Auth.Controllers;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = BasicAuthenticationHandler.SCHEMA)]
public class UsuarioController : ControllerBase
{
    [HttpGet("[action]")]
    public IActionResult Lista()
    {
        return Ok(new[] { "1","2", User.Identity?.Name});
    }
}