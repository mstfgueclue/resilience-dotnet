using Microsoft.AspNetCore.Mvc;

namespace ResilientApiGateway.HttpHelper;

public interface IApiRequestService
{
    Task<IActionResult> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken);
}