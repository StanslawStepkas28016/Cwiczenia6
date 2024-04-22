using Cwiczenia6.Model;
using Microsoft.AspNetCore.Mvc;
using Cwiczenia6.Services;

namespace Cwiczenia6.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private IAnimalsService _animalsService;

    public AnimalsController(IAnimalsService animalsService)
    {
        _animalsService = animalsService;
    }

    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "name")
    {
        var animals = _animalsService.GetAnimals(orderBy);

        if (!animals.Any())
        {
            return BadRequest("Invalid orderBy parameter!");
        }

        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(Animal animal)
    {
        var res = _animalsService.AddAnimal(animal);

        if (!(res > 0))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateAnimal(int id, Animal newAnimal)
    {
        var res = _animalsService.UpdateAnimal(id, newAnimal);

        if (!(res > 0))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteAnimal(int id)
    {
        var res = _animalsService.DeleteAnimal(id);

        if (!(res > 0))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        return StatusCode(StatusCodes.Status202Accepted);
    }
}