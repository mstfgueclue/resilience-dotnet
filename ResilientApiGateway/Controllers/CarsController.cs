using Microsoft.AspNetCore.Mvc;
using ResilientApiGateway.Data.DTOs;
using ResilientApiGateway.HttpHelper;
using ResilientApiGateway.Strategies;

namespace ResilientApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : Controller
{
    private readonly HttpClient _client;
    private readonly ClientStrategies _clientStrategies;
    private readonly IApiRequestService _apiRequestService;

    public CarsController(IHttpClientFactory client, IApiRequestService apiRequestService)
    {
        _client = client.CreateClient("ResilientClient");
        _clientStrategies = new ClientStrategies(_client);
        _apiRequestService = apiRequestService;
    }

    // GET api/cars
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var response = await _clientStrategies.WaitAndRetry.ExecuteAsync(
            async innerToken => await _client.GetAsync("api/cars", innerToken), cancellationToken);
        
        return await _apiRequestService.HandleResponseAsync<List<Car>>(response, cancellationToken);
    }
    
    // GET api/cars/5
    [HttpGet("{id}/fallback")]
    public async Task<IActionResult> GetFallback(int id, CancellationToken cancellationToken)
    {
        var response = await _clientStrategies.Fallback.ExecuteAsync(
            async innerToken => await _client.GetAsync($"api/cars/{id}", innerToken), cancellationToken);
        
        return await _apiRequestService.HandleResponseAsync<Car>(response, cancellationToken);
    }
    
    // GET api/cars/5
    [HttpGet("{id}/wait-and-retry")]
    public async Task<IActionResult> GetWaitAndRetry(int id, CancellationToken cancellationToken)
    {
        var response = await _clientStrategies.WaitAndRetry.ExecuteAsync(
            async innerToken => await _client.GetAsync($"api/cars/{id}", innerToken), cancellationToken);
        
        return await _apiRequestService.HandleResponseAsync<Car>(response, cancellationToken);
    }
    
    // GET api/cars/5
    [HttpGet("{id}/timeout")]
    public async Task<IActionResult> GetTimeout(int id, CancellationToken cancellationToken)
    {
        var response = await _clientStrategies.Timeout.ExecuteAsync(
            async innerToken => await _client.GetAsync($"api/cars/{id}", innerToken), cancellationToken);
        
        return await _apiRequestService.HandleResponseAsync<Car>(response, cancellationToken);
    }
}