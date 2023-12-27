using Microsoft.AspNetCore.Mvc;
using ResilientApi.Data.Models;
using ResilientApi.Data.Repositories;

namespace ResilientApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController(ICarRepository carRepository) : Controller
{
    // GET api/cars
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cars = await carRepository.GetAllCarsAsync();
        return Ok(cars);
    }
    
    // GET api/cars/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        await Task.Delay(2000); // simulate a long running request
        
        // trigger a random error
        var random = new Random();
        var randomNumber = random.Next(1, 10);
        Console.WriteLine($"random number: {randomNumber}");
        if (randomNumber % 2 == 0)
        {
            return StatusCode(500, "Something went wrong");
        }
        
        var car = await carRepository.GetCarByIdAsync(id);
        return Ok(car);
    }
    
    // POST api/cars
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Car car)
    {
        var newCar = await carRepository.CreateCarAsync(car);
        return CreatedAtRoute("GetCarById", new { id = newCar.Id }, newCar);
    }
    
    // PUT api/cars/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Car car)
    {
        car.Id = id;
        await carRepository.UpdateCarAsync(car);
        return NoContent();
    }
    
    // DELETE api/cars/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await carRepository.DeleteCarAsync(id);
        return NoContent();
    }
    
    // AssignCarToOwnerAsync
    [HttpPost("{carId}/owners/{ownerId}")]
    public async Task<IActionResult> AssignCarToOwnerAsync(int carId, int ownerId)
    {
        var result = await carRepository.AssignCarToOwnerAsync(carId, ownerId);
        return Ok(result);
    }
}