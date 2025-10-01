using Entities.Responses;

namespace CompanyEmployees.Presentation.Extensions;

public static class ApiBaseResponseExtensions
{
    public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse)
    {
        return apiBaseResponse switch
        {
            ApiOkResponse<TResultType> apiOkResponse => apiOkResponse.Result,
            _ => throw new ArgumentException()
        };
    }
}
