using CompanyEmployees.Presentation.ActionFilters;
using Entities.LinkModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyid:Guid}/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _services;

    public EmployeesController(IServiceManager services)
    {
        _services = services;
    }

    [HttpGet("{id}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployeeForCompany([FromRoute] Guid companyId, [FromRoute] Guid id)
    {
        return Ok(await _services.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false));
    }

    [HttpGet]
    [HttpHead]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetEmployeesForCompany([FromRoute] Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
    {
        var linkParameters = new LinkParameters(employeeParameters, HttpContext);

        var result = await _services.EmployeeService.GetEmployeesAsync(companyId, linkParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

        return result.linkResponse.HasLinks ? 
            Ok(result.linkResponse.LinkedEntities) :
            Ok(result.linkResponse.ShapedEntities);
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
