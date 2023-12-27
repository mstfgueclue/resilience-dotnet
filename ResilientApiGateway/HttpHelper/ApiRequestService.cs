using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ResilientApiGateway.HttpHelper;

public class ApiRequestService : IApiRequestService
{
    public async Task<IActionResult> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            if (!response.IsSuccessStatusCode)
            {
                return new StatusCodeResult((int)response.StatusCode);
            }

            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new OkObjectResult(data);
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}