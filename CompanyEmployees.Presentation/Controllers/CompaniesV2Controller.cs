using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[ApiVersion("2.0", Deprecated = true)]
[Route("api/companies")]
[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
public class CompaniesV2Controller : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public CompaniesV2Controller(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(trackChanges: false);

        var companiesV2 = companies.Select(x => $"{x.Name} V2");

        return Ok(companiesV2);
    }
}
