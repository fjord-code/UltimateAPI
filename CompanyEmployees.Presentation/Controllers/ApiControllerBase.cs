using Entities.ErrorModel;
using Entities.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Presentation.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    public IActionResult ProcessError(ApiBaseResponse baseResponse)
    {
        return baseResponse switch
        {
            ApiNotFoundResponse apiNotFoundResponse => NotFound(new ErrorDetails()
            {
                Message = apiNotFoundResponse.Message,
                StatusCode = StatusCodes.Status404NotFound
            }),
            ApiBadRequestResponse apiBadRequestResponse => BadRequest(new ErrorDetails()
            {
                Message = apiBadRequestResponse.Message,
                StatusCode = StatusCodes.Status400BadRequest
            }),
            _ => throw new NotImplementedException()
        };
    }
}
