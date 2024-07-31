using Lab19Exercitii.Data;
using Lab19Exercitii.Models;
using Microsoft.AspNetCore.Mvc;


namespace Lab19Exercitii.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public SeedController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SeedDatabase()
        {
            if (!_context.Students.Any() && !_context.Addresses.Any())
            {
                var address1 = new Address { City = "City1", Street = "Street1", Number = "1" };
                var address2 = new Address { City = "City2", Street = "Street2", Number = "2" };
                var address3 = new Address { City = "City3", Street = "Street3", Number = "3" };

                var student1 = new Student { Name = "Jake", Surname = "Peralta", Age = 25, Address = address1 };
                var student2 = new Student { Name = "Amy", Surname = "Santiago", Age = 22, Address = address2 };
                var student3 = new Student { Name = "Charles", Surname = "Boyle", Age = 21, Address = address3 };
                var student4 = new Student { Name = "Rosa", Surname = "Diaz", Age = 23, Address = address1 };

                _context.Addresses.AddRange(address1, address2, address3);
                _context.Students.AddRange(student1, student2, student3, student4);

                _context.SaveChanges();
            }

            return Ok();
        }
    }
}


