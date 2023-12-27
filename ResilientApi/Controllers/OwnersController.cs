using Microsoft.AspNetCore.Mvc;
using ResilientApi.Data.Models;
using ResilientApi.Data.Repositories;

namespace ResilientApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnersController(IOwnerRepository ownerRepository) : Controller
{
    private int _requestCount;

    // GET api/owners
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var owners = await ownerRepository.GetAllOwnersAsync();
        return Ok(owners);
    }
    
    // GET api/owners/5
    [HttpGet("{id}", Name = "GetOwnerById")]
    public async Task<IActionResult> Get(int id)
    {
        await Task.Delay(1000); // simulate a long running request
        _requestCount++;
        
        if (_requestCount % 4 == 0) // every 4th request will fail
        {
            return StatusCode(500, "Something went wrong");
        }
        
        var owner = await ownerRepository.GetOwnerByIdAsync(id);
        return Ok(owner);
    }
    
    // POST api/owners
    [HttpPost(Name = "CreateOwner")]
    public async Task<IActionResult> Post([FromBody] Owner owner)
    {
        var newOwner = await ownerRepository.CreateOwnerAsync(owner);
        return CreatedAtRoute("GetOwnerById", new { id = newOwner.Id }, newOwner);
    }
    
    // PUT api/owners/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Owner owner)
    {
        owner.Id = id;
        await ownerRepository.UpdateOwnerAsync(owner);
        return NoContent();
    }
    
    // DELETE api/owners/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var owner = await ownerRepository.GetOwnerByIdAsync(id);
        await ownerRepository.DeleteOwnerAsync(owner);
        return NoContent();
    }
}