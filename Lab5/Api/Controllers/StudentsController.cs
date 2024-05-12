using Api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    public StudentsController()
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        //lazy loading
        //eager loading
        var dbContext = new TripsContext();
        var result = await dbContext.Countries
                                .Where(c => c.CountryName.Contains("A"))
                                .ToListAsync();

        if (result.Count() > 10)
        {
            //....
        }
        
        return Ok();
    }

}