using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyid}/[controller]")]
[ApiController]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _services;

    public EmployeesController(IServiceManager services)
    {
        _services = services;
    }

    [HttpGet]
    public IActionResult GetEmployees([FromRoute] Guid companyId)
    {
        return Ok(_services.EmployeeService.GetEmployees(companyId, trackChanges: false));
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployee([FromRoute] Guid companyId, [FromRoute] Guid id)
    {
        return Ok(_services.EmployeeService.GetEmployee(companyId, id, trackChanges: false));
    }
}
