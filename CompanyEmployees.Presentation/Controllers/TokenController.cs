using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public TokenController(
        IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("refresh")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenPair)
    {
        var tokenPairToReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenPair);

        return Ok(tokenPairToReturn);
    }
}
