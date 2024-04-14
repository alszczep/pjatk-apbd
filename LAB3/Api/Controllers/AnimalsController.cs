using Api.Model;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controlllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private IAnimalsService animalsService;
    
    public AnimalsController(IAnimalsService animalsService)
    {
        this.animalsService = animalsService;
    }
    
    [HttpGet]
    public IActionResult GetAnimals([FromQuery(Name = "orderBy")] string? orderBy)
    {
        var animals = animalsService.GetAnimals(orderBy);
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult CreateAnimal(Animal animal)
    {
        var affectedCount = animalsService.CreateAnimal(animal);
        return StatusCode(StatusCodes.Status201Created);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult UpdateAnimal(int id, Animal animal)
    {
        if (id != animal.IdAnimal)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        
        var affectedCount = animalsService.UpdateAnimal(animal);
        return StatusCode(StatusCodes.Status204NoContent);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteAnimal(int id)
    {
        var affectedCount = animalsService.DeleteAnimal(id);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}