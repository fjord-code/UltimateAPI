using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceProvider _serviceManager;

    public AuthenticationController(IServiceProvider serviceManager)
    {
        _serviceManager = serviceManager;
    }
}
