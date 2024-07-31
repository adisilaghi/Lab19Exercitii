using Lab19Exercitii.Data;
using Lab19Exercitii.DTO;
using Lab19Exercitii.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab19Exercitii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCtrl : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public StudentCtrl(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            return await _context.Students
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Surname = s.Surname,
                    Age = s.Age,
                    AddressId = s.AddressID
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                Age = student.Age,
                AddressId = student.AddressID
            };
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(CreateStudentDTO createStudentDTO)
        {
            var student = new Student
            {
                Name = createStudentDTO.Name,
                Surname = createStudentDTO.Surname,
                Age = createStudentDTO.Age
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDTO updateStudentDTO)
        {
            if (id != updateStudentDTO.Id)
            {
                return BadRequest();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.Name = updateStudentDTO.Name;
            student.Surname = updateStudentDTO.Surname;
            student.Age = updateStudentDTO.Age;
            student.AddressID = updateStudentDTO.AddressId;

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id, [FromQuery] bool deleteAddress = false)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var addressId = student.AddressID;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            if (deleteAddress && addressId.HasValue)
            {
                var otherStudentsUsingAddress = await _context.Students
                .Where(s => s.AddressID == addressId)
                .ToListAsync();

                if (!otherStudentsUsingAddress.Any())
                {
                    var address = await _context.Addresses.FindAsync(addressId.Value);
                    if (address != null)
                    {
                        _context.Addresses.Remove(address);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                   return Ok("Address found for another student, not deleting the address.");
                }
            }

            return NoContent();
        }



        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
