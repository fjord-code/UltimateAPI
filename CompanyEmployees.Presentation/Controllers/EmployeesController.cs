using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyid}/[controller]")]
[ApiController]
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

    [HttpGet("{id}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployee([FromRoute] Guid companyId, [FromRoute] Guid id)
    {
        return Ok(_services.EmployeeService.GetEmployee(companyId, id, trackChanges: false));
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany([FromRoute] Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreationDto)
    {
        if (employeeForCreationDto is null)
        {
            return BadRequest("EmployeeForCreationDto object is null!");
        }

        var employeeToReturn = _services.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCreationDto, false);
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.id }, employeeToReturn);
    }

}
