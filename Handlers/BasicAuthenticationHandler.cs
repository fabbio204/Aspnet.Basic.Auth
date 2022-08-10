using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Aspnet.Basic.Auth.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SCHEMA = "BasicAuthentication";
    
    private readonly IConfiguration _configuration;
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }
        try
        {
            AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            
            if (authHeader.Parameter is null)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            byte[] credentialsByes = Convert.FromBase64String(authHeader.Parameter);
            string[] credentials = Encoding.UTF8.GetString(credentialsByes).Split(':');

            string? configuredUserName = await Task.Run(() =>  _configuration["BasicAuth:Username"]);
            string? configuredPassword = await Task.Run(() =>  _configuration["BasicAuth:Password"]);

            if (configuredUserName.Equals(credentials[0]) & configuredPassword.Equals(credentials[1]))
            {
                Claim[] claims = new[] {
                    new Claim(ClaimTypes.Name, credentials[0])
                };
                ClaimsIdentity identity = new(claims, Scheme.Name);
                ClaimsPrincipal principal = new(identity);
                AuthenticationTicket ticket = new(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("Invalid Credentials");
            }
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }
    }
}