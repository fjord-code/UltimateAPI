using CompanyEmployees.Presentation.ActionFilters;
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
    public async Task<IActionResult> GetEmployees([FromRoute] Guid companyId)
    {
        return Ok(await _services.EmployeeService.GetEmployeesAsync(companyId, trackChanges: false));
    }

    [HttpGet("{id}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployee([FromRoute] Guid companyId, [FromRoute] Guid id)
    {
        return Ok(await _services.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false));
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompany([FromRoute] Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreationDto)
    {
        var employeeToReturn = await _services.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employeeForCreationDto, false);
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, Id = employeeToReturn.Id }, employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany([FromRoute] Guid companyId, [FromRoute] Guid id)
    {
        await _services.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:Guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] EmployeeForUpdateDto employee)
    {
        await _services.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true);

        return NoContent();
    }

    [HttpPatch("{id:Guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest("patchDoc object sent from client is null.");
        }

        var result = await _services.EmployeeService.GetEmployeeForPatchAsync(companyId, id, 
            compTrackChanges: false, empTrackChanges: true);

        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }

        await _services.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch,
            result.employeeEntity);

        return NoContent();
    }
}
