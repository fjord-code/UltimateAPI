using Asp.Versioning;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Entities.Responses;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
//[ResponseCache(CacheProfileName = "120SecondsDuration")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class CompaniesController : ApiControllerBase
{
    private readonly IServiceManager _service;

    public CompaniesController(IServiceManager service)
    {
        _service = service;
    }

    /// <summary>
    /// Gets the list of all companies.
    /// </summary>
    /// <returns>A list with all companies.</returns>
    /// <response code="200">Returns a list of all companies.</response>
    /// <response code="401">No authorization.</response>
    /// <response code="403">You have no rights to access it.</response>
    [HttpGet(Name = "GetCompanies")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCompanies()
    {
        var baseResponse = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);

        return baseResponse switch
        {
            ApiOkResponse<IEnumerable<CompanyDto>> apiOkResponse => Ok(apiOkResponse.Result),
            _ => ProcessError(baseResponse)
        };
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    //[ResponseCache(Duration = 60)]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
    [HttpCacheValidation(MustRevalidate = false)]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var baseResponse = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);

        return baseResponse switch
        {
            ApiOkResponse<CompanyDto> apiOkResponse => Ok(apiOkResponse.Result),
            _ => ProcessError(baseResponse)
        };
    }

    [HttpPost(Name = "CreateCompany")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
        var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
        return Ok(companies);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);

        return CreatedAtRoute("CompanyCollection", new { ids = result.ids }, result.companies);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:Guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);

        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetCompaniesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST");

        return Ok();
    }
}
