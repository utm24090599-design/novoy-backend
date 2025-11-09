using backend.Context;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    public class BusController : Controller
    {
        private readonly AppDbContext _context;

        public BusController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetBuses")]
        public async Task<IActionResult> GetBuses()
        {
            var result = await _context.Buses.Select(
                x => new Bus
                {
                    Id = x.Id,
                    Max_Capacity = x.Max_Capacity,
                    Number_Plate = x.Number_Plate,
                    Route = x.Route
                }
                ).ToListAsync();

            return Ok(result);
        }
    }
}
