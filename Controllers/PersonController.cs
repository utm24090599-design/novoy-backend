using backend.Context;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PeopleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetPeople")]
        public async Task<IActionResult> GetPeople()
        {
            var people = await _context.People
                .Select(p => new
                {
                    p.Id,
                    p.Age,
                    p.Gender
                })
                .ToListAsync();

            return Ok(new
            {
                Total = people.Count,
                People = people
            });
        }
    }
}
