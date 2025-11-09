using backend.Context;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    public class PriceController : Controller
    {

        private readonly AppDbContext _context;

        public PriceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetPrices")]
        public async Task<IActionResult> GetPrices()
        {
            var result = await _context.Prices.Select(
                x => new Price
                {
                    Id = x.Id,
                    Children = x.Children,
                    Old = x.Old,
                    Student = x.Student,
                    Any_Person = x.Any_Person
                }
                ).ToListAsync();

            return Ok(result);
        }
    }
}
