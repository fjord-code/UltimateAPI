using Microsoft.AspNetCore.JsonPatch;
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

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }

        var employeeToReturn = _services.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCreationDto, false);
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, Id = employeeToReturn.Id }, employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany([FromRoute] Guid companyId, [FromRoute] Guid id)
    {
        _services.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:Guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null)
        {
            return BadRequest("EmployeeForUpdateDto object is null.");
        }

        _services.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true);

        return NoContent();
    }

    [HttpPatch("{id:Guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest("patchDoc object sent from client is null.");
        }

        var result = _services.EmployeeService.GetEmployeeForPatch(companyId, id, 
            compTrackChanges: false, empTrackChanges: true);

        patchDoc.ApplyTo(result.employeeToPatch);

        _services.EmployeeService.SaveChangesForPatch(result.employeeToPatch,
            result.employeeEntity);

        return NoContent();
    }
}
