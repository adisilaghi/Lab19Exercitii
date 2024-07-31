using Lab19Exercitii.Data;
using Lab19Exercitii.DTO;
using Lab19Exercitii.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab19Exercitii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   public class AddressCtrl : ControllerBase
        {
            private readonly SchoolDbContext _context;

            public AddressCtrl(SchoolDbContext context)
            {
                _context = context;
            }

            [HttpGet("{studentId}")]
            public async Task<ActionResult<AddressDTO>> GetAddress(int studentId)
            {
                var student = await _context.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == studentId);
                if (student == null || student.Address == null)
                {
                    return NotFound();
                }

                var address = student.Address;

                return new AddressDTO
                {
                    Id = address.Id,
                    City = address.City,
                    Street = address.Street,
                    Number = address.Number
                };
            }

            [HttpPut("{studentId}")]
            public async Task<IActionResult> UpdateAddress(int studentId, AddressDTO addressDTO)
            {
                var student = await _context.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == studentId);
                if (student == null)
                {
                    return NotFound();
                }

                if (student.Address == null)
                {
                    var newAddress = new Address
                    {
                        City = addressDTO.City,
                        Street = addressDTO.Street,
                        Number = addressDTO.Number
                    };

                    _context.Addresses.Add(newAddress);
                    await _context.SaveChangesAsync();

                    student.AddressID = newAddress.Id;
                }
                else
                {
                    var address = student.Address;
                    address.City = addressDTO.City;
                    address.Street = addressDTO.Street;
                    address.Number = addressDTO.Number;

                    _context.Entry(address).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();

                return NoContent();
            }

            [HttpDelete("{studentId}")]
            public async Task<IActionResult> DeleteAddress(int studentId)
            {
                var student = await _context.Students.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == studentId);
                if (student == null || student.Address == null)
                {
                    return NotFound();
                }

                var address = student.Address;
                student.AddressID = null;

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

