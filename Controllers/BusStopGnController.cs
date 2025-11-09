using backend.Context;
using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusStopGnController : ControllerBase
    {
        private readonly Random _random;
        private readonly AppDbContext _context;

        private readonly int _peopleIn;
        private readonly double _percentStudents;
        private readonly double _percentChildren;
        private readonly double _percentOld;
        private readonly double _percentAnyPerson;
        private readonly double _maxPercent = 1;
        private readonly double _percentTotal;

        private readonly int _students;
        private readonly int _children;
        private readonly int _old;
        private readonly int _others;

        public BusStopGnController(AppDbContext context)
        {
            _context = context;
            _random = new Random();
            _peopleIn = _random.Next(1, 81); // total de personas

            // Limites máximos por tipo
            double maxStudents = 0.2;
            double maxChildren = 0.1;
            double maxOld = 0.35;

            // Generar porcentajes aleatorios dentro de sus límites
            _percentStudents = _random.NextDouble() * maxStudents;
            _percentChildren = _random.NextDouble() * maxChildren;
            _percentOld = _random.NextDouble() * maxOld;

            // Sumar y normalizar si excede 100%
            double totalPercent = _percentStudents + _percentChildren + _percentOld;
            if (totalPercent > 1.0)
            {
                double factor = 1.0 / totalPercent;
                _percentStudents *= factor;
                _percentChildren *= factor;
                _percentOld *= factor;
            }

            // El resto es "otros"
            _percentAnyPerson = 1.0 - (_percentStudents + _percentChildren + _percentOld);

            // Calcular cantidades
            _students = (int)Math.Floor(_percentStudents * _peopleIn);
            _children = (int)Math.Floor(_percentChildren * _peopleIn);
            _old = (int)Math.Floor(_percentOld * _peopleIn);
            _others = _peopleIn - (_students + _children + _old);
        }

        [HttpGet]
        public IActionResult GenerateBusStop(){
            var busStopGn = new BusStopGn
            {
                PeopleIn = _peopleIn,
                StudentsCantity = _students,
                ChildrenCantity = _children,
                OldCantity = _old,
                AnyPersonCantity = _others
            };

            for (int i = 0; i < _peopleIn; i++)
            {
                
            }

            return Ok(busStopGn);
        }

        [HttpGet("GetBusStop")]
        public IActionResult GetBusStop()
        {
            var busStop = new BusStopGn
            {
                PeopleIn = _peopleIn,
            };

            return Ok(busStop);
        }

        [HttpGet("GetClassifiedPeople")]
        public async Task<IActionResult> GetClassifiedPeople()
        {
            var basePeople = await _context.People.ToListAsync();

            if (basePeople.Count == 0)
                return NotFound("No hay personas en la base de datos.");

            var random = new Random();
            int totalToGenerate = random.Next(1, 81); // entre 1 y 80 personas

            // Porcentajes máximos
            double maxStudents = 0.2;
            double maxChildren = 0.1;
            double maxOld = 0.35;

            // Porcentajes aleatorios
            double pStudents = random.NextDouble() * maxStudents;
            double pChildren = random.NextDouble() * maxChildren;
            double pOld = random.NextDouble() * maxOld;

            // Normalizar si excede 100%
            double totalP = pStudents + pChildren + pOld;
            if (totalP > 1.0)
            {
                double factor = 1.0 / totalP;
                pStudents *= factor;
                pChildren *= factor;
                pOld *= factor;
            }

            int nStudents = (int)Math.Floor(pStudents * totalToGenerate);
            int nChildren = (int)Math.Floor(pChildren * totalToGenerate);
            int nOld = (int)Math.Floor(pOld * totalToGenerate);
            int nOthers = totalToGenerate - (nStudents + nChildren + nOld);

            // Generar personas clasificadas
            var classifiedPeople = new List<PersonWithCategoryDto>();

            for (int i = 0; i < totalToGenerate; i++)
            {
                var selected = basePeople[random.Next(basePeople.Count)];

                string category = 
                    selected.Age < 16 ? "Niño"
                    : selected.Age > 60 ? "Viejo"
                    : selected.Age >= 16 && selected.Age <= 25 ? "Estudiante"
                    : "Otro";


                classifiedPeople.Add(new PersonWithCategoryDto
                {
                    Id = selected.Id,
                    Age = selected.Age,
                    Gender = selected.Gender,
                    Category = category
                });
            }

            return Ok(new
            {
                Total = classifiedPeople.Count,
                Personas = classifiedPeople
            });
        }
    }
}
